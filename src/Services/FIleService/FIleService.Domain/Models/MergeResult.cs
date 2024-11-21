namespace FIleService.Domain.Models;

public class MergeResult
{
    public int TotalLinesProcessed { get; set; }
    public int ExcludedLinesCount { get; set; }
    public string OutputFilePath { get; set; }
    public TimeSpan ProcessingTime { get; set; }
}