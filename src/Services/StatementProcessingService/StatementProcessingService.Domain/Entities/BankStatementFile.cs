using StatementProcessingService.Domain.Common;

namespace StatementProcessingService.Domain.Entities;

public class BankStatementFile : BaseEntity
{
    public string FileName { get; set; }
    public DateTime UploadDate { get; set; }
    public IEnumerable<BankStatementEntry> Entries { get; set; }
}