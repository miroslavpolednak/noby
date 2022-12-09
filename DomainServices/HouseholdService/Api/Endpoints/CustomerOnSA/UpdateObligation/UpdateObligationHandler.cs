using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateObligation;

internal sealed class UpdateObligationHandler
    : IRequestHandler<Obligation, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Obligation request, CancellationToken cancellation)
    {
        var entity = await _dbContext.CustomersObligations
            .Where(t => t.CustomerOnSAObligationId == request.ObligationId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16042, $"Obligation ID {request.ObligationId} does not exist.");

        entity.ObligationState = request.ObligationState;
        entity.ObligationTypeId = request.ObligationTypeId!.Value;
        entity.InstallmentAmount = request.InstallmentAmount;
        entity.LoanPrincipalAmount = request.LoanPrincipalAmount;
        entity.CreditCardLimit = request.CreditCardLimit;
        entity.AmountConsolidated = request.AmountConsolidated;
        entity.CreditorId = request.Creditor?.CreditorId;
        entity.CreditorName = request.Creditor?.Name;
        entity.CreditorIsExternal = request.Creditor?.IsExternal;
        entity.CorrectionTypeId = request.Correction?.CorrectionTypeId;
        entity.CreditCardLimitCorrection = request.Correction?.CreditCardLimitCorrection;
        entity.InstallmentAmountCorrection = request.Correction?.InstallmentAmountCorrection;
        entity.LoanPrincipalAmountCorrection = request.Correction?.LoanPrincipalAmountCorrection;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public UpdateObligationHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
