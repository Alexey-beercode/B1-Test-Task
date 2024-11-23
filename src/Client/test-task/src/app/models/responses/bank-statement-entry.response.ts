export interface BankStatementEntryResponse {
  id: string;
  accountNumber: string;
  initialBalanceActive: number;
  initialBalancePassive: number;
  turnoverDebit: number;
  turnoverCredit: number;
  finalBalanceActive: number;
  finalBalancePassive: number;
}
