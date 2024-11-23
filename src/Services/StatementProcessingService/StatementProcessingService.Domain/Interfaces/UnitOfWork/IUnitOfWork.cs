using StatementProcessingService.Domain.Interfaces.Repositories;

namespace StatementProcessingService.Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork:IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
    IBankStatementEntryRepository BankStatementEntries { get; }
    IBankStatementFileRepository BankStatementFiles { get; }
}