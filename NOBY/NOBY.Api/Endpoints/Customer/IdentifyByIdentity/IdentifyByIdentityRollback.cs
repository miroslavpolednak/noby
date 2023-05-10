using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.HouseholdService.Clients;
using NOBY.Api.Endpoints.Offer.CreateMortgageCase;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

internal sealed class IdentifyByIdentityRollback
    : IRollbackAction<IdentifyByIdentityRequest>
{
    public async Task ExecuteRollback(Exception exception, IdentifyByIdentityRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RollbackHandlerStarted(nameof(CreateMortgageCaseRollback));

        // vratit zpatky data puvodniho customera
        if (_bag.ContainsKey(BagKeyCustomerOnSA))
        {
            var customerInstance = (_HO.CustomerOnSA)_bag[BagKeyCustomerOnSA]!;

            var modelToUpdate = new _HO.UpdateCustomerRequest
            {
                CustomerOnSAId = customerInstance.CustomerOnSAId,
                Customer = new _HO.CustomerOnSABase
                {
                    DateOfBirthNaturalPerson = customerInstance.DateOfBirthNaturalPerson,
                    FirstNameNaturalPerson = customerInstance.FirstNameNaturalPerson,
                    Name = customerInstance.Name,
                    LockedIncomeDateTime = customerInstance.LockedIncomeDateTime,
                    MaritalStatusId = customerInstance.MaritalStatusId
                }
            };
            
            if (customerInstance.CustomerIdentifiers is not null)
            {
                modelToUpdate.Customer.CustomerIdentifiers.Add(customerInstance.CustomerIdentifiers);
            }

            await _customerOnSAService.UpdateCustomer(modelToUpdate, cancellationToken);

            _logger.RollbackHandlerStepDone(BagKeyCustomerOnSA, _bag[BagKeyCustomerOnSA]!);
        }
    }

    public bool OverrideThrownException { get => true; }

    public Exception OnOverrideException(Exception exception)
        => exception is NobyValidationException ? exception : new NobyValidationException(exception.Message);

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
