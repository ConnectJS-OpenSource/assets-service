using Azure.Identity;
using Contracts;

namespace AzStorage.Provider;

public class AzStorageService(ClientSecretCredential credential) : IAssetService
{
    public Task<bool> Ping()
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetAsset(string root, string Path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PutAsset(string root, string? path, Stream fileStream)
    {
        throw new NotImplementedException();
    }

    public Task<string?> PutAssetDynamic(string root, string path, string filename, Stream fileStream)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsset(string root, string path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(string root, string path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Move(string root, string from, string to)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PutAsset(string root, string path)
    {
        throw new NotImplementedException();
    }
}