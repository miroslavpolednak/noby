using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients.Services;

internal class CustomerOnSAService : ICustomerOnSAServiceClient
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

    #region Obligation
    public async Task<IServiceCallResult> CreateObligation(CreateObligationRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateObligation), request.CustomerOnSAId);
        var result = await _service.CreateObligationAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<int>(result.ObligationId);
    }

    public async Task<IServiceCallResult> DeleteObligation(int ObligationId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteObligation), ObligationId);
        var result = await _service.DeleteObligationAsync(
            new()
            {
                ObligationId = ObligationId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetObligation(int ObligationId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetObligation), ObligationId);
        var result = await _service.GetObligationAsync(
            new()
            {
                ObligationId = ObligationId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<Obligation>(result);
    }

    public async Task<IServiceCallResult> GetObligationList(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetObligationList), customerOnSAId);
        var result = await _service.GetObligationListAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<List<Obligation>>(result.Obligations.ToList());
    }

    public async Task<IServiceCallResult> UpdateObligation(Obligation request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateObligation), request.ObligationId);
        var result = await _service.UpdateObligationAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }
    #endregion Obligation

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