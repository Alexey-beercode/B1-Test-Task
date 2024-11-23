export interface ExportBankStatementRequest {
  fileId: string;
  exportFormat: 'Excel' | 'CSV';
}
