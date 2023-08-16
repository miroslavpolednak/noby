using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal sealed class SaveRequestHouseholdValidator
    : AbstractValidator<Contracts.LoanApplication.V2.LoanApplicationHousehold>
{
    public SaveRequestHouseholdValidator()
    {
        RuleFor(t => t.HouseholdId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleForEach(t => t.Customers)
            .ChildRules(c =>
            {
                c.RuleFor(t  => t.InternalCustomerId)
                    .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

                c.RuleFor(t => t.PrimaryCustomerId)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

                c.RuleFor(t => t.CustomerRoleId)
                    .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

                When(c => c.Customers is not null, () =>
                {
                    RuleForEach(x => x.Customers)
                        .SetValidator(new SaveRequestHouseholdCustomerValidator());
                });
            });
    }
}