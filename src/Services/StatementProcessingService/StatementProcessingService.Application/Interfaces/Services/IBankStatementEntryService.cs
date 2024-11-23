using StatementProcessingService.Application.Dtos.Request.File;
using StatementProcessingService.Application.Dtos.Response.Entries;

namespace StatementProcessingService.Application.Interfaces.Services;

public interface IBankStatementEntryService
{
    Task<IEnumerable<BankStatementEntryResponse>> GetEntriesByFileIdAsync(
        Guid fileId, 
        CancellationToken cancellationToken = default);
            
    Task<IEnumerable<BankStatementEntryResponse>> GetPagedEntriesAsync(
        GetBankStatementsRequest bankStatementsRequest,
        CancellationToken cancellationToken = default);
            
    Task CreateBulkAsync(
        Guid fileId, 
        IEnumerable<BankStatementEntryResponse> entries,
        CancellationToken cancellationToken = default);
}