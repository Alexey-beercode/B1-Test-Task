namespace FIleService.Domain.Models;

public class Statistics
{
    public Statistics(long sumOfIntegers, decimal medianOfDecimals)
    {
        SumOfIntegers = sumOfIntegers;
        MedianOfDecimals = medianOfDecimals;
    }

    public long SumOfIntegers { get; set; }
    public decimal MedianOfDecimals { get; set; }
}