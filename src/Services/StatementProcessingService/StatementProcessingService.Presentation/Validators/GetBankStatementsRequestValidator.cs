using FluentValidation;
using StatementProcessingService.Application.Dtos.Request.File;

namespace StatementProcessingService.Presentation.Validators;

public class GetBankStatementsRequestValidator : AbstractValidator<GetBankStatementsRequest>
{
    public GetBankStatementsRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("PageSize cannot be greater than 100.");

        RuleFor(x => x.BankStatementId)
            .NotEmpty()
            .WithMessage("BankStatementId cannot be empty.");
    }
}