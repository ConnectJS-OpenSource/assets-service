using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Contracts;

public interface IAssetService: IHealthCheck
{
    Task<bool> Ping();
    Task<Stream> GetAsset(string path);
    Task<bool> PutAsset(string? path, Stream fileStream);
    Task<string?> PutAssetDynamic(string path, string filename, Stream fileStream);
    Task<bool> DeleteAsset(string path);
    Task<bool> Exists(string path);
    Task<bool> Move(string from, string to);
}