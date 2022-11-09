using CIS.Infrastructure.MediatR.Rollback;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Offer.CreateMortgageCase;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.AsImplementedInterfacesService]
internal class CreateMortgageCaseRollback
    : IRollbackAction<CreateMortgageCaseRequest>
{
    public async Task ExecuteRollback(Exception exception, CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        // smazat domacnost
        if (_bag.ContainsKey(BagKeyHouseholdId))
            await _householdService.DeleteHousehold((int)_bag[BagKeyHouseholdId]!, cancellationToken);

        // smazat customer on SA
        if (_bag.ContainsKey(BagKeyCustomerOnSAId))
            await _customerOnSAService.DeleteCustomer((int)_bag[BagKeyCustomerOnSAId]!, cancellationToken);

        // smazat SA
        if (_bag.ContainsKey(BagKeySalesArrangementId))
            await _salesArrangementService.DeleteSalesArrangement((int)_bag[BagKeySalesArrangementId]!, cancellationToken);

        // smazat case
        if (_bag.ContainsKey(BagKeyCaseId))
            await _caseService.DeleteCase((long)_bag[BagKeyCaseId]!, cancellationToken);
    }

    public const string BagKeyCaseId = "CaseId";
    public const string BagKeySalesArrangementId = "SAId";
    public const string BagKeyProductId = "ProductId";
    public const string BagKeyHouseholdId = "HouseholdId";
    public const string BagKeyCustomerOnSAId = "CustomerOnSAId";

    private readonly IRollbackBag _bag;
    private readonly ILogger<CreateMortgageCaseRollback> _logger;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ISalesArrangementServiceClients _salesArrangementService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICaseServiceClient _caseService;

    public CreateMortgageCaseRollback(
        IRollbackBag bag,
        ILogger<CreateMortgageCaseRollback> logger,
        ICustomerOnSAServiceClient customerOnSAService,
        ISalesArrangementServiceClients salesArrangementService,
        IHouseholdServiceClient householdService,
        ICaseServiceClient caseService)
    {
        _logger = logger;
        _bag = bag;
        _customerOnSAService = customerOnSAService;
        _caseService = caseService;
        _householdService = householdService;
        _salesArrangementService = salesArrangementService;
    }
}
