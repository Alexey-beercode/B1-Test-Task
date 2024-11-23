export interface MergeResult {
  totalLinesProcessed: number;
  excludedLinesCount: number;
  outputFilePath: string;
  processingTime: string; // TimeSpan придет как строка
}
