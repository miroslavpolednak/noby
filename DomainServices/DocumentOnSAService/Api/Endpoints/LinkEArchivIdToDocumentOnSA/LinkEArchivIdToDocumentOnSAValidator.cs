using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.LinkEArchivIdToDocumentOnSA;

public class LinkEArchivIdToDocumentOnSAValidator : AbstractValidator<LinkEArchivIdToDocumentOnSARequest>
{
    public LinkEArchivIdToDocumentOnSAValidator()
    {
        RuleFor(e => e.EArchivId).NotEmpty().WithErrorCode(ErrorCodeMapper.EArchivIdIsRequired);
    }
}
