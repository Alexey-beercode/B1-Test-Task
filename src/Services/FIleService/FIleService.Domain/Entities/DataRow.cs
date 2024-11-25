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

        return new DataRow(date, latinChars, cyrillicChars, evenNumber, floatingNumber);
    }

    public override string ToString()
    {
        return $"{Date:dd.MM.yyyy}||{LatinChars}||{CyrillicChars}||{EvenNumber}||{FloatingNumber:F8}||";
    }
}