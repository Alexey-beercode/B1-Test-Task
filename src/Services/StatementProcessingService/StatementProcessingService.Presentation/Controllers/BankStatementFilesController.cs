using Microsoft.AspNetCore.Mvc;
using StatementProcessingService.Application.Dtos.Request.File;
using StatementProcessingService.Application.Interfaces.Services;

namespace StatementProcessingService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankStatementFilesController : ControllerBase
{
    private readonly IBankStatementFileService _bankStatementFileService;

    public BankStatementFilesController(IBankStatementFileService bankStatementFileService)
    {
        _bankStatementFileService = bankStatementFileService;
    }

    /// <summary>
    /// Загружает файл банковской выписки.
    /// </summary>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] UploadBankStatementRequest request, CancellationToken cancellationToken)
    {
        var response = await _bankStatementFileService.UploadFileAsync(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Получает список файлов банковских выписок.
    /// </summary>
    [HttpGet("files")]
    public async Task<IActionResult> GetFilesList([FromQuery] GetBankStatementsRequest request, CancellationToken cancellationToken)
    {
        var files = await _bankStatementFileService.GetFilesListAsync(request, cancellationToken);
        return Ok(files);
    }

    /// <summary>
    /// Получает детали конкретного файла.
    /// </summary>
    [HttpGet("files/{fileId}")]
    public async Task<IActionResult> GetFileDetails(Guid fileId, CancellationToken cancellationToken)
    {
        var fileDetails = await _bankStatementFileService.GetFileDetailsAsync(fileId, cancellationToken);
        return Ok(fileDetails);
    }
}