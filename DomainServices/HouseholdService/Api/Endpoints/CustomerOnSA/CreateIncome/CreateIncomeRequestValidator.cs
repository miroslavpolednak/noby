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
            .WithErrorCode(ErrorCodeMapper.CustomerOnSAIdIsEmpty);

        // existuje customer
        RuleFor(t => t.CustomerOnSAId)
            .MustAsync(async (customerOnSAId, cancellationToken) => await dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerOnSAId, cancellationToken))
            .WithErrorCode(ErrorCodeMapper.CustomerOnSANotFound)
            .ThrowCisException(GrpcValidationBehaviorExeptionTypes.CisNotFoundException);

        RuleFor(t => t.IncomeTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.IncomeTypeIdIsEmpty)
            .Must(t => (HouseholdTypes)t != HouseholdTypes.Unknown)
            .WithErrorCode(ErrorCodeMapper.IncomeTypeIdIsEmpty);

        RuleFor(t => t.BaseData)
            .SetInheritanceValidator(v =>
            {
                v.Add(new Validators.IncomeBaseDataValidator(codebookService));
            });

        // nelze uvést Cin a BirthNumber zároveň
        RuleFor(t => t.Employement)
            .Must(t => !(!string.IsNullOrEmpty(t.Employer.Cin) && !string.IsNullOrEmpty(t.Employer.BirthNumber)))
            .WithErrorCode(ErrorCodeMapper.EmployementCinBirthNo)
            .When(t => t.Employement is not null);

        // kontrola poctu prijmu
        RuleFor(t => t.IncomeTypeId)
            .MustAsync(async (request, incomeTypeId, cancellationToken) =>
            {
                CustomerIncomeTypes incomeType = (CustomerIncomeTypes)incomeTypeId;

                int totalIncomesOfType = await dbContext
                    .CustomersIncomes
                    .CountAsync(t => t.CustomerOnSAId == request.CustomerOnSAId && t.IncomeTypeId == incomeType, cancellationToken);

                return !IncomeHelpers.AlreadyHasMaxIncomes(incomeType, totalIncomesOfType);
            })
            .WithErrorCode(ErrorCodeMapper.MaxIncomesReached);
    }
}