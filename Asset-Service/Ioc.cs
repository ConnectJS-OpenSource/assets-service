using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using AWS.S3.Provider;
using AzStorage.Provider;
using Azure.Identity;
using Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Asset_Service;

public class Ioc
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddAuthentication().AddBearerToken("LocalAuthIssuer");
        services.AddAuthorizationBuilder().AddPolicy("auth", config =>
        {
            config.AddRequirements(new CustomTokenRequirement(configuration));
        });

        services.AddSingleton<IAuthorizationHandler, CustomTokenRequirementHandler>();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var aws = new
        {
            creds = new BasicAWSCredentials(
                configuration.GetSection("AWS:ACCESS_KEY_ID").Get<string>(),
                configuration.GetSection("AWS:SECRET_ACCESS_KEY").Get<string>()
            ),
            config = new AmazonS3Config
            {
                RegionEndpoint =
                    RegionEndpoint.GetBySystemName(configuration.GetSection("AWS:DEFAULT_REGION").Get<string>()),
                AllowAutoRedirect = true
            },
            bucket = configuration.GetSection("AWS:BUCKET").Get<string>()
        };
        
        services.AddDefaultAWSOptions(new AWSOptions
        {
            Region = aws.config.RegionEndpoint,
            Credentials = aws.creds
        });

        services.AddScoped<IClientConfig>(f => aws.config);

        services.AddSingleton<ClientSecretCredential>(f => new ClientSecretCredential(
            configuration.GetSection("Azure:TenantId").Get<string>(),
            configuration.GetSection("Azure:ClientId").Get<string>(),
            configuration.GetSection("Azure:ClientSecret").Get<string>()
        ));
        
        services.AddAWSService<IAmazonS3>();

        if(configuration.GetValue<string>("Provider") == "AWS")
            services.AddHealthChecks().AddCheck<S3Service>("s3-check");
        
        if(configuration.GetValue<string>("Provider") == "Azure")
            services.AddHealthChecks().AddCheck<AzStorageService>("azure-check");
        
        services.AddScoped<S3Service>(f => new S3Service(f.GetRequiredService<IAmazonS3>(),
            configuration.GetSection("AWS:BUCKET").Get<string>() ?? ""));
        services.AddScoped<AzStorageService>(f => new AzStorageService(f.GetRequiredService<ClientSecretCredential>(),
            configuration.GetSection("Azure:Container").Get<string>() ?? ""));
        
        services.AddKeyedScoped<IAssetService, S3Service>("aws", (f,i) => f.GetRequiredService<S3Service>());
        services.AddKeyedScoped<IAssetService, AzStorageService>("azure", (f,i) => f.GetRequiredService<AzStorageService>());
    }
}