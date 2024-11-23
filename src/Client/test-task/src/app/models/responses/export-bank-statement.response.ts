export interface ExportBankStatementResponse {
  fileName: string;
  fileContent: Uint8Array;
  contentType: string;
}
