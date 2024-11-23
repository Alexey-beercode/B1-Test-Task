using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;
using StatementProcessingService.Application.Dtos.Response.Entries;
using StatementProcessingService.Application.Dtos.Response.File;
using StatementProcessingService.Application.Interfaces.Services;

namespace StatementProcessingService.Application.Services;

public class ExcelParserService : IExcelParserService
{
    public async Task<IEnumerable<BankStatementEntryResponse>> ParseExcelFileAsync(
        Stream fileStream, CancellationToken cancellationToken = default)
    {
        // Убедимся, что EPPlus может работать
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var entries = new List<BankStatementEntryResponse>();

        using (var package = new ExcelPackage(fileStream))
        {
            // Получаем первый лист в Excel
            var worksheet = package.Workbook.Worksheets[0];

            // Предполагаем, что первая строка — это заголовок, начинаем с 2-й строки
            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                // Парсим данные строки в объект BankStatementEntryResponse
                var entry = new BankStatementEntryResponse
                {
                    AccountNumber = worksheet.Cells[row, 1].Text,
                    InitialBalanceActive = decimal.TryParse(worksheet.Cells[row, 2].Text, out var iba) ? iba : 0,
                    InitialBalancePassive = decimal.TryParse(worksheet.Cells[row, 3].Text, out var ibp) ? ibp : 0,
                    TurnoverDebit = decimal.TryParse(worksheet.Cells[row, 4].Text, out var td) ? td : 0,
                    TurnoverCredit = decimal.TryParse(worksheet.Cells[row, 5].Text, out var tc) ? tc : 0,
                    FinalBalanceActive = decimal.TryParse(worksheet.Cells[row, 6].Text, out var fba) ? fba : 0,
                    FinalBalancePassive = decimal.TryParse(worksheet.Cells[row, 7].Text, out var fbp) ? fbp : 0
                };

                entries.Add(entry);
            }
        }

        return await Task.FromResult(entries);
    }

    public async Task<byte[]> GenerateExcelFileAsync(
        BankStatementDetailsResponse statement, CancellationToken cancellationToken = default)
    {
        // Убедимся, что EPPlus может работать
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            // Создаем новый лист
            var worksheet = package.Workbook.Worksheets.Add("Bank Statement");

            // Добавляем заголовки
            worksheet.Cells[1, 1].Value = "AccountNumber";
            worksheet.Cells[1, 2].Value = "InitialBalanceActive";
            worksheet.Cells[1, 3].Value = "InitialBalancePassive";
            worksheet.Cells[1, 4].Value = "TurnoverDebit";
            worksheet.Cells[1, 5].Value = "TurnoverCredit";
            worksheet.Cells[1, 6].Value = "FinalBalanceActive";
            worksheet.Cells[1, 7].Value = "FinalBalancePassive";

            // Заполняем строки данными
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
                row++;
            }

            // Авторазмер колонок
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Возвращаем файл как массив байтов
            return await Task.FromResult(package.GetAsByteArray());
        }
    }
}