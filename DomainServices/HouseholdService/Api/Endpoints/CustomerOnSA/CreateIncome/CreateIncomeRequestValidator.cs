using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateIncome;

internal sealed class CreateIncomeRequestValidator
    : AbstractValidator<CreateIncomeRequest>
{
    public CreateIncomeRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService, Database.HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithErrorCode(ValidationMessages.CustomerOnSAIdIsEmpty);

        RuleFor(t => t.IncomeTypeId)
            .GreaterThan(0)
            .WithErrorCode(ValidationMessages.IncomeTypeIdIsEmpty)
            .Must(t => (CIS.Foms.Enums.HouseholdTypes)t != CIS.Foms.Enums.HouseholdTypes.Unknown)
            .WithErrorCode(ValidationMessages.IncomeTypeIdIsEmpty);

        RuleFor(t => t.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add(new Validators.IncomeBaseDataValidator(codebookService));
            });

        // nelze uvést Cin a BirthNumber zároveň
        RuleFor(t => t.Employement)
            .Must(t => !(!string.IsNullOrEmpty(t.Employer.Cin) && !string.IsNullOrEmpty(t.Employer.BirthNumber)))
            .WithErrorCode(ValidationMessages.EmployementCinBirthNo)
            .When(t => t.Employement is not null);

        RuleFor(t => t.CustomerOnSAId)
            .Must(customerOnSAId => dbContext.Customers.Any(t => t.CustomerOnSAId == customerOnSAId))
            .WithErrorCode(ValidationMessages.CustomerOnSANotFound)
            .ThrowCisException(GrpcValidationBehaviorExeptionTypes.CisNotFoundException);
    }
}