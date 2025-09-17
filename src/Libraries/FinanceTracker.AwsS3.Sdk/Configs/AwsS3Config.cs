namespace FinanceTracker.AwsS3.Sdk.Configs;

public class AwsS3Config
{
    public string AccessKeyId { get; set; } = null!;
    public string SecretAccessKey { get; set; } = null!;
    public string FolderName { get; set; } = null!;
    public string S3BucketName { get; set; } = null!;
    public string BaseUrl { get; set; } = null!;
}
