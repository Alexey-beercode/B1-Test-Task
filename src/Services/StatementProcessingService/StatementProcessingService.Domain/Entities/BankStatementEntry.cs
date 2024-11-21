using StatementProcessingService.Domain.Common;

namespace StatementProcessingService.Domain.Entities;

public class BankStatementEntry : BaseEntity
{
    public Guid BankStatementId { get; set; }
    public string AccountNumber { get; set; }
    public decimal InitialBalanceActive { get; set; }
    public decimal InitialBalancePassive { get; set; }
    public decimal TurnoverDebit { get; set; }
    public decimal TurnoverCredit { get; set; }
    public decimal FinalBalanceActive { get; set; }
    public decimal FinalBalancePassive { get; set; }
    public BankStatementFile BankStatementFile { get; set; }
}