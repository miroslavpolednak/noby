using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.CreateDocumentOnSA;

public class CreateDocumentOnSAValidator: AbstractValidator<CreateDocumentOnSARequest>
{
    public CreateDocumentOnSAValidator()
    {
        RuleFor(e => e.SalesArrangementId).NotNull().WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsRequired);
        RuleFor(e => e.DocumentTypeId).NotNull().WithErrorCode(ErrorCodeMapper.DocumentTypeIdIsRequired);
        RuleFor(e => e.FormId).NotEmpty().WithErrorCode(ErrorCodeMapper.FormIdIsRequired);
        RuleFor(e => e.EArchivId).NotEmpty().WithErrorCode(ErrorCodeMapper.EArchivIdIsRequired);
    }
}
