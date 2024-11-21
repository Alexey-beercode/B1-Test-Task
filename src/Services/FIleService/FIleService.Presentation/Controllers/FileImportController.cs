using FIleService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIleService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileImportController : ControllerBase
{
    private readonly IFileImportService _importService;

    public FileImportController(IFileImportService importService)
    {
        _importService = importService;
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportFiles(CancellationToken cancellationToken)
    {
        await _importService.ImportFilesAsync(cancellationToken);
        return Ok();
    }
}