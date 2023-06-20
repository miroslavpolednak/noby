using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.CreateRealEstateValuation;

internal sealed class CreateRealEstateValuationRequestValidator
    : AbstractValidator<CreateRealEstateValuationRequest>
{
    public CreateRealEstateValuationRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdEmpty);
    }
}
