using Microsoft.AspNetCore.Mvc;
using StatementProcessingService.Application.Dtos.Request.File;
using StatementProcessingService.Application.Interfaces.Services;

namespace StatementProcessingService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    private readonly IFileService _fileService;

    public ExportController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Экспортирует файл банковской выписки в указанный формат.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ExportFile([FromBody] ExportBankStatementRequest request, CancellationToken cancellationToken)
    {
        var response = await _fileService.ExportFileAsync(request, cancellationToken);
        return File(response.FileContent, response.ContentType, response.FileName);
    }
}