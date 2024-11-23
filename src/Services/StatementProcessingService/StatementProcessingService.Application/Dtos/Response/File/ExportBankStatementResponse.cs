namespace StatementProcessingService.Application.Dtos.Response.File;

public class ExportBankStatementResponse
{
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
    public string ContentType { get; set; }
}