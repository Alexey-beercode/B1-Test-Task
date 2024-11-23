namespace StatementProcessingService.Application.Dtos.Request.File;

public class GetBankStatementsRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid BankStatementId { get; set; }
}