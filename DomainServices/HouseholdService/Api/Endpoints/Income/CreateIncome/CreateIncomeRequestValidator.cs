using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.CreateIncome;

internal sealed class CreateIncomeRequestValidator
    : AbstractValidator<CreateIncomeRequest>
{
    public CreateIncomeRequestValidator(ICodebookServiceClient codebookService, Database.HouseholdServiceDbContext dbContext, IDocumentDataStorage documentDataStorage)
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
                EnumIncomeTypes incomeType = (EnumIncomeTypes)incomeTypeId;

                var incomes = await documentDataStorage.GetList<Database.DocumentDataEntities.Income, int>(request.CustomerOnSAId, cancellationToken);
                int totalIncomesOfType = incomes.Count(t => t.Data?.IncomeTypeId == incomeType);

                return !IncomeHelpers.AlreadyHasMaxIncomes(incomeType, totalIncomesOfType);
            })
            .WithErrorCode(ErrorCodeMapper.MaxIncomesReached);
    }
}