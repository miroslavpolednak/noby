﻿using CIS.Infrastructure.MediatR.Rollback;
using DomainServices.CaseService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Offer.CreateMortgageCase;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.AsImplementedInterfacesService]
internal class CreateMortgageCaseRollback
    : IRollbackAction<CreateMortgageCaseRequest>
{
    public async Task ExecuteRollback(Exception exception, CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        // smazat domacnost a customery
        if (_bag.ContainsKey(BagKeyHouseholdId))
            await _householdService.DeleteHousehold((int)_bag[BagKeyHouseholdId]!, true, cancellationToken);

        // smazat SA
        if (_bag.ContainsKey(BagKeySalesArrangementId))
            await _salesArrangementService.DeleteSalesArrangement((int)_bag[BagKeySalesArrangementId]!, true, cancellationToken);

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
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICaseServiceClient _caseService;

    public CreateMortgageCaseRollback(
        IRollbackBag bag,
        ILogger<CreateMortgageCaseRollback> logger,
        ICustomerOnSAServiceClient customerOnSAService,
        ISalesArrangementServiceClient salesArrangementService,
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
