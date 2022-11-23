using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.DocumentArchiveService.Contracts;
using FluentValidation;
using System.Globalization;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.UploadDocument
{
    public sealed class UploadDocumentMediatrRequestValidator : AbstractValidator<UploadDocumentMediatrRequest>
    {
        public UploadDocumentMediatrRequestValidator()
        {
            RuleFor(t => t.Request).NotEmpty();

            When(t => t.Request is not null, () =>
            {
                RuleFor(t => t.Request.BinaryData)
                    .NotEmpty()
                    .WithMessage("BinaryData cannot be emty");
            });

            When(t => t.Request is not null, () =>
            {
                RuleFor(t => t.Request.Metadata)
                    .NotNull()
                    .WithMessage("Metadata cannot be empty");
            });

            RuleFor(m => m.Request.Metadata)
                .NotNull()
                .SetValidator(new DocumentMetadataValidator());
        }
    }

    public sealed class DocumentMetadataValidator : AbstractValidator<DocumentMetadata>
    {
        private readonly Func<GrpcDate, bool> ValidateNotNullDateOnly = (grpcDate) =>
        {
            if (grpcDate is null)
            {
                return false;
            }

            var dateInvariant = $"{grpcDate.Month}/{grpcDate.Day}/{grpcDate.Year}";
            return DateOnly.TryParse(dateInvariant, CultureInfo.InvariantCulture, out var dateOnly);
        };

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
             .Must(ValidateNotNullDateOnly)
             .WithMessage($"Metadata.{nameof(DocumentMetadata.CreatedOn)} is null or invalid date format");

            RuleFor(e => e.Filename)
                .NotEmpty()
                .WithMessage($"Metadata.{nameof(DocumentMetadata.AuthorUserLogin)} cannot be null or empty string");

            RuleFor(e => e.FolderDocument)
                .IsInEnum()
                .WithMessage($"Unknown Metadata.{nameof(DocumentMetadata.FolderDocument)}");

            RuleFor(e => e.DocumentDirection)
                .IsInEnum()
                .WithMessage($"Unknown Metadata.{nameof(DocumentMetadata.DocumentDirection)}");
        }
    }
}
