import { BankStatementEntryResponse } from './bank-statement-entry.response';

export interface BankStatementDetailsResponse {
  id: string;
  fileName: string;
  uploadDate: Date;
  entries: BankStatementEntryResponse[];
}
