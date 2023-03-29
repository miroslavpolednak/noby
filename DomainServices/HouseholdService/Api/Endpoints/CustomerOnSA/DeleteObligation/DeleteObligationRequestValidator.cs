using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteObligation;

internal sealed class DeleteObligationRequestValidator
    : AbstractValidator<Contracts.DeleteObligationRequest>
{
    public DeleteObligationRequestValidator(Database.HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.ObligationId)
            .MustAsync(async (obligationId, cancellationToken) => await dbContext.CustomersObligations.AnyAsync(t => t.CustomerOnSAObligationId == obligationId, cancellationToken))
            .WithErrorCode(ErrorCodeMapper.ObligationNotFound)
            .ThrowCisException(GrpcValidationBehaviorExceptionTypes.CisNotFoundException);
    }
}