using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    
    public TransactionType Type { get; set; }
    public string? Category { get; set; }
    
    public string? AccountName { get; set; }
}
