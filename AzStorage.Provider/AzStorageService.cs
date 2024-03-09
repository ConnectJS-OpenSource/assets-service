using Azure.Identity;
using Contracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AzStorage.Provider;

public class AzStorageService(ClientSecretCredential credential, string container) : IAssetService
{
    public Task<bool> Ping()
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetAsset(string path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PutAsset(string? path, Stream fileStream)
    {
        throw new NotImplementedException();
    }

    public Task<string?> PutAssetDynamic(string path, string filename, Stream fileStream)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsset(string path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(string path)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Move(string from, string to)
    {
        throw new NotImplementedException();
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}