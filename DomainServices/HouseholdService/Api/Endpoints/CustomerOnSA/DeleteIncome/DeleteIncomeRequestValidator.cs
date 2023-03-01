using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteIncome;

internal sealed class DeleteIncomeRequestValidator
    : AbstractValidator<Contracts.DeleteIncomeRequest>
{
    public DeleteIncomeRequestValidator(Database.HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.IncomeId)
            .MustAsync(async (incomeId, cancellationToken) => await dbContext.CustomersIncomes.AnyAsync(t => t.CustomerOnSAIncomeId == incomeId, cancellationToken))
            .WithErrorCode(ErrorCodeMapper.IncomeNotFound)
            .ThrowCisException(GrpcValidationBehaviorExceptionTypes.CisNotFoundException);
    }
}
