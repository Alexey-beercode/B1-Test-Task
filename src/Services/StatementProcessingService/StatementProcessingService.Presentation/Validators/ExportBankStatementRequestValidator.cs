using FluentValidation;
using StatementProcessingService.Application.Dtos.Request.File;

namespace StatementProcessingService.Presentation.Validators;

public class ExportBankStatementRequestValidator : AbstractValidator<ExportBankStatementRequest>
{
    public ExportBankStatementRequestValidator()
    {
        RuleFor(x => x.FileId)
            .NotEmpty()
            .WithMessage("FileId cannot be empty.");

        RuleFor(x => x.ExportFormat)
            .NotEmpty()
            .WithMessage("ExportFormat is required.")
            .Must(format => format.Equals("Excel", StringComparison.OrdinalIgnoreCase) ||
                            format.Equals("CSV", StringComparison.OrdinalIgnoreCase) ||
                            format.Equals("PDF", StringComparison.OrdinalIgnoreCase))
            .WithMessage("ExportFormat must be one of the following: Excel, CSV, PDF.");
    }
}