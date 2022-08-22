using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal sealed class SaveRequestValidator
    : AbstractValidator<Contracts.LoanApplication.V2.LoanApplicationSaveRequest>
{
    public SaveRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0);

        RuleFor(t => t.LoanApplicationDataVersion)
            .NotEmpty();

        RuleFor(t => t.Households)
            .NotEmpty();

        RuleFor(t => t.Product)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .SetValidator(new SaveRequestProductValidator());

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
                    x.RuleFor(x => x.ProductType)
                        .NotEmpty();

                    x.RuleFor(x => x.RelationType)
                        .NotEmpty();

                    x.RuleFor(x => x.RemainingExposure)
                        .Cascade(CascadeMode.Stop)
                        .NotEmpty()
                        .GreaterThan(0);

                    x.RuleFor(x => x.Customers)
                        .NotEmpty();

                    x.RuleForEach(x => x.Customers)
                        .ChildRules(x2 =>
                        {
                            x2.RuleFor(x2 => x2.CustomerRoleId)
                                .NotEmpty();

                            x2.RuleFor(x2 => x2.CustomerId)
                                .NotEmpty();
                        });
                });
        });
    }
}


internal sealed class SaveRequestProductValidator
    : AbstractValidator<Contracts.LoanApplication.V2.LoanApplicationProduct>
{
    public SaveRequestProductValidator()
    {
        RuleFor(t => t.ProductTypeId)
            .GreaterThan(0);

        RuleFor(t => t.LoanKindId)
            .GreaterThan(0);
    }
}