using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.DeleteRealEstateValuation;

internal sealed class DeleteRealEstateValuationHandlerRequestValidator
    : AbstractValidator<DeleteRealEstateValuationRequest>
{
    public DeleteRealEstateValuationHandlerRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
