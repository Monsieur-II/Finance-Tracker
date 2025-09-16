using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using FinanceTracker.AwsS3.Sdk.Configs;
using FinanceTracker.AwsS3.Sdk.Services.Interfaces;
using FinanceTracker.Utils.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinanceTracker.AwsS3.Sdk.Services.Providers;

public class AmazonBucketService(IOptions<AwsS3Config> config, ILogger<AmazonBucketService> logger) : IAmazonBucketService
{
    public async Task<bool> UploadFileToBucketAsync(Stream fileStream, string fileName, string fileType = "image/png")
    {
        try
        {
            var amazonConfig = config.Value;
            
            var client = new AmazonS3Client(amazonConfig.AccessKeyId, amazonConfig.SecretAccessKey, RegionEndpoint.EUNorth1);

            var request = new PutObjectRequest
            {
                InputStream = fileStream,
                BucketName = amazonConfig.S3BucketName,
                Key = $"{amazonConfig.FolderName}/{fileName}"
            };
            request.Metadata.Add("type", fileType);

            var response = await client.PutObjectAsync(request);
            
            logger.LogDebug("Response after file upload to s3: {Response}", response.Serialize());

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while uploading file to s3 bucket");

            return false;
        }
    }
}
