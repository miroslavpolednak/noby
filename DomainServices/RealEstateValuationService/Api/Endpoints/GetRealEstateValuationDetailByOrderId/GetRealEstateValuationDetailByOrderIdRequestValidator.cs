using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetRealEstateValuationDetailByOrderId;

internal sealed class GetRealEstateValuationDetailByOrderIdRequestValidator
    : AbstractValidator<GetRealEstateValuationDetailByOrderIdRequest>
{
    public GetRealEstateValuationDetailByOrderIdRequestValidator()
    {
        RuleFor(t => t.OrderId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.OrderIdEmpty);
    }
}
