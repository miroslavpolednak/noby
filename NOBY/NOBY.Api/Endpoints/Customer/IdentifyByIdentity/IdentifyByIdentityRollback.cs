using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

internal sealed class IdentifyByIdentityRollback(
    IRollbackBag _bag,
    ILogger<IdentifyByIdentityRollback> _logger,
    ICustomerOnSAServiceClient _customerOnSAService)
        : IRollbackAction<CustomerIdentifyByIdentityRequest>
{
    public async Task ExecuteRollback(Exception exception, CustomerIdentifyByIdentityRequest request, CancellationToken cancellationToken = default)
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
}
