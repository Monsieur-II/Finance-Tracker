using FinanceTracker.Utils;

namespace FinanceTracker.Api.Services.Interfaces;

public interface ITransactionService
{
    Task<ApiResponse<bool>> UploadFileAsync(IFormFile? file);
}
