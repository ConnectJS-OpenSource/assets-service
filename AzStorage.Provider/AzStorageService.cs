using Azure.Identity;
using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzStorage.Provider
{
    public class AzStorageService(ClientSecretCredential credential) : IAssetService
    {
        public Task<Stream> GetAsset(string ParentDirectory, string Path)
        {
            throw new NotImplementedException();
        }
    }
}
