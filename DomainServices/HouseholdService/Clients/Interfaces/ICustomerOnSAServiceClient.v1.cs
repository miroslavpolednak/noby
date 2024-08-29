﻿using SharedTypes.GrpcTypes;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients.v1;

public interface ICustomerOnSAServiceClient
{
    Task<List<GetCustomerChangeMetadataResponse.Types.GetCustomerChangeMetadataResponseItem>?> GetCustomerChangeMetadata(int salesArrangementId, CancellationToken cancellationToken = default);

    Task UpdateCustomerDetail(UpdateCustomerDetailRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vytvoreni noveho klienta
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16021; CustomerRoleId does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Smazani klienta
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task DeleteCustomer(int customerOnSAId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vraci instanci vybraneho klienta
    /// </summary>
    /// <returns><see cref="CustomerOnSA"/>Customer instance</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<CustomerOnSA> GetCustomer(int customerOnSAId, CancellationToken cancellationToken = default);
    Task<CustomerOnSA> GetCustomerWithoutCache(int customerOnSAId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vraci seznam instanci klientu dle Identity
    /// </summary>
    /// <returns><see cref="List{}"/> where T : <see cref="CustomerOnSA" /></returns>
    Task<List<CustomerOnSA>> GetCustomersByIdentity(Identity identity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Vraci seznam instanci klientu dle SA Id
    /// </summary>
    /// <returns><see cref="List{}"/> where T : <see cref="CustomerOnSA" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<List<CustomerOnSA>> GetCustomerList(int salesArrangementId, CancellationToken cancellationToken = default);
    Task<List<CustomerOnSA>> GetCustomerListWithoutCache(int salesArrangementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update dat o customerovi
    /// </summary>
    /// <returns><see cref="UpdateCustomerResponse"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16020; CustomerOnSA ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update dat o customerovi s puvodni instance daneho customera - pro potreby rollbacku
    /// </summary>
    Task<UpdateCustomerResponse> UpdateCustomer(CustomerOnSA originalCustomer, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vytvoreni noveho prijmu
    /// </summary>
    /// <returns><see cref="int{}"/>IncomeId</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16024; CustomerOnSAId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16028; IncomeTypeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<int> CreateIncome(CreateIncomeRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Smazani prijmu
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16029; Income ID {IncomeId} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task DeleteIncome(int incomeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vraci instanci pozadovaneho prijmu
    /// </summary>
    /// <returns><see cref="Income"/>Income instance</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16029; Income ID {request.IncomeId} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<Income> GetIncome(int incomeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vraci vsechny prijmy pro daneho Customera
    /// </summary>
    /// <returns><see cref="List{}"/> where T : <see cref="IncomeInList" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<List<IncomeInList>> GetIncomeList(int customerOnSAId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update detailu dat o prijmu
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16055; IncomeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task UpdateIncome(Income request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vytvoreni noveho zavazku
    /// </summary>
    /// <returns><see cref="int"/>ObligationId</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16024; CustomerOnSAId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16028; ObligationTypeId must be > 0</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<int> CreateObligation(CreateObligationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Smazani zavazku
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16042; Obligation ID {ObligationId} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task DeleteObligation(int ObligationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vraci instanci pozadovaneho zavazku
    /// </summary>
    /// <returns><see cref="Obligation"/>Obligation instance</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16042; Obligation ID {ObligationId} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<Obligation> GetObligation(int ObligationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vraci vsechny zavazky pro daneho Customera
    /// </summary>
    /// <returns><see cref="List{}"/>where T : <see cref="ObligationInList" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<List<Obligation>> GetObligationList(int customerOnSAId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update detailu dat o zavazku
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16042; Obligation ID {ObligationId} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task UpdateObligation(Obligation request, CancellationToken cancellationToken = default);

    Task<ValidateCustomerOnSAIdResponse> ValidateCustomerOnSAId(int customerOnSAId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default);
    Task<ValidateCustomerOnSAIdResponse> ValidateCustomerOnSAIdWithoutCache(int customerOnSAId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default);
}