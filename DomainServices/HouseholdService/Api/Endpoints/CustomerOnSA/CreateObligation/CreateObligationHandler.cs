using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateObligation;

internal class CreateObligationHandler
    : IRequestHandler<CreateObligationRequest, CreateObligationResponse>
{
    public async Task<CreateObligationResponse> Handle(CreateObligationRequest request, CancellationToken cancellation)
    {
        // check customer existence
        if (!await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.CustomerOnSAId, cancellation))
            throw new CisNotFoundException(16020, "CustomerOnSA", request.CustomerOnSAId);

        var entity = new Database.Entities.CustomerOnSAObligation
        {
            CustomerOnSAId = request.CustomerOnSAId,
            ObligationState = request.ObligationState,
            ObligationTypeId = request.ObligationTypeId!.Value,
            InstallmentAmount = request.InstallmentAmount,
            LoanPrincipalAmount = request.LoanPrincipalAmount,
            CreditCardLimit = request.CreditCardLimit,
            AmountConsolidated = request.AmountConsolidated,
            CreditorId = request.Creditor?.CreditorId ?? "",
            CreditorName = request.Creditor?.Name ?? "",
            CreditorIsExternal = request.Creditor?.IsExternal,
            CorrectionTypeId = request.Correction?.CorrectionTypeId,
            CreditCardLimitCorrection = request.Correction?.CreditCardLimitCorrection,
            InstallmentAmountCorrection = request.Correction?.InstallmentAmountCorrection,
            LoanPrincipalAmountCorrection = request.Correction?.LoanPrincipalAmountCorrection
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
