using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Contracts;

namespace AWS.S3.Provider;

public class S3Service(AWSOptions awsOptions, IAmazonS3 amazonS3Client) : IAssetService
{
    public async Task<Stream> GetAsset(string parentDirectory, string path)
    {
        var obj = await amazonS3Client.GetObjectAsync(parentDirectory, path);
        return obj.ResponseStream;
    }
}