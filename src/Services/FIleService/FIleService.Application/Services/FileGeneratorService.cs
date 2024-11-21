using FIleService.Application.Interfaces;
using FIleService.Domain.Entities;

namespace FIleService.Application.Services;

public class FileGeneratorService : IFileGeneratorService
{
    private readonly Random _random = new();
    private const string LatinChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string CyrillicChars = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

    public async Task GenerateFilesAsync(int fileCount, int rowsPerFile, string outputDirectory, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(outputDirectory);

        var tasks = new List<Task>();
        for (int i = 1; i <= fileCount; i++)
        {
            var fileName = Path.Combine(outputDirectory, $"data_{i}.txt");
            tasks.Add(GenerateFileAsync(fileName, rowsPerFile, cancellationToken));
        }

        await Task.WhenAll(tasks);
    }

    private async Task GenerateFileAsync(string filePath, int rowCount, CancellationToken cancellationToken)
    {
        await using var writer = new StreamWriter(filePath, false);
        for (int i = 0; i < rowCount; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var dataRow = GenerateDataRow();
            await writer.WriteLineAsync(dataRow.ToString());
        }
    }

    private DataRow GenerateDataRow()
    {
        var date = DateTime.Now.AddDays(-_random.Next(0, 365 * 5));
        var latinChars = GenerateRandomString(LatinChars, 10);
        var cyrillicChars = GenerateRandomString(CyrillicChars, 10);
        var evenNumber = GenerateEvenNumber();
        var floatingNumber = GenerateFloatingNumber();

        return DataRow.Create(date, latinChars, cyrillicChars, evenNumber, floatingNumber);
    }

    private string GenerateRandomString(string allowedChars, int length)
    {
        return new string(Enumerable.Range(0, length)
            .Select(_ => allowedChars[_random.Next(allowedChars.Length)])
            .ToArray());
    }

    private long GenerateEvenNumber()
    {
        long number = _random.NextInt64(1, 100_000_000);
        return number + (number % 2);
    }

    private decimal GenerateFloatingNumber()
    {
        return (decimal)(_random.NextDouble() * 19 + 1);
    }
}