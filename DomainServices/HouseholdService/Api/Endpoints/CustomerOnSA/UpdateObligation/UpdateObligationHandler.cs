﻿using Microsoft.EntityFrameworkCore;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateObligation;

internal sealed class UpdateObligationHandler
    : IRequestHandler<UpdateObligationMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateObligationMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.CustomersObligations
            .Where(t => t.CustomerOnSAObligationId == request.Request.ObligationId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16042, $"Obligation ID {request.Request.ObligationId} does not exist.");

        entity.ObligationState = request.Request.ObligationState;
        entity.ObligationTypeId = request.Request.ObligationTypeId!.Value;
        entity.InstallmentAmount = request.Request.InstallmentAmount;
        entity.LoanPrincipalAmount = request.Request.LoanPrincipalAmount;
        entity.CreditCardLimit = request.Request.CreditCardLimit;
        entity.AmountConsolidated = request.Request.AmountConsolidated;
        entity.CreditorId = request.Request.Creditor?.CreditorId;
        entity.CreditorName = request.Request.Creditor?.Name;
        entity.CreditorIsExternal = request.Request.Creditor?.IsExternal;
        entity.CorrectionTypeId = request.Request.Correction?.CorrectionTypeId;
        entity.CreditCardLimitCorrection = request.Request.Correction?.CreditCardLimitCorrection;
        entity.InstallmentAmountCorrection = request.Request.Correction?.InstallmentAmountCorrection;
        entity.LoanPrincipalAmountCorrection = request.Request.Correction?.LoanPrincipalAmountCorrection;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.HouseholdServiceDbContext _dbContext;

    public UpdateObligationHandler(Repositories.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
