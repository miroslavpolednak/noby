using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.CreateDocumentOnSA;

public class CreateDocumentOnSAValidator: AbstractValidator<CreateDocumentOnSARequest>
{
    public CreateDocumentOnSAValidator()
    {
        RuleFor(e => e.SalesArrangementId).NotNull().WithMessage($"{nameof(CreateDocumentOnSARequest.SalesArrangementId)} is required");
        RuleFor(e => e.DocumentTypeId).NotNull().WithMessage($"{nameof(CreateDocumentOnSARequest.DocumentTypeId)} is required");
        RuleFor(e => e.FormId).NotEmpty().WithMessage($"{nameof(CreateDocumentOnSARequest.FormId)} is required");
        RuleFor(e => e.EArchivId).NotEmpty().WithMessage($"{nameof(CreateDocumentOnSARequest.EArchivId)} is required");
    }
}
