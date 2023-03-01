using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.LinkCustomerOnSAToHousehold;

internal sealed class LinkCustomerOnSAToHouseholdRequestValidator
    : AbstractValidator<LinkCustomerOnSAToHouseholdRequest>
{
    public LinkCustomerOnSAToHouseholdRequestValidator(HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.CustomerOnSAId1)
            .NotNull()
            .When(t => t.CustomerOnSAId2.HasValue)
            .WithErrorCode(ErrorCodeMapper.Customer2WithoutCustomer1);

        RuleFor(t => t.HouseholdId)
            .MustAsync(async (householdId, cancellationToken) => await dbContext.Households.FindAsync(new object[] { householdId }, cancellationToken) is not null)
            .WithErrorCode(ErrorCodeMapper.HouseholdNotFound)
            .ThrowCisException(GrpcValidationBehaviorExeptionTypes.CisNotFoundException);

        RuleFor(t => t.CustomerOnSAId1)
            .MustAsync(async (request, customerOnSAId, cancellationToken) =>
            {
                var household = await dbContext.Households.FindAsync(new object[] { request.HouseholdId }, cancellationToken);
                return await dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerOnSAId && t.SalesArrangementId == household!.SalesArrangementId, cancellationToken);
            })
            .WithErrorCode(ErrorCodeMapper.CustomerNotOnSA)
            .When(t => t.CustomerOnSAId1.HasValue);

        RuleFor(t => t.CustomerOnSAId2)
            .MustAsync(async (request, customerOnSAId, cancellationToken) =>
            {
                var household = await dbContext.Households.FindAsync(new object[] { request.HouseholdId }, cancellationToken);
                return await dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerOnSAId && t.SalesArrangementId == household!.SalesArrangementId, cancellationToken);
            })
            .WithErrorCode(ErrorCodeMapper.CustomerNotOnSA)
            .When(t => t.CustomerOnSAId2.HasValue);
    }
}
