using FIleService.Application.Interfaces;
using FIleService.Domain.Entities;
using FIleService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FIleService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileMergeController : ControllerBase
{
    private readonly IFileMergeService _fileMergeService;

    public FileMergeController(IFileMergeService fileMergeService)
    {
        _fileMergeService = fileMergeService;
    }

    [HttpPost("merge/{patternToExclude}")]
    public async Task<ActionResult<MergeResult>> MergeFiles(string patternToExclude,
        CancellationToken cancellationToken)
    {
        var sourceDirectory = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedFiles");
        var outputFilePath = Path.Combine(
            sourceDirectory,
            $"merged_{DateTime.Now:yyyyMMddHHmmss}.txt");

        var result = await _fileMergeService.MergeFilesAsync(
            sourceDirectory,
            outputFilePath,
            patternToExclude,
            cancellationToken);

        return Ok(result);
    }
}