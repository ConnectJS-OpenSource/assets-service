using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using AWS.S3.Provider;
using AzStorage.Provider;
using Azure.Identity;
using Contracts;

namespace Asset_Service;

public class Ioc
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();


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
            }
        };
        
        services.AddDefaultAWSOptions(new AWSOptions
        {
            Region = aws.config.RegionEndpoint,
            Credentials = aws.creds
        });

        services.AddSingleton<ClientSecretCredential>(f => new ClientSecretCredential(
            configuration.GetSection("Azure").GetValue<string>("TenantId"),
            configuration.GetSection("Azure").GetValue<string>("ClientId"),
            configuration.GetSection("Azure").GetValue<string>("ClientSecret")
        ));
        
        services.AddAWSService<IAmazonS3>();
        
        services
            .AddHealthChecks()
            .AddS3(options =>
            {
                options.Credentials = aws.creds;
                options.S3Config = aws.config;
            });

        services.AddKeyedScoped<IAssetService, S3Service>("aws");
        services.AddKeyedScoped<IAssetService, AzStorageService>("azure");
    }
}