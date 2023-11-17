using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.S3.Provider
{
    public class S3Service: IAssetService
    {
        private readonly AWSOptions _awsOptions;
        private readonly IAmazonS3 _amazonS3Client;

        public S3Service(AWSOptions AwsOptions, IAmazonS3 amazonS3Client)
        {
            this._awsOptions = AwsOptions;
            _amazonS3Client = amazonS3Client;
        }

        public async Task<Stream> GetAsset(string ParentDirectory, string Path)
        {
            var obj = await this._amazonS3Client.GetObjectAsync(ParentDirectory, Path);
            
            return obj.ResponseStream;
        }

    }
}
