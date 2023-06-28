using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.CreateIncome;

internal sealed class CreateIncomeRequestValidator
    : AbstractValidator<CreateIncomeRequest>
{
    public CreateIncomeRequestValidator(ICodebookServiceClient codebookService, Database.HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CustomerOnSAIdIsEmpty);

        // existuje customer
        RuleFor(t => t.CustomerOnSAId)
            .MustAsync(async (customerOnSAId, cancellationToken) => await dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerOnSAId, cancellationToken))
            .WithErrorCode(ErrorCodeMapper.CustomerOnSANotFound)
            .ThrowCisException(GrpcValidationBehaviorExceptionTypes.CisNotFoundException);

        RuleFor(t => t)
            .SetValidator(new Validators.IncomeValidator(codebookService));

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