using Microsoft.EntityFrameworkCore;
using StatementProcessingService.Domain.Entities;
using StatementProcessingService.Domain.Interfaces.Repositories;
using StatementProcessingService.Infrastructure.Data;

namespace StatementProcessingService.Infrastructure.Repositories;

public class BankStatementEntryRepository : BaseRepository<BankStatementEntry>, IBankStatementEntryRepository
{
    public BankStatementEntryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<BankStatementEntry>> GetEntriesByFileIdAsync(
        Guid fileId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(e => e.BankStatementId == fileId)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateBulkAsync(
        IEnumerable<BankStatementEntry> entries, 
        CancellationToken cancellationToken = default)
    {
        if (entries == null || !entries.Any())
            return;
        
        const int batchSize = 1000;
        var batches = entries
            .Select((entry, index) => new { entry, index })
            .GroupBy(x => x.index / batchSize)
            .Select(g => g.Select(x => x.entry));

        foreach (var batch in batches)
        {
            await _dbSet.AddRangeAsync(batch, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<BankStatementEntry>> GetPagedEntriesAsync(
        Guid fileId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(e => e.BankStatementId == fileId)
            .OrderBy(e => e.AccountNumber)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<BankStatementEntry>> GetEntriesWithFileAsync(
        Guid fileId, 
        CancellationToken cancellationToken = default)
    {
        
        return await _dbSet
            .AsNoTracking()
            .Include(e => e.BankStatementFile)
            .Where(e => e.BankStatementId == fileId)
            .ToListAsync(cancellationToken);
    }
}