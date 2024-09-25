using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.SetForeignRealEstateTypesByRealEstateValuation;

internal sealed class SetForeignRealEstateTypesByRealEstateValuationRequestValidator
    : AbstractValidator<SetForeignRealEstateTypesByRealEstateValuationRequest>
{
    public SetForeignRealEstateTypesByRealEstateValuationRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);

        RuleFor(t => t.ACVRealEstateTypeId)
            .NotEmpty()
            .Length(2)
            .WithErrorCode(ErrorCodeMapper.ACVRealEstateTypeIsEmpty);

        RuleFor(t => t.BagmanRealEstateTypeId)
            .NotEmpty()
            .Length(2)
            .WithErrorCode(ErrorCodeMapper.ACVRealEstateTypeIsEmpty);
    }
}
