using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteIncome;

internal sealed class DeleteIncomeRequestValidator
    : AbstractValidator<DeleteIncomeRequest>
{
    public DeleteIncomeRequestValidator(Database.HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.IncomeId)
            .MustAsync(async (incomeId, cancellationToken) =>
            {
                return await dbContext.CustomersIncomes.FindAsync(new object[] { incomeId }, cancellationToken) != null;
            })
            .WithErrorCode(ErrorCodeMapper.IncomeNotFound)
            .ThrowCisException(GrpcValidationBehaviorExeptionTypes.CisNotFoundException);
    }
}
