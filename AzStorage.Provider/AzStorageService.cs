using Azure.Identity;
using Contracts;

namespace AzStorage.Provider;

public class AzStorageService(ClientSecretCredential credential) : IAssetService
{
    public Task<Stream> GetAsset(string ParentDirectory, string Path)
    {
        throw new NotImplementedException();
    }
}