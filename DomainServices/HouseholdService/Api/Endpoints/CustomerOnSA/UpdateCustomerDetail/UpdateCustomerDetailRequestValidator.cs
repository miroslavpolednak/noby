using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateCustomerDetail;

internal class UpdateCustomerDetailRequestValidator
    : AbstractValidator<UpdateCustomerDetailRequest>
{
    public UpdateCustomerDetailRequestValidator()
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithMessage("CustomerOnSAId must be > 0").WithErrorCode("16024");
    }
}