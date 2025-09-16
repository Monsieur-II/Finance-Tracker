using Akka.Actor;
using Akka.Hosting;
using FinanceTracker.Api.Actors;
using FinanceTracker.Api.Actors.Messages;
using FinanceTracker.Api.Services.Interfaces;
using FinanceTracker.AwsS3.Sdk.Services.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Pg.Sdk.Repositories.Interfaces;
using FinanceTracker.Utils;
using FinanceTracker.Utils.Extensions;

namespace FinanceTracker.Api.Services.Providers;

public class TransactionService(ILogger<TransactionService> logger,
    IRequiredActor<FinanceTrackerActor> requiredActor,
    IUnitOfWork unitOfWork,
    IAmazonBucketService s3Service) : ITransactionService
{
    public async Task<ApiResponse<bool>> UploadFileAsync(IFormFile? file)
    {
        try
        {
            var isValidFile = ValidateFile(file, logger);
            
            if (!isValidFile)
                return false.ToApiResponse("File is null or empty", StatusCodes.Status400BadRequest);

            var extenstion = "";
            var ingestId = Guid.NewGuid().ToString();
            var fileName = $"ingest_{ingestId}.{extenstion}";
            
            var result = await s3Service.UploadFileToBucketAsync(file!.OpenReadStream(), fileName);
            
            if (!result)
            {
                return false.ToApiResponse("Error occured while uploading transaction file", StatusCodes.Status424FailedDependency);
            }

            var ingestionRecord = new Ingestion
            {
                FileName = fileName,
                IngestedAt = DateTime.UtcNow
            };
            
            await unitOfWork.Ingestions.AddAsync(ingestionRecord);
            
            requiredActor.ActorRef.Tell(new ComputeTransactionMessage());
            
            return true.ToApiResponse("File uploaded successfully", StatusCodes.Status200OK);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while uploading transaction file");
            
            return false.ToApiResponse("Something went wrong", StatusCodes.Status500InternalServerError);
        }
    }
    
    private static bool ValidateFile(IFormFile? file, ILogger logger)
    {
        if (file is null || file.Length <= 0)
        {
            logger.LogDebug("[.] No file was uploaded or file is empty");

            return false;
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var isValidFile = !extension.Contains(UtilityConstants.FileExtensions.Xlsx)
                          && !extension.Contains(UtilityConstants.FileExtensions.Csv);

        if (isValidFile)
        {
            logger.LogDebug("[.] Invalid file extension: {Extension}", extension);

            return false;
        }

        return true;
    }
}
