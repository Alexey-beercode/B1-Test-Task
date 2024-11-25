using Microsoft.AspNetCore.Mvc;
using StatementProcessingService.Application.Dtos.Request.File;
using StatementProcessingService.Application.Interfaces.Services;

namespace StatementProcessingService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankStatementFilesController : ControllerBase
{
    private readonly IBankStatementFileService _bankStatementFileService;
    private readonly ILogger<BankStatementFilesController> _logger;

    public BankStatementFilesController(IBankStatementFileService bankStatementFileService, ILogger<BankStatementFilesController> logger)
    {
        _bankStatementFileService = bankStatementFileService;
        _logger = logger;
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
    public async Task<IActionResult> GetFilesList( CancellationToken cancellationToken)
    {
        var files = await _bankStatementFileService.GetFilesListAsync( cancellationToken);
        _logger.LogInformation("files :" + files.ToString());
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