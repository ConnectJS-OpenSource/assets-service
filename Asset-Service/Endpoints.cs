using Contracts;
using HeyRed.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Asset_Service;

public class Endpoints
{
    public static void RegisterRoot(WebApplication app)
    {
        app.MapGet("/get", async (
            [FromKeyedServices("aws")] IAssetService assetsService,
            [FromQuery] string path) =>
        {
            try
            {
                var file = Path.GetFileName(path);
                var mime = MimeTypesMap.GetMimeType(file);
                var asset = await assetsService.GetAsset(path);
                return Results.File(asset, contentType: mime, fileDownloadName: file, enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                return Results.NotFound();
            }
        }).RequireAuthorization("auth").WithName("Get Asset").WithOpenApi();

        app.MapPost("/upload", async (HttpContext ctx,
            [FromKeyedServices("aws")] IAssetService assetsService,
            [FromQuery] string path,
            [FromQuery] bool autogen = true,
            IFormFile? file = null)=>
        {
            if (file == null)
                return Results.BadRequest();

            if (autogen)
            {
                if (Path.HasExtension(path))
                    return Results.BadRequest(new { error = "Path can't contain file name" });
                
                if (Path.EndsInDirectorySeparator(path))
                    return Results.BadRequest(new { error = "Remove slash from the end {path}" });

                path = (path.IsNotNullOrEmpty() ? path + "/" : "");
                
                var newPath = await assetsService.PutAssetDynamic(path, file.FileName, file.OpenReadStream());
                if (newPath != null) return Results.Ok(new
                {
                    newPath,
                    url = $"/get?path={Uri.EscapeDataString(newPath)}"
                });
                return Results.NoContent();
            }
            
            if(path.IsNullOrEmpty())
                return Results.BadRequest(new { error = "Path can't be blank" });
            
            if (!Path.HasExtension(path))
                return Results.BadRequest(new { error = "Path should contain file name" });
                
            if (Path.EndsInDirectorySeparator(path))
                return Results.BadRequest(new { error = "Remove slash from the end {path}" });
            
            
            var ok = await assetsService.PutAsset(path, file.OpenReadStream());
            return ok ? Results.Ok() : Results.NoContent();

        }).DisableAntiforgery().RequireAuthorization("auth").WithName("Upload Asset").WithOpenApi();

        app.MapDelete("/delete", async (
            [FromKeyedServices("aws")] IAssetService assetsService,
            [FromQuery] string path
            ) =>
        {
            var res = await assetsService.DeleteAsset(path);
            return res ? Results.Ok() : Results.NoContent();
        }).DisableAntiforgery().RequireAuthorization("auth").WithName("Delete Asset").WithOpenApi();
    }
}