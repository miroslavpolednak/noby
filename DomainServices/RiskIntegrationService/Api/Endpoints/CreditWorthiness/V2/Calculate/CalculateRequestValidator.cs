﻿using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.Calculate;

internal sealed class CalculateRequestValidator
    : AbstractValidator<Contracts.CreditWorthiness.V2.CreditWorthinessCalculateRequest>
{
    public CalculateRequestValidator()
    {
        RuleFor(t => t.ResourceProcessId)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.ResourceProcessIdIsEmpty);

        RuleFor(t => t.Product)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithErrorCode("Product")
            .ChildRules(t =>
            {
                t.RuleFor(t => t!.ProductTypeId)
                    .GreaterThan(0)
                    .WithErrorCode("Product.ProductTypeId");
                t.RuleFor(t => t!.LoanInterestRate)
                    .GreaterThan(0)
                    .WithErrorCode("Product.LoanInterestRate");
                t.RuleFor(t => t!.LoanAmount)
                    .GreaterThan(0)
                    .WithErrorCode("Product.LoanAmount");
                t.RuleFor(t => t!.LoanPaymentAmount)
                    .GreaterThan(0)
                    .WithErrorCode("Product.LoanPaymentAmount");
                t.RuleFor(t => t!.FixedRatePeriod)
                    .GreaterThan(0)
                    .WithErrorCode("Product.FixedRatePeriod");
                t.RuleFor(t => t!.LoanDuration)
                    .GreaterThan(0)
                    .WithErrorCode("Product.LoanDuration");
            })
            .WithErrorCode("Product");

        RuleFor(t => t.Households)
            .NotEmpty()
            .WithErrorCode("Households");

        RuleForEach(t => t.Households)
            .ChildRules(t =>
            {
                t.RuleFor(t => t!.Customers)
                    .NotEmpty()
                    .WithErrorCode("Households.Customers");

                t.RuleForEach(t => t!.Customers)
                    .ChildRules(x =>
                    {
                        x.When(z => z.Incomes is not null, () =>
                        {
                            x.RuleForEach(t => t!.Incomes)
                                .ChildRules(y =>
                                {
                                    y.RuleFor(t => t!.IncomeTypeId)
                                        .GreaterThan(0);
                                })
                                .WithErrorCode("Households.Customers.Incomes");
                        });
                    })
                    .WithErrorCode("Households.Customers");
            })
            .WithErrorCode("Households");

        When(t => t.UserIdentity is not null, () =>
        {
            RuleFor(t => t.UserIdentity)
                .SetValidator(new IdentityValidator())
                .WithErrorCode("UserIdentity");
        });
    }
}
