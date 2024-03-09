using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Contracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AWS.S3.Provider;

public class S3Service(IAmazonS3 amazonS3Client, string bucketName) : IAssetService
{
    private readonly IAmazonS3 _amazonS3Client = amazonS3Client;
    private readonly string _bucketName = bucketName;

    public async Task<bool> Ping()
    {
        var res = await _amazonS3Client.ListObjectsAsync(new ListObjectsRequest
        {
            BucketName = _bucketName,
            MaxKeys = 1
        });

        return res.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<Stream> GetAsset(string path)
    {
        var obj = await _amazonS3Client.GetObjectAsync(_bucketName, path);
        return obj.ResponseStream;
    }

    public async Task<bool> PutAsset(string? path, Stream fileStream)
    {
        var result = await _amazonS3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = path,
            InputStream = fileStream
        });
        
        return result.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<string?> PutAssetDynamic(string path, string filename, Stream fileStream)
    {
        var ext = Path.GetExtension(filename);
        var newFilepath = $"{path}{DateTime.Now:yyyy-MM-dd}/{Guid.NewGuid()}{ext}";
        var res = await PutAsset(newFilepath, fileStream);
        return res ? newFilepath : null;
    }

    public async Task<bool> DeleteAsset(string path)
    {
        var res = await _amazonS3Client.DeleteObjectAsync(new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = path
        });

        return res.HttpStatusCode == HttpStatusCode.OK;

    }

    public Task<bool> Exists(string path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Move(string from, string to)
    {
        throw new NotImplementedException();
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var ping = await Ping();
        if (ping)
            return HealthCheckResult.Healthy();

        return HealthCheckResult.Unhealthy();
    }
}