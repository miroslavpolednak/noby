using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.UpdateOfferContacts;

internal sealed class UpdateOfferContactsRequestValidator 
    : AbstractValidator<UpdateOfferContactsRequest>
{
    public UpdateOfferContactsRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);
    }
}
