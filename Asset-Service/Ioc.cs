using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using AWS.S3.Provider;
using AzStorage.Provider;
using Contracts;
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
                Region = RegionEndpoint.GetBySystemName(configuration.GetSection("aws").GetValue<string>("region")),
                Credentials = new BasicAWSCredentials(
                        accessKey: configuration.GetSection("aws").GetValue<string>("key"),
                        secretKey: configuration.GetSection("aws").GetValue<string>("secret")
                    )
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
            //Services.AddKeyedScoped<IAssetService, AzStorageService>("azure");
        }
    }
}
