﻿using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

internal sealed class IdentifyByIdentityRollback
    : IRollbackAction<IdentifyByIdentityRequest>
{
    public async Task ExecuteRollback(Exception exception, IdentifyByIdentityRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RollbackHandlerStarted(nameof(IdentifyByIdentityRollback));

        // vratit zpatky data puvodniho customera
        if (_bag.ContainsKey(BagKeyCustomerOnSA))
        {
            var customerInstance = (_HO.CustomerOnSA)_bag[BagKeyCustomerOnSA]!;

            await _customerOnSAService.UpdateCustomer(customerInstance, cancellationToken);

            _logger.RollbackHandlerStepDone(BagKeyCustomerOnSA, _bag[BagKeyCustomerOnSA]!);
        }
    }

    public bool OverrideThrownException { get => false; }

    public const string BagKeyCustomerOnSA = "BagKeyCustomerOnSA";

    private readonly IRollbackBag _bag;
    private readonly ILogger<IdentifyByIdentityRollback> _logger;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    
    public IdentifyByIdentityRollback(
        IRollbackBag bag,
        ILogger<IdentifyByIdentityRollback> logger,
        ICustomerOnSAServiceClient customerOnSAService)
    {
        _logger = logger;
        _bag = bag;
        _customerOnSAService = customerOnSAService;
    }
}
