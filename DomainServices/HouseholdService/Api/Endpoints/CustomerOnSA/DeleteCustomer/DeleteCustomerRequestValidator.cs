using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteCustomer;

internal sealed class DeleteCustomerRequestValidator
    : AbstractValidator<DeleteCustomerRequest>
{
    public DeleteCustomerRequestValidator(Database.HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CustomerOnSAIdIsEmpty);

        // kontrola existence customera
        RuleFor(t => t.CustomerOnSAId)
            .MustAsync(async (customerOnSAId, cancellationToken) =>
            {
                return await dbContext.Customers.FindAsync(new object[] { customerOnSAId }, cancellationToken) != null;
            })
            .WithErrorCode(ErrorCodeMapper.CustomerOnSANotFound)
            .ThrowCisException(GrpcValidationBehaviorExeptionTypes.CisNotFoundException);

        // kontrola ze nemazu Debtora
        RuleFor(t => t.CustomerOnSAId)
            .MustAsync(async (request, customerOnSAId, cancellationToken) =>
            {
                var entity = await dbContext.Customers.FindAsync(new object[] { customerOnSAId }, cancellationToken);
                return !(entity!.CustomerRoleId == CIS.Foms.Enums.CustomerRoles.Debtor && !request.HardDelete);
            })
            .WithErrorCode(ErrorCodeMapper.CantDeleteDebtor);
    }
}
