using FIleService.Domain.Entities;
using FIleService.Domain.Models;

namespace FIleService.Application.Interfaces;

public interface IFileMergeService
{
    Task<MergeResult> MergeFilesAsync(string sourceDirectory, string outputFilePath, string patternToExclude, CancellationToken cancellationToken = default);
}