using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Contracts;

namespace AWS.S3.Provider;

public class S3Service(IAmazonS3 amazonS3Client) : IAssetService
{
    public async Task<bool> Ping()
    {
        var res = await amazonS3Client.GetBucketLocationAsync(new GetBucketLocationRequest
        {
            BucketName = "test"
        });

        return res.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<Stream> GetAsset(string root, string path)
    {
        var obj = await amazonS3Client.GetObjectAsync(root, path);
        return obj.ResponseStream;
    }

    public async Task<bool> PutAsset(string root, string? path, Stream fileStream)
    {
        var result = await amazonS3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = root,
            Key = path,
            InputStream = fileStream
        });
        
        return result.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<string?> PutAssetDynamic(string root, string path, string filename, Stream fileStream)
    {
        string ext = Path.GetExtension(filename);
        string? newFilepath = $"{path}{DateTime.Now:yyyy-MM-dd}/{Guid.NewGuid()}{ext}";
        var res = await PutAsset(root, newFilepath, fileStream);
        return res ? newFilepath : null;
    }

    public async Task<bool> DeleteAsset(string root, string path)
    {
        var res = await amazonS3Client.DeleteObjectAsync(new DeleteObjectRequest
        {
            BucketName = root,
            Key = path
        });

        return res.HttpStatusCode == HttpStatusCode.OK;

    }

    public Task<bool> Exists(string root, string path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Move(string root, string from, string to)
    {
        throw new NotImplementedException();
    }
}