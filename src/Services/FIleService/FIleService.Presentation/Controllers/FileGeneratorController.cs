using FIleService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIleService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileGeneratorController : ControllerBase
{
    private readonly IFileGeneratorService _fileGeneratorService;

    public FileGeneratorController(IFileGeneratorService fileGeneratorService)
    {
        _fileGeneratorService = fileGeneratorService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateFiles(CancellationToken cancellationToken)
    {
        var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedFiles");
        await _fileGeneratorService.GenerateFilesAsync(100, 100000, outputDirectory, cancellationToken);
        return Ok(new { message = "Files generated successfully", directory = outputDirectory });
    }
}