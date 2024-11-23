﻿using StatementProcessingService.Application.Dtos.Response.Entries;
using StatementProcessingService.Application.Dtos.Response.File;

namespace StatementProcessingService.Application.Interfaces.Services;

public interface IExcelParserService
{
    Task<IEnumerable<BankStatementEntryResponse>> ParseExcelFileAsync(
        Stream fileStream,
        CancellationToken cancellationToken = default);
            
    Task<byte[]> GenerateExcelFileAsync(
        BankStatementDetailsResponse statement,
        CancellationToken cancellationToken = default);
}