using DomainServices.HouseholdService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateObligation;

internal class CreateObligationHandler
    : IRequestHandler<CreateObligationMediatrRequest, CreateObligationResponse>
{
    public async Task<CreateObligationResponse> Handle(CreateObligationMediatrRequest request, CancellationToken cancellation)
    {
        // check customer existence
        if (!await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId, cancellation))
            throw new CisNotFoundException(16020, "CustomerOnSA", request.Request.CustomerOnSAId);

        var entity = new Database.Entities.CustomerOnSAObligation
        {
            CustomerOnSAId = request.Request.CustomerOnSAId,
            ObligationState = request.Request.ObligationState,
            ObligationTypeId = request.Request.ObligationTypeId!.Value,
            InstallmentAmount = request.Request.InstallmentAmount,
            LoanPrincipalAmount = request.Request.LoanPrincipalAmount,
            CreditCardLimit = request.Request.CreditCardLimit,
            AmountConsolidated = request.Request.AmountConsolidated,
            CreditorId = request.Request.Creditor?.CreditorId ?? "",
            CreditorName = request.Request.Creditor?.Name ?? "",
            CreditorIsExternal = request.Request.Creditor?.IsExternal,
            CorrectionTypeId = request.Request.Correction?.CorrectionTypeId,
            CreditCardLimitCorrection = request.Request.Correction?.CreditCardLimitCorrection,
            InstallmentAmountCorrection = request.Request.Correction?.InstallmentAmountCorrection,
            LoanPrincipalAmountCorrection = request.Request.Correction?.LoanPrincipalAmountCorrection
        };

        _dbContext.CustomersObligations.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);

        _logger.EntityCreated(nameof(Database.Entities.CustomerOnSAObligation), entity.CustomerOnSAObligationId);

        return new CreateObligationResponse
        {
            ObligationId = entity.CustomerOnSAObligationId
        };
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<CreateObligationHandler> _logger;

    public CreateObligationHandler(
        Database.HouseholdServiceDbContext dbContext,
        ILogger<CreateObligationHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
