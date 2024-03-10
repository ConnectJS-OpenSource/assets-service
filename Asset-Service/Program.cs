using System.Text;
using Asset_Service;
using Contracts;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);

Ioc.Register(builder.Services, builder.Configuration);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

Endpoints.RegisterRoot(app);

app.MapHealthChecks("/health").WithName("healthz").WithOpenApi();

app.MapGet("/", () => new { message = "Assets Service Up and Running" });
app.Run();