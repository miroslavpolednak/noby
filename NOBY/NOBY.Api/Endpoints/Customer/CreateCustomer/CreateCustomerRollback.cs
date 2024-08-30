using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.HouseholdService.Clients.v1;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Customer.CreateCustomer;

internal sealed class CreateCustomerRollback(
    IRollbackBag _bag,
    ILogger<CreateCustomerRollback> _logger,
    ICustomerOnSAServiceClient _customerOnSAService)
        : IRollbackAction<CustomerCreateCustomerRequest>
{
    public async Task ExecuteRollback(Exception exception, CustomerCreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RollbackHandlerStarted(nameof(CreateCustomerRollback));

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
