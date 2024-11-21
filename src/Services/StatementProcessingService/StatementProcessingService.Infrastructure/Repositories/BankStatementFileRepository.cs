using Microsoft.EntityFrameworkCore;
using StatementProcessingService.Domain.Entities;
using StatementProcessingService.Domain.Interfaces.Repositories;
using StatementProcessingService.Infrastructure.Data;

namespace StatementProcessingService.Infrastructure.Repositories;

public class BankStatementFileRepository : BaseRepository<BankStatementFile>, IBankStatementFileRepository
{
    public BankStatementFileRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<BankStatementFile> GetByIdWithEntriesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(f => f.Entries)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<BankStatementFile> GetByFileNameAsync(string fileName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.FileName == fileName, cancellationToken);
    }

    public async Task<IEnumerable<BankStatementFile>> GetFilesSummaryAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(f => f.Entries)
            .ToListAsync(cancellationToken);
    }
}