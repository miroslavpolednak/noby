using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.CaseService.Clients.v1;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Offer.CreateMortgageCase;

internal sealed class CreateMortgageCaseRollback(
    IRollbackBag _bag,
    ILogger<CreateMortgageCaseRollback> _logger,
    ISalesArrangementServiceClient _salesArrangementService,
    IHouseholdServiceClient _householdService,
    ICaseServiceClient _caseService)
        : IRollbackAction<OfferCreateMortgageCaseRequest>
{
    public async Task ExecuteRollback(Exception exception, OfferCreateMortgageCaseRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RollbackHandlerStarted(nameof(CreateMortgageCaseRollback));

        // smazat domacnost a customery
        if (_bag.ContainsKey(BagKeyHouseholdId))
        {
            await _householdService.DeleteHousehold((int)_bag[BagKeyHouseholdId]!, true, cancellationToken);
            _logger.RollbackHandlerStepDone(BagKeyHouseholdId, _bag[BagKeyHouseholdId]!);
        }

        // smazat SA
        if (_bag.ContainsKey(BagKeySalesArrangementId))
        {
            await _salesArrangementService.DeleteSalesArrangement((int)_bag[BagKeySalesArrangementId]!, true, cancellationToken);
            _logger.RollbackHandlerStepDone(BagKeySalesArrangementId, _bag[BagKeySalesArrangementId]!);
        }

        // smazat case
        if (_bag.ContainsKey(BagKeyCaseId))
        {
            await _caseService.DeleteCase((long)_bag[BagKeyCaseId]!, cancellationToken);
            _logger.RollbackHandlerStepDone(BagKeyCaseId, _bag[BagKeyCaseId]!);
        }
    }

    public const string BagKeyCaseId = "CaseId";
    public const string BagKeySalesArrangementId = "SAId";
    public const string BagKeyProductId = "ProductId";
    public const string BagKeyHouseholdId = "HouseholdId";
    public const string BagKeyCustomerOnSAId = "CustomerOnSAId";
}
