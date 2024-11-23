namespace StatementProcessingService.Application.Dtos.Request.File;

public class ExportBankStatementRequest
{
    public Guid FileId { get; set; }
    public string ExportFormat { get; set; } // Excel, CSV, PDF
}