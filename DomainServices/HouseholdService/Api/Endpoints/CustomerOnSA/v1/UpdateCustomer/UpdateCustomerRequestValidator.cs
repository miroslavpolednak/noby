using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.UpdateCustomer;

internal sealed class UpdateCustomerRequestValidator
    : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CustomerOnSAIdIsEmpty);

        RuleFor(t => t.Customer)
            .SetInheritanceValidator(v =>
            {
                v.Add(new Validators.CustomerOnSAValidator());
            });
    }
}