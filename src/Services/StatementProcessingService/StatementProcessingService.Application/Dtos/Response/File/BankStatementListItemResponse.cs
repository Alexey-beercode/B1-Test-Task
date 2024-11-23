namespace StatementProcessingService.Application.Dtos.Response.File;

public class BankStatementListItemResponse
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public DateTime UploadDate { get; set; }
    public int EntriesCount { get; set; }
}