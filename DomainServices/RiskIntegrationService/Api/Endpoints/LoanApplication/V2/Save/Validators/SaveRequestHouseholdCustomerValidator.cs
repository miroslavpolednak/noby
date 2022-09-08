using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal sealed class SaveRequestHouseholdCustomerValidator
    : AbstractValidator<Contracts.LoanApplication.V2.LoanApplicationCustomer>
{
    public SaveRequestHouseholdCustomerValidator()
    {
        When(t => t.IdentificationDocument is not null, () =>
        {
            RuleFor(t => t.IdentificationDocument)
                .ChildRules(t =>
                {
                    t.RuleFor(x => x!.IdentificationDocumentTypeId)
                        .GreaterThan(0)
                        .WithErrorCode("Households.Customers.IdentificationDocument.IdentificationDocumentTypeId");
                });
        });

        When(t => t.Obligations is not null, () =>
        {
            RuleForEach(t => t.Obligations)
                .ChildRules(t =>
                {
                    t.RuleFor(x => x.ObligationTypeId)
                        .GreaterThan(0)
                        .WithErrorCode("Households.Customers.");

                    t.When(x => x.ObligationTypeId == 3 || x.ObligationTypeId == 4, () =>
                    {
                        t.RuleFor(x => x.Installment)
                            .Empty()
                            .WithMessage("Installment must be empty for ObligationTypeId in (3,4)")
                            .WithErrorCode("Households.Customers.");

                        t.RuleFor(x => x.InstallmentConsolidated)
                            .Empty()
                            .WithMessage("InstallmentConsolidated must be empty for ObligationTypeId in (3,4)")
                            .WithErrorCode("Households.Customers.");
                    });
                });
        });

        When(t => t.Income is not null, () =>
        {
            RuleFor(t => t.Income)
                .ChildRules(t2 =>
                {
                    t2.When(z => z!.EmploymentIncomes is not null, () =>
                    {
                        t2.RuleForEach(x => x!.EmploymentIncomes)
                            .ChildRules(x =>
                            {
                                x.When(x2 => x2.MonthlyIncomeAmount is not null, () =>
                                {
                                    x.RuleFor(t => t.MonthlyIncomeAmount!.Amount)
                                        .GreaterThan(0)
                                        .WithErrorCode("Households.Customers.EmploymentIncomes.MonthlyAmount.Amount");
                                });
                            });
                    });

                    t2.When(z => z!.OtherIncomes is not null, () =>
                    {
                        t2.RuleForEach(x => x!.OtherIncomes)
                            .ChildRules(x =>
                            {
                                x.RuleFor(x2 => x2.IncomeOtherTypeId)
                                    .GreaterThan(0)
                                    .WithErrorCode("Households.Customers.OtherIncomes.IncomeOtherTypeId");

                                x.RuleFor(x2 => x2.MonthlyIncomeAmount)
                                    .Cascade(CascadeMode.Stop)
                                    .NotNull()
                                    .WithErrorCode("Households.Customers.OtherIncomes.MonthlyAmount")
                                    .ChildRules(x3 =>
                                    {
                                        x3.RuleFor(t => t.Amount)
                                            .GreaterThan(0)
                                            .WithErrorCode("Households.Customers.OtherIncomes.MonthlyAmount.Amount");
                                    });
                            });
                    });
                });
        });
    }
}
