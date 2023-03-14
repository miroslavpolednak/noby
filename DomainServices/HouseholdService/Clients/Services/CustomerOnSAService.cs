using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients.Services;

internal sealed class CustomerOnSAService
    : ICustomerOnSAServiceClient
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

    public async Task UpdateCustomerDetail(UpdateCustomerDetailRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.UpdateCustomerDetailAsync(request, cancellationToken: cancellationToken);
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

    public async Task<int> CreateObligation(CreateObligationRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.CreateObligationAsync(request, cancellationToken: cancellationToken);
        return result.ObligationId;
    }

    public async Task DeleteObligation(int ObligationId, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.DeleteObligationAsync(
            new()
            {
                ObligationId = ObligationId
            }, cancellationToken: cancellationToken);
    }

    public async Task<Obligation> GetObligation(int ObligationId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.GetObligationAsync(
            new()
            {
                ObligationId = ObligationId
            }, cancellationToken: cancellationToken);
    }

    public async Task<List<Obligation>> GetObligationList(int customerOnSAId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetObligationListAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken);
        return result.Obligations.ToList();
    }

    public async Task UpdateObligation(Obligation request, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.UpdateObligationAsync(request, cancellationToken: cancellationToken);
    }

    #endregion Obligation

    private readonly Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient _service;

    public CustomerOnSAService(Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient service) => _service = service;
}