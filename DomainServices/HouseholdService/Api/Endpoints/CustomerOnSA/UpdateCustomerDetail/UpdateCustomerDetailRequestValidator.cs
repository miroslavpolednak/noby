using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateCustomerDetail;

internal sealed class UpdateCustomerDetailRequestValidator
    : AbstractValidator<UpdateCustomerDetailRequest>
{
    public UpdateCustomerDetailRequestValidator()
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithErrorCode(ValidationMessages.CustomerOnSAIdIsEmpty);
    }
}