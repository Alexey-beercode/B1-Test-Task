using System;

namespace StatementProcessingService.Application.Dtos.Response.Entries;

public class BankStatementEntryResponse
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; }
    public decimal InitialBalanceActive { get; set; }
    public decimal InitialBalancePassive { get; set; }
    public decimal TurnoverDebit { get; set; }
    public decimal TurnoverCredit { get; set; }
    public decimal FinalBalanceActive { get; set; }
    public decimal FinalBalancePassive { get; set; }
}