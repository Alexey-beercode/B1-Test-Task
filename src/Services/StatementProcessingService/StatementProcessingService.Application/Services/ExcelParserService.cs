using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using StatementProcessingService.Application.Dtos.Response.Entries;
using StatementProcessingService.Application.Dtos.Response.File;
using StatementProcessingService.Application.Interfaces.Services;

namespace StatementProcessingService.Application.Services;

public class ExcelParserService : IExcelParserService
{
    private readonly ILogger<ExcelParserService> _logger;

    public ExcelParserService(ILogger<ExcelParserService> logger)
    {
        _logger = logger;
    }

 public async Task<IEnumerable<BankStatementEntryResponse>> ParseExcelFileAsync(
    IFormFile file, 
    CancellationToken cancellationToken)
{
    _logger.LogInformation("Starting Excel file parsing. File: {FileName}, Size: {Size} bytes, ContentType: {ContentType}",
        file.FileName, file.Length, file.ContentType);

    ValidateFile(file);

    var entries = new List<BankStatementEntryResponse>();
    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

    using var stream = new MemoryStream();
    await file.CopyToAsync(stream, cancellationToken);
    stream.Position = 0;

    try
    {
        if (fileExtension == ".xls")
        {
            // Используйте другую библиотеку для старого формата .xls
            using (var reader = ExcelReaderFactory.CreateBinaryReader(stream))
            {
                var result = reader.AsDataSet();
                var worksheet = result.Tables[0];

                for (int row = 1; row < worksheet.Rows.Count; row++) // Пропускаем заголовок
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    try
                    {
                        var entry = new BankStatementEntryResponse
                        {
                            Id = Guid.NewGuid(),
                            AccountNumber = worksheet.Rows[row][0]?.ToString()?.Trim() ?? "",
                            InitialBalanceActive = ParseDecimalValue(worksheet.Rows[row][1]?.ToString(), nameof(BankStatementEntryResponse.InitialBalanceActive)),
                            InitialBalancePassive = ParseDecimalValue(worksheet.Rows[row][2]?.ToString(), nameof(BankStatementEntryResponse.InitialBalancePassive)),
                            TurnoverDebit = ParseDecimalValue(worksheet.Rows[row][3]?.ToString(), nameof(BankStatementEntryResponse.TurnoverDebit)),
                            TurnoverCredit = ParseDecimalValue(worksheet.Rows[row][4]?.ToString(), nameof(BankStatementEntryResponse.TurnoverCredit)),
                            FinalBalanceActive = ParseDecimalValue(worksheet.Rows[row][5]?.ToString(), nameof(BankStatementEntryResponse.FinalBalanceActive)),
                            FinalBalancePassive = ParseDecimalValue(worksheet.Rows[row][6]?.ToString(), nameof(BankStatementEntryResponse.FinalBalancePassive))
                        };
                        entries.Add(entry);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error parsing row {Row}: {Error}", row, ex.Message);
                    }
                }
            }
        }
        else
        {
            // Существующий код для .xlsx файлов
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(stream);
            // ... остальной код остается без изменений
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error processing Excel file: {FileName}", file.FileName);
        throw new InvalidOperationException($"Failed to process Excel file: {ex.Message}", ex);
    }

    return entries;
}

