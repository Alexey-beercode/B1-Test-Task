using System;

namespace StatementProcessingService.Application.Dtos.Response.File;

public class UploadBankStatementResponse
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public DateTime UploadDate { get; set; }
}