using StatementProcessingService.Domain.Interfaces.Repositories;
using StatementProcessingService.Domain.Interfaces.UnitOfWork;
using StatementProcessingService.Infrastructure.Data;

namespace StatementProcessingService.Infrastructure.UnitOfWork;

public class UnitOfWork:IUnitOfWork
{
    private readonly IBankStatementEntryRepository _bankStatementEntryRepository;
    private readonly IBankStatementFileRepository _bankStatementFileRepository;
    private readonly ApplicationDbContext _dbContext;
    private bool _disposed;

    public UnitOfWork(IBankStatementFileRepository bankStatementFileRepository,IBankStatementEntryRepository bankStatementEntryRepository, ApplicationDbContext dbContext)
    {
        _bankStatementFileRepository = bankStatementFileRepository;
        _bankStatementEntryRepository = bankStatementEntryRepository;
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }
    }

    public IBankStatementEntryRepository BankStatementEntries => _bankStatementEntryRepository;
    public IBankStatementFileRepository BankStatementFiles => _bankStatementFileRepository;
}