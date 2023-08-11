﻿using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients.Services;

internal sealed class CustomerOnSAService
    : ICustomerOnSAServiceClient
{
    public async Task<List<GetCustomerChangeMetadataResponse.Types.GetCustomerChangeMetadataResponseItem>?> GetCustomerChangeMetadata(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCustomerChangeMetadataAsync(new GetCustomerChangeMetadataRequest
        {
            SalesArrangementId = salesArrangementId
        }, cancellationToken: cancellationToken);
        return result.CustomersOnSAMetadata?.ToList();
    }

    public async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.CreateCustomerAsync(request, cancellationToken: cancellationToken);
    }

    public async Task DeleteCustomer(int customerOnSAId, bool hardDelete = false, CancellationToken cancellationToken = default)
    {
        await _service.DeleteCustomerAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId,
                HardDelete = hardDelete
            }, cancellationToken: cancellationToken);
    }

    public async Task<CustomerOnSA> GetCustomer(int customerOnSAId, CancellationToken cancellationToken = default)
    {
        if (_cacheGetCustomer is null)
        {
            _cacheGetCustomer = await _service.GetCustomerAsync(
                new()
                {
                    CustomerOnSAId = customerOnSAId
                }, cancellationToken: cancellationToken);
        }
        return _cacheGetCustomer;
    }

    public async Task<List<CustomerOnSA>> GetCustomersByIdentity(Identity identity, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCustomersByIdentityAsync(
            new()
            {
                CustomerIdentifier = identity
            }, cancellationToken: cancellationToken);
        return result.Customers.ToList();
    }

    public async Task<List<CustomerOnSA>> GetCustomerList(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCustomerListAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
        return result.Customers.ToList();
    }

    public async Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.UpdateCustomerAsync(request, cancellationToken: cancellationToken);
    }

    public async Task UpdateCustomerDetail(UpdateCustomerDetailRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateCustomerDetailAsync(request, cancellationToken: cancellationToken);
    }

    #region Income

    public async Task<int> CreateIncome(CreateIncomeRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateIncomeAsync(request, cancellationToken: cancellationToken);
        return result.IncomeId;
    }

    public async Task DeleteIncome(int incomeId, CancellationToken cancellationToken = default)
    {
        await _service.DeleteIncomeAsync(
            new()
            {
                IncomeId = incomeId
            }, cancellationToken: cancellationToken);
    }

    public async Task<Income> GetIncome(int incomeId, CancellationToken cancellationToken = default)
    {
        return await _service.GetIncomeAsync(
            new()
            {
                IncomeId = incomeId
            }, cancellationToken: cancellationToken);
    }

    public async Task<List<IncomeInList>> GetIncomeList(int customerOnSAId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetIncomeListAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken);
        return result.Incomes.ToList();
    }

    public async Task UpdateIncome(UpdateIncomeRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateIncomeAsync(request, cancellationToken: cancellationToken);
    }

    public async Task UpdateIncomeBaseData(UpdateIncomeBaseDataRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateIncomeBaseDataAsync(request, cancellationToken: cancellationToken);
    }

    #endregion Income

    #region Obligation

    public async Task<int> CreateObligation(CreateObligationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateObligationAsync(request, cancellationToken: cancellationToken);
        return result.ObligationId;
    }

    public async Task DeleteObligation(int ObligationId, CancellationToken cancellationToken = default)
    {
        await _service.DeleteObligationAsync(
            new()
            {
                ObligationId = ObligationId
            }, cancellationToken: cancellationToken);
    }

    public async Task<Obligation> GetObligation(int ObligationId, CancellationToken cancellationToken = default)
    {
        return await _service.GetObligationAsync(
            new()
            {
                ObligationId = ObligationId
            }, cancellationToken: cancellationToken);
    }

    public async Task<List<Obligation>> GetObligationList(int customerOnSAId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetObligationListAsync(
            new()
            {
                CustomerOnSAId = customerOnSAId
            }, cancellationToken: cancellationToken);
        return result.Obligations.ToList();
    }

    public async Task UpdateObligation(Obligation request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateObligationAsync(request, cancellationToken: cancellationToken);
    }

    #endregion Obligation

    public async Task<ValidateCustomerOnSAIdResponse> ValidateCustomerOnSAId(int customerOnSAId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default)
    {
        if (_cacheValidateCustomerOnSAId is null)
        {
            _cacheValidateCustomerOnSAId = await _service.ValidateCustomerOnSAIdAsync(new ValidateCustomerOnSAIdRequest
            {
                CustomerOnSAId = customerOnSAId,
                ThrowExceptionIfNotFound = throwExceptionIfNotFound
            }, cancellationToken: cancellationToken);
        }
        return _cacheValidateCustomerOnSAId;
    }

    private CustomerOnSA? _cacheGetCustomer;
    private ValidateCustomerOnSAIdResponse? _cacheValidateCustomerOnSAId;

    private readonly Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient _service;

    public CustomerOnSAService(Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient service) => _service = service;
}