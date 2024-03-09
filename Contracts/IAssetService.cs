namespace Contracts;

public interface IAssetService
{
    Task<bool> Ping();
    Task<Stream> GetAsset(string root, string Path);
    Task<bool> PutAsset(string root, string? path, Stream fileStream);
    Task<string?> PutAssetDynamic(string root, string path, string filename, Stream fileStream);
    Task<bool> DeleteAsset(string root, string path);
    Task<bool> Exists(string root, string path);
    Task<bool> Move(string root, string from, string to);
}