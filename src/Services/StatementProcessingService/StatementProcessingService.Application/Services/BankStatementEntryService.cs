using AutoMapper;
using StatementProcessingService.Application.Dtos.Request.File;
using StatementProcessingService.Application.Dtos.Response.Entries;
using StatementProcessingService.Application.Interfaces.Services;
using StatementProcessingService.Domain.Entities;
using StatementProcessingService.Domain.Interfaces.UnitOfWork;

namespace StatementProcessingService.Application.Services;

public class BankStatementEntryService : IBankStatementEntryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BankStatementEntryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Получает все записи, связанные с указанным файлом.
    /// </summary>
    public async Task<IEnumerable<BankStatementEntryResponse>> GetEntriesByFileIdAsync(
        Guid bankStatementId, CancellationToken cancellationToken = default)
    {
        var entries = await _unitOfWork.BankStatementEntries.GetEntriesByFileIdAsync(bankStatementId, cancellationToken);
        return _mapper.Map<IEnumerable<BankStatementEntryResponse>>(entries);
    }

    /// <summary>
    /// Получает записи по ID файла постранично.
    /// </summary>
    public async Task<IEnumerable<BankStatementEntryResponse>> GetPagedEntriesAsync(
        GetBankStatementsRequest bankStatementsRequest, CancellationToken cancellationToken = default)
    {
        var entries = await _unitOfWork.BankStatementEntries
            .GetPagedEntriesAsync(bankStatementsRequest.BankStatementId, bankStatementsRequest.PageNumber, bankStatementsRequest.PageSize, cancellationToken);
        return _mapper.Map<IEnumerable<BankStatementEntryResponse>>(entries);
    }

    /// <summary>
    /// Создает записи в базе данных массово.
    /// </summary>
    public async Task CreateBulkAsync(
        Guid bankStatementId, IEnumerable<BankStatementEntryResponse> entries, CancellationToken cancellationToken = default)
    {
        // Проверяем входные данные
        if (entries == null || !entries.Any())
        {
            throw new ArgumentException("Entries list cannot be empty.");
        }
        
        var entryEntities = _mapper.Map<IEnumerable<BankStatementEntry>>(entries);
        
        foreach (var entry in entryEntities)
        {
            entry.BankStatementId = bankStatementId;
        }
        
        await _unitOfWork.BankStatementEntries.CreateBulkAsync(entryEntities, cancellationToken);
    }
}