using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.UpdateRealEstateValuationDetail;

internal sealed class UpdateRealEstateValuationDetailRequestValidator
    : AbstractValidator<UpdateRealEstateValuationDetailRequest>
{
    public UpdateRealEstateValuationDetailRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
