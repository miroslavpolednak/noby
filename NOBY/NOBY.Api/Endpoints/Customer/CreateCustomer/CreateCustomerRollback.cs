using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Customer.CreateCustomer;

internal sealed class CreateCustomerRollback
    : IRollbackAction<CreateCustomerRequest>
{
    public async Task ExecuteRollback(Exception exception, CreateCustomerRequest request, CancellationToken cancellationToken = default)
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

    private readonly IRollbackBag _bag;
    private readonly ILogger<CreateCustomerRollback> _logger;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public CreateCustomerRollback(
        IRollbackBag bag,
        ILogger<CreateCustomerRollback> logger,
        ICustomerOnSAServiceClient customerOnSAService)
    {
        _logger = logger;
        _bag = bag;
        _customerOnSAService = customerOnSAService;
    }
}
