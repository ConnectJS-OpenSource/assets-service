using Asset_Service;
using Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

Ioc.Register(builder.Services, builder.Configuration);

var app = builder.Build();




app.MapGet("/{dir}/{file}", async (
    HttpContext ctx, 
    [FromKeyedServices("aws")] IAssetService assetsService,
    string dir,
    string file) =>
{
    return Results.File(await assetsService.GetAsset(dir, file));
});

app.MapGet("/", () => new { message = "Assets Service Up and Running" });








app.Run();