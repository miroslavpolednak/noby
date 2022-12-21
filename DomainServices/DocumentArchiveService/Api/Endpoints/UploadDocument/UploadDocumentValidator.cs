using DomainServices.DocumentArchiveService.Api.Endpoints.Common.Validators;
using DomainServices.DocumentArchiveService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.UploadDocument;

public sealed class UploadDocumentValidator : AbstractValidator<UploadDocumentRequest>
{
    public UploadDocumentValidator()
    {
        RuleFor(t => t).NotEmpty();

        When(t => t is not null, () =>
                {
                    RuleFor(t => t.BinaryData)
                        .NotEmpty()
                        .WithMessage("BinaryData cannot be emty");
                });

        When(t => t is not null, () =>
        {
            RuleFor(t => t.Kdv)
                .InclusiveBetween(0, 1)
                .WithMessage("Kdv have to be in range 0-1");
        });

        When(t => t is not null, () =>
        {
            RuleFor(t => t.Metadata)
                .NotNull()
                .WithMessage("Metadata cannot be empty");
        });

        RuleFor(m => m.Metadata)
            .NotNull()
            .SetValidator(new DocumentMetadataValidator());
    }
}

public sealed class DocumentMetadataValidator : AbstractValidator<DocumentMetadata>
{
    public DocumentMetadataValidator()
    {
        RuleFor(e => e.CaseId)
            .NotNull()
            .WithMessage($"Metadata.{nameof(DocumentMetadata.CaseId)} cannot be null");

        RuleFor(e => e.DocumentId)
            .NotEmpty()
            .WithMessage($"Metadata.{nameof(DocumentMetadata.DocumentId)} cannot be null or empty string");

        RuleFor(e => e.EaCodeMainId)
            .NotNull()
            .WithMessage($"Metadata.{nameof(DocumentMetadata.EaCodeMainId)} cannot be null");

        RuleFor(e => e.Filename)
           .NotEmpty()
           .WithMessage($"Metadata.{nameof(DocumentMetadata.DocumentId)} cannot be null or empty string");

        RuleFor(e => e.CreatedOn)
         .Must(CommonValidators.ValidateDateOnly)
         .WithMessage($"Metadata.{nameof(DocumentMetadata.CreatedOn)} is null or invalid date format");

        RuleFor(e => e.AuthorUserLogin)
            .NotEmpty()
            .WithMessage($"Metadata.{nameof(DocumentMetadata.AuthorUserLogin)} cannot be null or empty string");

    }
}
