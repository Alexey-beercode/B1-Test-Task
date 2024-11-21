namespace FIleService.Application.Interfaces;

public interface IFileImportService
{
    Task ImportFilesAsync(CancellationToken cancellationToken = default);
}