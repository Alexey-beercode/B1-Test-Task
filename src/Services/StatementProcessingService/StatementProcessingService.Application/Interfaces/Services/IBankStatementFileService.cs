using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StatementProcessingService.Application.Dtos.Request.File;
using StatementProcessingService.Application.Dtos.Response.File;

namespace StatementProcessingService.Application.Interfaces.Services;

public interface IBankStatementFileService
{
    Task<UploadBankStatementResponse> UploadFileAsync(
        UploadBankStatementRequest request,
        CancellationToken cancellationToken = default);

    Task<BankStatementDetailsResponse> GetFileDetailsAsync(
        Guid fileId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<BankStatementListItemResponse>> GetFilesListAsync(
        GetBankStatementsRequest request,
        CancellationToken cancellationToken = default);
}