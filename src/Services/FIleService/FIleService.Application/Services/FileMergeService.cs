using FIleService.Application.Interfaces;
using FIleService.Domain.Entities;
using FIleService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace FIleService.Application.Services;

public class FileMergeService : IFileMergeService
{
    private readonly ILogger<FileMergeService> _logger;

    public FileMergeService(ILogger<FileMergeService> logger)
    {
        _logger = logger;
    }

    public async Task<MergeResult> MergeFilesAsync(
        string sourceDirectory,
        string outputFilePath,
        string patternToExclude,
        CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.Now;
        var result = new MergeResult
        {
            OutputFilePath = outputFilePath
        };

        try
        {
            var files = Directory.GetFiles(sourceDirectory, "*.txt");
            if (!files.Any())
            {
                throw new FileNotFoundException("No text files found in the specified directory.");
            }

            using var outputStream = new StreamWriter(outputFilePath, false);
            foreach (var file in files)
            {
                var (processedLines, excludedLines) = await ProcessFileAsync(
                    file, outputStream, patternToExclude, cancellationToken);
                
                result.TotalLinesProcessed += processedLines;
                result.ExcludedLinesCount += excludedLines;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during file merge operation");
            throw;
        }

        result.ProcessingTime = DateTime.Now - startTime;
        return result;
    }

    private async Task<(int ProcessedLines, int ExcludedLines)> ProcessFileAsync(
        string inputFilePath,
        StreamWriter outputStream,
        string patternToExclude,
        CancellationToken cancellationToken)
    {
        var processedLines = 0;
        var excludedLines = 0;

        using var inputStream = new StreamReader(inputFilePath);
        string line;
        while ((line = await inputStream.ReadLineAsync()) != null)
        {
            cancellationToken.ThrowIfCancellationRequested();
            processedLines++;

            if (string.IsNullOrEmpty(patternToExclude) || !line.Contains(patternToExclude))
            {
                await outputStream.WriteLineAsync(line);
            }
            else
            {
                excludedLines++;
            }
        }

        return (processedLines, excludedLines);
    }
}