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
        services.AddDefaultAWSOptions(new AWSOptions
        {
            Region = RegionEndpoint.GetBySystemName(configuration.GetSection("AWS").GetValue<string>("DEFAULT_REGION")),
            Credentials = new BasicAWSCredentials(
                configuration.GetSection("AWS").GetValue<string>("ACCESS_KEY_ID"),
                configuration.GetSection("AWS").GetValue<string>("SECRET_ACCESS_KEY")
            )
        });

        services.AddSingleton<ClientSecretCredential>(f => new ClientSecretCredential(
            configuration.GetSection("Azure").GetValue<string>("TenantId"),
            configuration.GetSection("Azure").GetValue<string>("ClientId"),
            configuration.GetSection("Azure").GetValue<string>("ClientSecret")
        ));

        services.AddScoped<IClientConfig>(f => new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(configuration.GetSection("aws").GetValue<string>("region")),
            AllowAutoRedirect = true
        });

        services.AddAWSService<IAmazonS3>();

        services.AddKeyedScoped<IAssetService, S3Service>("aws");
        services.AddKeyedScoped<IAssetService, AzStorageService>("azure");
    }
}