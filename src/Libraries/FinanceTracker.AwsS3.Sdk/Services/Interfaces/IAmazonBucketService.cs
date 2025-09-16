namespace FinanceTracker.AwsS3.Sdk.Services.Interfaces;

public interface IAmazonBucketService
{
    Task<bool> UploadFileToBucketAsync(Stream fileStream, string fileName, string fileType = "image/png");
}
