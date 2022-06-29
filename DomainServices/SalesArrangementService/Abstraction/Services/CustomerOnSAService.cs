using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Abstraction.Services;

internal class CustomerOnSAService : ICustomerOnSAServiceAbstraction
{
    public async Task<IServiceCallResult> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateCustomer), request.SalesArrangementId);
        var result = await _service.CreateCustomerAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<CreateCustomerResponse>(result);
    }

    public async Task<IServiceCallResult> DeleteCustomer(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteCustomer), customerOnSAId);
        var result = await _service.DeleteCustomerAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetCustomer(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCustomer), customerOnSAId);
        var result = await _service.GetCustomerAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<CustomerOnSA>(result);
    }

    public async Task<IServiceCallResult> GetCustomerList(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCustomerList), salesArrangementId);
        var result = await _service.GetCustomerListAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<List<CustomerOnSA>>(result.Customers.ToList());
    }

    public async Task<IServiceCallResult> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateCustomer), request.CustomerOnSAId);
        var result = await _service.UpdateCustomerAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<UpdateCustomerResponse>(result);
    }

    public async Task<IServiceCallResult> UpdateObligations(UpdateObligationsRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateObligations), request.CustomerOnSAId);
        var result = await _service.UpdateObligationsAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    #region Income
    public async Task<IServiceCallResult> CreateIncome(CreateIncomeRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateIncome), request.CustomerOnSAId);
        var result = await _service.CreateIncomeAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<int>(result.IncomeId);
    }

    public async Task<IServiceCallResult> DeleteIncome(int incomeId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteIncome), incomeId);
        var result = await _service.DeleteIncomeAsync(
            new()
            {
                IncomeId = incomeId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetIncome(int incomeId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetIncome), incomeId);
        var result = await _service.GetIncomeAsync(
            new()
            {
                IncomeId = incomeId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<Income>(result);
    }

    public async Task<IServiceCallResult> GetIncomeList(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetIncomeList), customerOnSAId);
        var result = await _service.GetIncomeListAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<List<IncomeInList>>(result.Incomes.ToList());
    }

    public async Task<IServiceCallResult> UpdateIncome(UpdateIncomeRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateIncome), request.IncomeId);
        var result = await _service.UpdateIncomeAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateIncomeBaseData(UpdateIncomeBaseDataRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateIncomeBaseData), request.IncomeId);
        var result = await _service.UpdateIncomeBaseDataAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }
    #endregion Income

    private readonly ILogger<CustomerOnSAService> _logger;
    private readonly Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient _service;

    public CustomerOnSAService(
        ILogger<CustomerOnSAService> logger,
        Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient service)
    {
        _service = service;
        _logger = logger;
    }
}