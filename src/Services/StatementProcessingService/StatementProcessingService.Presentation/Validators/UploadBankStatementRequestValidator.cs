using FluentValidation;
using StatementProcessingService.Application.Dtos.Request.File;

namespace StatementProcessingService.Presentation.Validators;

public class UploadBankStatementRequestValidator : AbstractValidator<UploadBankStatementRequest>
{
    private const int MaxFileSize = 10 * 1024 * 1024; // 10MB
    private readonly string[] allowedExtensions = { ".xls", ".xlsx", ".csv" };

    public UploadBankStatementRequestValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("FileName is required.")
            .MaximumLength(255)
            .WithMessage("FileName cannot exceed 255 characters.")
            .WithMessage("FileName must be a valid file name with an extension (e.g., .xls, .csv)")
            .Must(fileName => fileName != null && 
                              allowedExtensions.Contains(Path.GetExtension(fileName).ToLower()))
            .WithMessage($"File must have one of the following extensions: {string.Join(", ", allowedExtensions)}");

        RuleFor(x => x.FileContent)
            .NotNull()
            .WithMessage("FileContent cannot be null")
            .Must(content => content != null)
            .WithMessage("FileContent cannot be null")
            .Must(content => content != null && content.Length > 0)
            .WithMessage("FileContent cannot be empty")
            .Must(content => content != null && content.Length <= MaxFileSize)
            .WithMessage($"File size must not exceed {MaxFileSize / 1024 / 1024}MB");
    }
}