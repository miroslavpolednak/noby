using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Abstraction.Services;

internal class CustomerOnSAService : ICustomerOnSAServiceAbstraction
{
    public async Task<IServiceCallResult> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateCustomer), request.SalesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.CreateCustomerAsync(request, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<int>(result.CustomerOnSAId);
    }

    public async Task<IServiceCallResult> DeleteCustomer(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteCustomer), customerOnSAId);
        var result = await _userContext.AddUserContext(async () => await _service.DeleteCustomerAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetCustomer(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCustomer), customerOnSAId);
        var result = await _userContext.AddUserContext(async () => await _service.GetCustomerAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<CustomerOnSA>(result);
    }

    public async Task<IServiceCallResult> GetCustomerList(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCustomerList), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.GetCustomerListAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<List<CustomerOnSA>>(result.Customers.ToList());
    }

    public async Task<IServiceCallResult> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateCustomer), request.CustomerOnSAId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateCustomerAsync(request, cancellationToken: cancellationToken));
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateObligations(UpdateObligationsRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateObligations), request.CustomerOnSAId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateObligationsAsync(request, cancellationToken: cancellationToken));
        return new SuccessfulServiceCallResult();
    }

    private readonly ILogger<CustomerOnSAService> _logger;
    private readonly Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient _service;
    private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;

    public CustomerOnSAService(
        ILogger<CustomerOnSAService> logger,
        Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient service,
        CIS.Security.InternalServices.ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }
}