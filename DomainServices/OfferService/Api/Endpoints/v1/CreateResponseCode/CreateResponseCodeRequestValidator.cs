using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.v1.CreateResponseCode;

internal sealed class CreateResponseCodeRequestValidator
    : AbstractValidator<CreateResponseCodeRequest>
{
    public CreateResponseCodeRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);
    }
}
