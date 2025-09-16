using Asp.Versioning;
using FinanceTracker.Api.Services.Interfaces;
using FinanceTracker.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController(ITransactionService transactionService) : ControllerBase
{
    /// <summary>
    /// Upload transactions data file
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
    //[SwaggerOperation(nameof(UploadTransaction), OperationId = nameof(UploadTransaction))]
    public async Task<IActionResult> UploadTransaction(IFormFile? file)
    {
        var result = await transactionService.UploadFileAsync(file);

        return StatusCode(result.Code, result);
    }
}
