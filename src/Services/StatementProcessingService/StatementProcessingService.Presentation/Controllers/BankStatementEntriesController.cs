using Microsoft.AspNetCore.Mvc;
using StatementProcessingService.Application.Dtos.Request.File;
using StatementProcessingService.Application.Interfaces.Services;

namespace StatementProcessingService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankStatementEntriesController : ControllerBase
{
    private readonly IBankStatementEntryService _bankStatementEntryService;

    public BankStatementEntriesController(IBankStatementEntryService bankStatementEntryService)
    {
        _bankStatementEntryService = bankStatementEntryService;
    }

    /// <summary>
    /// Получает записи из файла по идентификатору файла.
    /// </summary>
    [HttpGet("{fileId}")]
    public async Task<IActionResult> GetEntriesByFileId(Guid fileId, CancellationToken cancellationToken)
    {
        var entries = await _bankStatementEntryService.GetEntriesByFileIdAsync(fileId, cancellationToken);
        return Ok(entries);
    }

    /// <summary>
    /// Получает постраничные записи из выписки.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPagedEntries([FromQuery] GetBankStatementsRequest request, CancellationToken cancellationToken)
    {
        var entries = await _bankStatementEntryService.GetPagedEntriesAsync(request, cancellationToken);
        return Ok(entries);
    }
}