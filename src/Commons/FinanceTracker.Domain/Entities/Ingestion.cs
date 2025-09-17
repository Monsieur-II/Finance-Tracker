namespace FinanceTracker.Domain.Entities;

public class Ingestion : BaseEntity
{
    public string FileName { get; set; } = null!;
    public DateTime IngestedAt { get; set; }
    public string Status { get; set; } = "Staged";
    public string Extension { get; set; } = null!;
}
