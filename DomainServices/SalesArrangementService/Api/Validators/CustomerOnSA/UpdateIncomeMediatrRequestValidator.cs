﻿using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class UpdateIncomeMediatrRequestValidator
    : AbstractValidator<Dto.UpdateIncomeMediatrRequest>
{
    public UpdateIncomeMediatrRequestValidator(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.Request.IncomeId)
            .GreaterThan(0)
            .WithMessage("IncomeId must be > 0").WithErrorCode("16055");

        RuleFor(t => t.Request.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add<Contracts.IncomeBaseData>(new IncomeBaseDataValidator(codebookService));
            });

        RuleFor(t => t.Request.Employement)
            .Must(t => {
                if (t?.Employer is null)
                {
                    return true;
                }
                // nelze uvést Cin a BirthNumber zároveň
                return !(!String.IsNullOrEmpty(t.Employer.Cin) && !String.IsNullOrEmpty(t.Employer.BirthNumber));
            })
            .WithMessage("Only one of values can be set [Employement.Employer.Cin, Employement.Employer.BirthNumber]").WithErrorCode("16046");
    }
}