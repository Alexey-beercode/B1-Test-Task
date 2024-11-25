using Microsoft.AspNetCore.Http;

namespace StatementProcessingService.Application.Dtos.Request.File;

public class UploadBankStatementRequest
{
    public string FileName { get; set; }
    public IFormFile FileContent { get; set; }
}