namespace FIleService.Application.Interfaces;

public interface IFileGeneratorService
{
    Task GenerateFilesAsync(int fileCount, int rowsPerFile, string outputDirectory, CancellationToken cancellationToken = default);
}