using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients.Services;

internal class CustomerOnSAService : ICustomerOnSAServiceClient
{
    public async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.CreateCustomerAsync(request, cancellationToken: cancellationToken);
    }

    public async Task DeleteCustomer(int customerOnSAId, bool hardDelete = false, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.DeleteCustomerAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId,
                HardDelete = hardDelete
            }, cancellationToken: cancellationToken);
    }

    public async Task<CustomerOnSA> GetCustomer(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.GetCustomerAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken);
    }

    public async Task<List<CustomerOnSA>> GetCustomerList(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetCustomerListAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
        return result.Customers.ToList();
    }

    public async Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.UpdateCustomerAsync(request, cancellationToken: cancellationToken);
    }

    #region Income
    public async Task<int> CreateIncome(CreateIncomeRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.CreateIncomeAsync(request, cancellationToken: cancellationToken);
        return result.IncomeId;
    }

    public async Task DeleteIncome(int incomeId, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.DeleteIncomeAsync(
            new()
            {
                IncomeId = incomeId
            }, cancellationToken: cancellationToken);
    }

    public async Task<Income> GetIncome(int incomeId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.GetIncomeAsync(
            new()
            {
                IncomeId = incomeId
            }, cancellationToken: cancellationToken);
    }

    public async Task<List<IncomeInList>> GetIncomeList(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetIncomeListAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken);
        return result.Incomes.ToList();
    }

    public async Task UpdateIncome(UpdateIncomeRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.UpdateIncomeAsync(request, cancellationToken: cancellationToken);
    }

    public async Task UpdateIncomeBaseData(UpdateIncomeBaseDataRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.UpdateIncomeBaseDataAsync(request, cancellationToken: cancellationToken);
    }
    #endregion Income

    #region Obligation
    public async Task<IServiceCallResult> CreateObligation(CreateObligationRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.CreateObligationAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<int>(result.ObligationId);
    }

    public async Task<IServiceCallResult> DeleteObligation(int ObligationId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.DeleteObligationAsync(
            new()
            {
                ObligationId = ObligationId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetObligation(int ObligationId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetObligationAsync(
            new()
            {
                ObligationId = ObligationId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<Obligation>(result);
    }

    public async Task<IServiceCallResult> GetObligationList(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetObligationListAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<List<Obligation>>(result.Obligations.ToList());
    }

    public async Task<IServiceCallResult> UpdateObligation(Obligation request, CancellationToken cancellationToken = default(CancellationToken))
    {
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