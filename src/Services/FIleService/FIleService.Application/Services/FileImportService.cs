using System.Globalization;
using FIleService.Application.Hubs;
using FIleService.Application.Interfaces;
using FIleService.Domain.Entities;
using FIleService.Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace FIleService.Application.Services;

public class FileImportService : IFileImportService
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<ImportProgressHub> _hubContext;
    private readonly ILogger<FileImportService> _logger;
    private const int BatchSize = 1000;
    private const string DateFormat = "dd.MM.yyyy";

    public FileImportService(
        ApplicationDbContext context,
        IHubContext<ImportProgressHub> hubContext,
        ILogger<FileImportService> logger)
    {
        _context = context;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task ImportFilesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var sourceDirectory = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedFiles");
            var files = Directory.GetFiles(sourceDirectory, "*.txt");
            var totalLines = files.Sum(file => File.ReadLines(file).Count());
            var processedLines = 0;
            var batch = new List<DataRow>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                _logger.LogInformation("Processing file: {FileName}", fileName);

                foreach (var line in File.ReadLines(file))
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    var dataRow = ParseLine(line);
                    if (dataRow != null)
                    {
                        batch.Add(dataRow);
                        processedLines++;

                        if (batch.Count >= BatchSize)
                        {
                            await SaveBatchAsync(batch, processedLines, totalLines, cancellationToken);
                            batch.Clear();
                        }
                    }
                }
            }

            if (batch.Any())
            {
                await SaveBatchAsync(batch, processedLines, totalLines, cancellationToken);
            }

            await UpdateProgressAsync(processedLines, totalLines, "Import completed");
            _logger.LogInformation("Import completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during import process");
            throw;
        }
    }

    private DataRow? ParseLine(string line)
    {
        var parts = line.Split("||", StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 5)
            return null;

        try
        {
            return DataRow.Create(
                DateTime.ParseExact(parts[0], DateFormat, CultureInfo.InvariantCulture),
                parts[1],
                parts[2],
                long.Parse(parts[3]),
                decimal.Parse(parts[4], CultureInfo.InvariantCulture)
            );
        }
        catch
        {
            return null;
        }
    }

    private async Task SaveBatchAsync(List<DataRow> batch, int processedLines, int totalLines, 
        CancellationToken cancellationToken)
    {
        await _context.DataRows.AddRangeAsync(batch, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await UpdateProgressAsync(processedLines, totalLines);
    }

    private async Task UpdateProgressAsync(int processed, int total, string? status = null)
    {
        var message = status ?? $"Processed {processed:N0} of {total:N0} lines";
        await _hubContext.Clients.All.SendAsync("ReceiveProgress", processed, total, message);
    }
}