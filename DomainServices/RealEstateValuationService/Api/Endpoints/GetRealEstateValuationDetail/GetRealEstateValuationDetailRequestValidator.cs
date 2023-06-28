using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetRealEstateValuationDetail;

internal sealed class GetRealEstateValuationDetailRequestValidator
    : AbstractValidator<GetRealEstateValuationDetailRequest>
{
    public GetRealEstateValuationDetailRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