    private void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            _logger.LogError("Attempted to parse null or empty file");
            throw new ArgumentException("File is null or empty");
        }

        var validExtensions = new[] { ".xlsx", ".xls" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (!validExtensions.Contains(fileExtension))
        {
            _logger.LogError("Invalid file extension: {Extension}", fileExtension);
            throw new InvalidOperationException($"Invalid file extension. Expected: {string.Join(", ", validExtensions)}, got: {fileExtension}");
        }

        // Проверка размера файла (например, 10 MB максимум)
        const int maxFileSize = 10 * 1024 * 1024; // 10 MB
        if (file.Length > maxFileSize)
        {
            _logger.LogError("File size exceeds limit. Size: {Size} bytes, Max allowed: {MaxSize} bytes", 
                file.Length, maxFileSize);
            throw new InvalidOperationException($"File size exceeds the maximum limit of {maxFileSize / 1024 / 1024} MB");
        }
    }

    private bool IsRowEmpty(ExcelWorksheet worksheet, int row)
    {
        return worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column]
            .All(cell => cell.Text.Trim().Length == 0);
    }

    private BankStatementEntryResponse ParseRow(ExcelWorksheet worksheet, int row)
    {
        try
        {
            return new BankStatementEntryResponse
            {
                Id = Guid.NewGuid(),
                AccountNumber = worksheet.Cells[row, 1].Text.Trim(),
                InitialBalanceActive = ParseDecimalValue(worksheet.Cells[row, 2].Text, nameof(BankStatementEntryResponse.InitialBalanceActive)),
                InitialBalancePassive = ParseDecimalValue(worksheet.Cells[row, 3].Text, nameof(BankStatementEntryResponse.InitialBalancePassive)),
                TurnoverDebit = ParseDecimalValue(worksheet.Cells[row, 4].Text, nameof(BankStatementEntryResponse.TurnoverDebit)),
                TurnoverCredit = ParseDecimalValue(worksheet.Cells[row, 5].Text, nameof(BankStatementEntryResponse.TurnoverCredit)),
                FinalBalanceActive = ParseDecimalValue(worksheet.Cells[row, 6].Text, nameof(BankStatementEntryResponse.FinalBalanceActive)),
                FinalBalancePassive = ParseDecimalValue(worksheet.Cells[row, 7].Text, nameof(BankStatementEntryResponse.FinalBalancePassive))
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to parse row {Row}. Skipping row. Error: {Error}", row, ex.Message);
            return null;
        }
    }

    public async Task<byte[]> GenerateExcelFileAsync(
        BankStatementDetailsResponse statement, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Excel file generation");
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        try
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Bank Statement");

                // Настройка стилей заголовков
                var headerRange = worksheet.Cells["A1:G1"];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                // Заголовки
                worksheet.Cells[1, 1].Value = "Account Number";
                worksheet.Cells[1, 2].Value = "Initial Balance Active";
                worksheet.Cells[1, 3].Value = "Initial Balance Passive";
                worksheet.Cells[1, 4].Value = "Turnover Debit";
                worksheet.Cells[1, 5].Value = "Turnover Credit";
                worksheet.Cells[1, 6].Value = "Final Balance Active";
                worksheet.Cells[1, 7].Value = "Final Balance Passive";

                int row = 2;
                foreach (var entry in statement.Entries)
                {
                    worksheet.Cells[row, 1].Value = entry.AccountNumber;
                    worksheet.Cells[row, 2].Value = entry.InitialBalanceActive;
                    worksheet.Cells[row, 3].Value = entry.InitialBalancePassive;
                    worksheet.Cells[row, 4].Value = entry.TurnoverDebit;
                    worksheet.Cells[row, 5].Value = entry.TurnoverCredit;
                    worksheet.Cells[row, 6].Value = entry.FinalBalanceActive;
                    worksheet.Cells[row, 7].Value = entry.FinalBalancePassive;

                    // Настройка формата для числовых ячеек
                    worksheet.Cells[row, 2, row, 7].Style.Numberformat.Format = "#,##0.00";
                    row++;
                }

                // Автоматическая настройка ширины колонок
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Добавление границ
                var dataRange = worksheet.Cells[1, 1, row - 1, 7];
                dataRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                _logger.LogInformation("Excel file generation completed successfully");
                return await Task.FromResult(package.GetAsByteArray());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Excel file");
            throw new InvalidOperationException("Failed to generate Excel file", ex);
        }
    }

    private decimal ParseDecimalValue(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _logger.LogWarning($"Empty value for field {fieldName}");
            return 0;
        }

        if (decimal.TryParse(value, out decimal result))
        {
            return result;
        }

        _logger.LogWarning($"Failed to parse value '{value}' for field {fieldName}");
        return 0;
    }
}