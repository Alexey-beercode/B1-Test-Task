namespace StatementProcessingService.Application.Dtos.Request.File;

public class UploadBankStatementRequest
{
    public string FileName { get; set; }
    public Stream FileContent { get; set; }
}