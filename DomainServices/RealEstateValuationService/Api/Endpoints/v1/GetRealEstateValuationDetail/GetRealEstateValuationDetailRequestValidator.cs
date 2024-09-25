using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.GetRealEstateValuationDetail;

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
