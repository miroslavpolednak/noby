using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.UpdateHousehold;

internal sealed class UpdateHouseholdRequestValidator
    : AbstractValidator<UpdateHouseholdRequest>
{
    public UpdateHouseholdRequestValidator(HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.HouseholdId)
            .GreaterThan(0)
            .WithErrorCode(ValidationMessages.HouseholdIdIsEmpty);

        RuleFor(t => t.CustomerOnSAId1)
            .NotNull()
            .When(t => t.CustomerOnSAId2.HasValue)
            .WithErrorCode(ValidationMessages.Customer2WithoutCustomer1);

        RuleFor(t => t.HouseholdId)
            .MustAsync(async (householdId, cancellationToken) => await dbContext.Households.FindAsync(householdId, cancellationToken) is not null)
            .WithErrorCode(ValidationMessages.HouseholdNotFound)
            .ThrowCisException(GrpcValidationBehaviorExeptionTypes.CisNotFoundException);

        RuleFor(t => t.CustomerOnSAId1)
            .MustAsync(async (request, customerOnSAId, cancellationToken) =>
            {
                var household = await dbContext.Households.FindAsync(request.HouseholdId, cancellationToken);
                return await dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerOnSAId && t.SalesArrangementId == household!.SalesArrangementId, cancellationToken);
            })
            .WithErrorCode(ValidationMessages.CustomerNotOnSA)
            .When(t => t.CustomerOnSAId1.HasValue);

        RuleFor(t => t.CustomerOnSAId2)
            .MustAsync(async (request, customerOnSAId, cancellationToken) =>
            {
                var household = await dbContext.Households.FindAsync(request.HouseholdId, cancellationToken);
                return await dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerOnSAId && t.SalesArrangementId == household!.SalesArrangementId, cancellationToken);
            })
            .WithErrorCode(ValidationMessages.CustomerNotOnSA)
            .When(t => t.CustomerOnSAId2.HasValue);
    }
}