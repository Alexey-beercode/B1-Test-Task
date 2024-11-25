using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using StatementProcessingService.Application.Dtos.Request.File;
using StatementProcessingService.Application.Dtos.Response.File;
using StatementProcessingService.Application.Exceptions;
using StatementProcessingService.Application.Interfaces.Services;
using StatementProcessingService.Domain.Entities;
using StatementProcessingService.Domain.Interfaces.UnitOfWork;

namespace StatementProcessingService.Application.Services;

public class BankStatementFileService : IBankStatementFileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelParserService _excelParserService;
    private readonly IBankStatementEntryService _entryService;
    private readonly IMapper _mapper;

    public BankStatementFileService(
        IUnitOfWork unitOfWork,
        IExcelParserService excelParserService,
        IBankStatementEntryService entryService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _excelParserService = excelParserService;
        _entryService = entryService;
        _mapper = mapper;
    }

    public async Task<UploadBankStatementResponse> UploadFileAsync(
        UploadBankStatementRequest request, CancellationToken cancellationToken = default)
    {
        // Проверяем, существует ли файл с таким именем
        var existingFile = await _unitOfWork.BankStatementFiles.GetByFileNameAsync(request.FileName, cancellationToken);
        if (existingFile != null)
        {
            throw new FileUploadException("File is already exists");
        }

        // Парсим данные из Excel
        var parsedEntries = await _excelParserService.ParseExcelFileAsync(request.FileContent, cancellationToken);

        // Преобразуем DTO в сущности
        var entryEntities = _mapper.Map<IEnumerable<BankStatementEntry>>(parsedEntries);

        // Создаем запись о файле
        var file = new BankStatementFile
        {
            FileName = request.FileName,
            UploadDate = DateTime.UtcNow,
            Entries = entryEntities
        };

        // Сохраняем файл и записи в одной транзакции
        await _unitOfWork.BankStatementFiles.CreateAsync(file, cancellationToken);
        await _unitOfWork.BankStatementEntries.CreateBulkAsync(entryEntities, cancellationToken);

        // Сохраняем изменения в базе
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UploadBankStatementResponse
        {
            Id = file.Id,
            FileName = file.FileName,
            UploadDate = file.UploadDate
        };
    }

    public async Task<IEnumerable<BankStatementListItemResponse>> GetFilesListAsync(CancellationToken cancellationToken = default)
    {
        // Получаем список файлов из базы данных
        var files = await _unitOfWork.BankStatementFiles.GetFilesSummaryAsync(cancellationToken);

        // Преобразуем сущности в DTO
        return _mapper.Map<IEnumerable<BankStatementListItemResponse>>(files);
    }

    public async Task<BankStatementDetailsResponse> GetFileDetailsAsync(
        Guid fileId, CancellationToken cancellationToken = default)
    {
        // Получаем файл из базы данных, включая записи
        var file = await _unitOfWork.BankStatementFiles.GetByIdWithEntriesAsync(fileId, cancellationToken);
        if (file == null)
        {
            throw new EntityNotFoundException("File", fileId);
        }

        // Преобразуем данные файла в DTO
        var response = _mapper.Map<BankStatementDetailsResponse>(file);

        // Преобразуем записи файла в DTO
        var entries = await _entryService.GetEntriesByFileIdAsync(fileId, cancellationToken);
        response.Entries = entries.ToList();

        return response;
    }
}