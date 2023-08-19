using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.OrderOnlineValuation;

internal sealed class OrderOnlineValuationRequestValidator
    : AbstractValidator<OrderOnlineValuationRequest>
{
    public OrderOnlineValuationRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
