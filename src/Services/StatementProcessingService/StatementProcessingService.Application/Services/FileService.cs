using StatementProcessingService.Application.Dtos.Request.File;
using StatementProcessingService.Application.Dtos.Response.Entries;
using StatementProcessingService.Application.Dtos.Response.File;
using StatementProcessingService.Application.Interfaces.Services;

namespace StatementProcessingService.Application.Services;

public class FileService : IFileService
{
    private readonly IExcelParserService _excelParserService;
    private readonly IBankStatementEntryService _entryService;

    public FileService(IExcelParserService excelParserService, IBankStatementEntryService entryService)
    {
        _excelParserService = excelParserService;
        _entryService = entryService;
    }

    public async Task<ExportBankStatementResponse> ExportFileAsync(
        ExportBankStatementRequest request, CancellationToken cancellationToken = default)
    {
        // Получаем записи файла
        var entries = await _entryService.GetEntriesByFileIdAsync(request.FileId, cancellationToken);

        var statement = new BankStatementDetailsResponse
        {
            Id = request.FileId,
            Entries = entries.ToList()
        };

        // Генерируем файл в зависимости от формата
        var (fileContent, contentType, fileExtension) = request.ExportFormat.ToLower() switch
        {
            "excel" => (
                await _excelParserService.GenerateExcelFileAsync(statement, cancellationToken),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "xlsx"
            ),
            "csv" => (
                GenerateCsvFile(statement),
                "text/csv",
                "csv"
            ),
            _ => throw new NotSupportedException($"Export format {request.ExportFormat} is not supported.")
        };

        return new ExportBankStatementResponse
        {
            FileName = $"{statement.Id}.{fileExtension}",
            FileContent = fileContent,
            ContentType = contentType
        };
    }

    private byte[] GenerateCsvFile(BankStatementDetailsResponse statement)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        
        writer.WriteLine("AccountNumber,InitialBalanceActive,InitialBalancePassive,TurnoverDebit,TurnoverCredit,FinalBalanceActive,FinalBalancePassive");

        // Данные записей
        foreach (var entry in statement.Entries)
        {
            writer.WriteLine($"{entry.AccountNumber},{entry.InitialBalanceActive},{entry.InitialBalancePassive},{entry.TurnoverDebit},{entry.TurnoverCredit},{entry.FinalBalanceActive},{entry.FinalBalancePassive}");
        }

        writer.Flush();
        return memoryStream.ToArray();
    }
}