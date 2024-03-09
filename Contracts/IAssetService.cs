namespace Contracts;

public interface IAssetService
{
    public Task<Stream> GetAsset(string ParentDirectory, string Path);
}