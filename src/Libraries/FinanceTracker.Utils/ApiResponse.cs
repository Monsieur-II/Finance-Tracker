namespace FinanceTracker.Utils;

public sealed record ApiResponse<T>(string Message, int Code, T Data);
