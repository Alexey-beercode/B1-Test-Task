using StatementProcessingService.Domain.Entities;

namespace StatementProcessingService.Domain.Interfaces.Repositories;

public interface IBankStatementEntryRepository : IBaseRepository<BankStatementEntry>
{
    Task<IEnumerable<BankStatementEntry>> GetEntriesByFileIdAsync(Guid fileId, 
        CancellationToken cancellationToken = default);
    
    Task CreateBulkAsync(IEnumerable<BankStatementEntry> entries, 
        CancellationToken cancellationToken = default);

    Task<IEnumerable<BankStatementEntry>> GetPagedEntriesAsync(
        Guid fileId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<BankStatementEntry>> GetEntriesWithFileAsync(
        Guid fileId,
        CancellationToken cancellationToken = default);
}