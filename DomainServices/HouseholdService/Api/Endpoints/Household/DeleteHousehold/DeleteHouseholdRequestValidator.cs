using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.DeleteHousehold;

internal sealed class DeleteHouseholdRequestValidator
    : AbstractValidator<Contracts.DeleteHouseholdRequest>
{
    public DeleteHouseholdRequestValidator(Database.HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.HouseholdId)
            .MustAsync(async (householdId, cancellationToken) => await dbContext.Households.FindAsync(new object[] { householdId }, cancellationToken) is not null)
            .WithErrorCode(ValidationMessages.HouseholdNotFound)
            .ThrowCisException(GrpcValidationBehaviorExeptionTypes.CisNotFoundException);

        RuleFor(t => t.HouseholdId)
            .MustAsync(async (householdId, cancellationToken) =>
            {
                var householdInstance = await dbContext.Households.FindAsync(new object[] { householdId }, cancellationToken);
                return householdInstance!.HouseholdTypeId != CIS.Foms.Enums.HouseholdTypes.Main;
            })
            .WithErrorCode(ValidationMessages.CantDeleteDebtorHousehold)
            .When(request => !request.HardDelete);
    }
}
