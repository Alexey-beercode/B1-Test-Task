using FIleService.Domain.Common;

namespace FIleService.Domain.Entities;

public class DataRow : BaseEntity
{
    public DateTime Date { get; private set; }
    public string LatinChars { get; private set; }
    public string CyrillicChars { get; private set; }
    public long EvenNumber { get; private set; }
    public decimal FloatingNumber { get; private set; }

    private DataRow(DateTime date, string latinChars, string cyrillicChars, long evenNumber, decimal floatingNumber)
    {
        Date = date;
        LatinChars = latinChars;
        CyrillicChars = cyrillicChars;
        EvenNumber = evenNumber;
        FloatingNumber = floatingNumber;
    }

    public static DataRow Create(DateTime date, string latinChars, string cyrillicChars, long evenNumber,
        decimal floatingNumber)
    {
        if (string.IsNullOrEmpty(latinChars) || latinChars.Length != 10)
            throw new ArgumentException("Latin chars must be exactly 10 characters", nameof(latinChars));

        if (string.IsNullOrEmpty(cyrillicChars) || cyrillicChars.Length != 10)
            throw new ArgumentException("Cyrillic chars must be exactly 10 characters", nameof(cyrillicChars));

        if (evenNumber <= 0 || evenNumber > 100_000_000 || evenNumber % 2 != 0)
            throw new ArgumentException("Number must be positive, even and less than 100,000,000", nameof(evenNumber));

        if (floatingNumber < 1 || floatingNumber > 20)
            throw new ArgumentException("Floating number must be between 1 and 20", nameof(floatingNumber));

        return new DataRow(date, latinChars, cyrillicChars, evenNumber, floatingNumber);
    }

    public override string ToString()
    {
        return $"{Date:dd.MM.yyyy}||{LatinChars}||{CyrillicChars}||{EvenNumber}||{FloatingNumber:F8}||";
    }
}