using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal sealed class SaveRequestValidator
    : AbstractValidator<Contracts.LoanApplication.V2.LoanApplicationSaveRequest>
{
    public SaveRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.LoanApplicationDataVersion)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.Households)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.Product)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError)
            .SetValidator(new SaveRequestProductValidator());

        RuleFor(t => t.Households)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError)
            .ForEach(t => t.SetValidator(new SaveRequestHouseholdValidator()));

        When(t => t.UserIdentity is not null, () =>
        {
            RuleFor(t => t.UserIdentity)
                .SetValidator(new IdentityValidator());
        });

        When(t => t.ProductRelations is not null, () =>
        {
            RuleForEach(t => t.ProductRelations)
                .ChildRules(x =>
                {
                    x.RuleFor(x => x.RelationType)
                        .NotEmpty()
                        .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

                    x.RuleFor(x => x.RemainingExposure)
                        .Cascade(CascadeMode.Stop)
                        .NotEmpty()
                        .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

                    x.When(x => x.Customers != null, () => {
                        x.RuleForEach(x => x.Customers)
                            .ChildRules(x2 =>
                            {
                                x2.RuleFor(x2 => x2.CustomerRoleId)
                                    .NotEmpty()
                                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

                                x2.RuleFor(x2 => x2.CustomerId)
                                    .NotEmpty()
                                    .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
                            });
                    });                    

                    x.When(x => x.BankAccount != null, () =>
                    {
                        x.RuleFor(x => x.BankAccount)
                        .ChildRules(x2 =>
                        {
                            x2.RuleFor(x2 => x2!.NumberPrefix)
                            .MaximumLength(6)
                            .WithErrorCode(ErrorCodeMapper.GeneralValidationError)
                            .Must(x2 => x2 == null || (int.TryParse(x2, out var val) && val >= 0))
                            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

                            x2.RuleFor(x2 => x2!.Number)
                            .MaximumLength(10)
                            .WithErrorCode(ErrorCodeMapper.GeneralValidationError)
                            .Must(x2 => x2 == null || (long.TryParse(x2, out var val) && val > 0))
                            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
                        });
                    });
                });
        });
    }
}