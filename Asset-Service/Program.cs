using Asset_Service;
using Contracts;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);

Ioc.Register(builder.Services, builder.Configuration);

var app = builder.Build();

app.MapGet("/download", async (
    HttpContext ctx,
    [FromKeyedServices("aws")] IAssetService assetsService,
    [FromQuery] string dir,
    [FromQuery] string path) =>
{
    try
    {
        return Results.File(await assetsService.GetAsset(dir, path));
    }
    catch (Exception ex)
    {
        return Results.NotFound();
    }
});

app.MapGet("/", () => new { message = "Assets Service Up and Running" });

app.Run();