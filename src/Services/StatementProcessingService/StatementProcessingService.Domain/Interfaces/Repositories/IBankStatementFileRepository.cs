using StatementProcessingService.Domain.Entities;

namespace StatementProcessingService.Domain.Interfaces.Repositories;

public interface IBankStatementFileRepository : IBaseRepository<BankStatementFile>
{
    Task<BankStatementFile> GetByIdWithEntriesAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<BankStatementFile> GetByFileNameAsync(string fileName, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<BankStatementFile>> GetFilesSummaryAsync(CancellationToken cancellationToken = default);
}