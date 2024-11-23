using FluentValidation;
using StatementProcessingService.Application.Dtos.Request.File;

namespace StatementProcessingService.Presentation.Validators;

public class UploadBankStatementRequestValidator : AbstractValidator<UploadBankStatementRequest>
{
    public UploadBankStatementRequestValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("FileName is required.")
            .MaximumLength(255)
            .WithMessage("FileName cannot exceed 255 characters.")
            .Matches(@"^[\w,\s-]+\.[A-Za-z]{3,4}$")
            .WithMessage("FileName must be a valid file name with an extension (e.g., .xls, .csv).");

        RuleFor(x => x.FileContent)
            .NotNull()
            .WithMessage("FileContent cannot be null.")
            .Must(content => content.Length > 0)
            .WithMessage("FileContent cannot be empty.");
    }
}