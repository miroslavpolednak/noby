using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteObligation;

internal sealed class DeleteObligationRequestValidator
    : AbstractValidator<DeleteObligationRequest>
{
    public DeleteObligationRequestValidator(Database.HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.ObligationId)
            .MustAsync(async (obligationId, cancellationToken) =>
            {
                return await dbContext.CustomersObligations.FindAsync(new object[] { obligationId }, cancellationToken) != null;
            })
            .WithErrorCode(ValidationMessages.ObligationNotFound)
            .ThrowCisException(GrpcValidationBehaviorExeptionTypes.CisNotFoundException);
    }
}
