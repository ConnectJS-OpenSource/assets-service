using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using AWS.S3.Provider;
using AzStorage.Provider;
using Azure.Identity;
using Contracts;
using Microsoft.Extensions.Azure;
using System.Diagnostics.CodeAnalysis;

namespace Asset_Service
{
    public class Ioc
    {
        public static void Register(IServiceCollection Services, IConfiguration configuration)
        {
            Services.AddHttpContextAccessor();
            Services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = RegionEndpoint.GetBySystemName(configuration.GetSection("AWS").GetValue<string>("DEFAULT_REGION")),
                Credentials = new BasicAWSCredentials(
                        accessKey: configuration.GetSection("AWS").GetValue<string>("ACCESS_KEY_ID"),
                        secretKey: configuration.GetSection("AWS").GetValue<string>("SECRET_ACCESS_KEY")
                    )
            });

            Services.AddSingleton<ClientSecretCredential>(f =>
            {
                return new ClientSecretCredential(
                    tenantId: configuration.GetSection("Azure").GetValue<string>("TenantId"),
                    clientId: configuration.GetSection("Azure").GetValue<string>("ClientId"),
                    clientSecret: configuration.GetSection("Azure").GetValue<string>("ClientSecret")
                );
            });

            Services.AddScoped<IClientConfig>(f =>
            {
                return new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(configuration.GetSection("aws").GetValue<string>("region")),
                    AllowAutoRedirect = true
                };
            });

            Services.AddAWSService<IAmazonS3>();

            Services.AddKeyedScoped<IAssetService, S3Service>("aws");
            Services.AddKeyedScoped<IAssetService, AzStorageService>("azure");
        }
    }
}
