using System.Text;
using Asset_Service;
using Contracts;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);

Ioc.Register(builder.Services, builder.Configuration);

var app = builder.Build();

app.MapGet("/get", async (
    [FromKeyedServices("aws")] IAssetService assetsService,
    [FromQuery] string root,
    [FromQuery] string path) =>
{
    try
    {
        var file = Path.GetFileName(path);
        var mime = MimeTypesMap.GetMimeType(file);
        var asset = await assetsService.GetAsset(root, path);
        return Results.File(asset, contentType: mime, fileDownloadName: file, enableRangeProcessing: true);
    }
    catch (Exception ex)
    {
        return Results.NotFound();
    }
});

app.MapPost("/upload", async (HttpContext ctx,
    [FromKeyedServices("aws")] IAssetService assetsService,
    [FromQuery] string root,
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
        
        var newPath = await assetsService.PutAssetDynamic(root, path, file.FileName, file.OpenReadStream());
        if (newPath != null) return Results.Ok(new
        {
            newPath,
            url = $"/get?root={root}&path={Uri.EscapeDataString(newPath)}"
        });
        return Results.NoContent();
    }
    
    if(path.IsNullOrEmpty())
        return Results.BadRequest(new { error = "Path can't be blank" });
    
    if (!Path.HasExtension(path))
        return Results.BadRequest(new { error = "Path should contain file name" });
        
    if (Path.EndsInDirectorySeparator(path))
        return Results.BadRequest(new { error = "Remove slash from the end {path}" });
    
    
    var ok = await assetsService.PutAsset(root, path, file.OpenReadStream());
    return ok ? Results.Ok() : Results.NoContent();

}).DisableAntiforgery();

app.MapDelete("/delete", async (
    [FromKeyedServices("aws")] IAssetService assetsService,
    [FromQuery] string root,
    [FromQuery] string path
    ) =>
{
    var res = await assetsService.DeleteAsset(root, path);
    return res ? Results.Ok() : Results.NoContent();
});



app.MapHealthChecks("/health");

app.MapGet("/", () => new { message = "Assets Service Up and Running" });
app.Run();