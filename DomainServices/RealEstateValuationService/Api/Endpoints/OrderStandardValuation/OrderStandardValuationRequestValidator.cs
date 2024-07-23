using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.OrderStandardValuation;

internal sealed class OrderStandardValuationRequestValidator
    : AbstractValidator<OrderStandardValuationRequest>
{
    public OrderStandardValuationRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);

        RuleFor(t => t.LocalSurveyDetails)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.LocalSurveyDetailsIsEmpty);
    }
}
