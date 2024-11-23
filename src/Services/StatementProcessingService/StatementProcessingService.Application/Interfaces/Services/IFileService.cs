using StatementProcessingService.Application.Dtos.Request.File;
using StatementProcessingService.Application.Dtos.Response.Entries;
using StatementProcessingService.Application.Dtos.Response.File;

namespace StatementProcessingService.Application.Interfaces.Services;

public interface IFileService
{
    Task<ExportBankStatementResponse> ExportFileAsync(
        ExportBankStatementRequest request, CancellationToken cancellationToken = default);
}