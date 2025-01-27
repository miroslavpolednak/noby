﻿using DomainServices.SalesArrangementService.Contracts;
using SharedTypes.Enums;

namespace DomainServices.SalesArrangementService.Clients;

public interface ISalesArrangementServiceClient
{
    /// <summary>
    /// Vytvoreni Sales Arrangement
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16001; OfferInstance ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16002; Case ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16005; SalesArrangementType #{} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<int> CreateSalesArrangement(long caseId, int salesArrangementTypeId, int? offerInstanceId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vytvoreni Sales Arrangement
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16001; OfferInstance ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16002; Case ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16005; SalesArrangementType #{} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<int> CreateSalesArrangement(CreateSalesArrangementRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vraci detail Sales Arrangement bez JSON dat
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<SalesArrangement> GetSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default);

    Task<List<GetProductSalesArrangementsResponse.Types.SalesArrangement>> GetProductSalesArrangements(long caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vraci detail nalinkovaneho Sales Arrangement na zaklade OfferId
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<SalesArrangement?> GetSalesArrangementByOfferId(int offerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Napojeni OfferInstance na SA
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16001; OfferInstance ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task LinkModelationToSalesArrangement(int salesArrangementId, int offerInstanceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vrati seznam SalesArrangementu pro dane CaseId - s moznosti filtrovani na stavy
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<GetSalesArrangementListResponse> GetSalesArrangementList(long caseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update stavu SA
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16006; SalesArrangementState #{} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16007; SalesArrangement {} is already in state {}</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task UpdateSalesArrangementState(int salesArrangementId, EnumSalesArrangementStates state, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update obsahu SA
    /// </summary>
    Task UpdateSalesArrangement(UpdateSalesArrangementRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update parametru SA
    /// </summary>
    Task UpdateSalesArrangementParameters(Contracts.UpdateSalesArrangementParametersRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Předání žádosti ke zpracování.
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task SendToCmp(int salesArrangementId, bool isCancelled, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validace žádosti (CheckForm).
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    /// <exception cref="System.Exception">Unexpected error during validation process</exception>
    Task<ValidateSalesArrangementResponse> ValidateSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update LAID
    /// </summary>
    Task UpdateLoanAssessmentParameters(UpdateLoanAssessmentParametersRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Smazani zadosti
    /// </summary>
    Task DeleteSalesArrangement(int salesArrangementId, bool hardDelete = false, CancellationToken cancellationToken = default);

    Task<List<FlowSwitch>> GetFlowSwitches(int salesArrangementId, CancellationToken cancellationToken = default);

    Task SetFlowSwitches(int salesArrangementId, List<EditableFlowSwitch> flowSwitches, CancellationToken cancellationToken = default);

    Task SetFlowSwitch(int salesArrangementId, FlowSwitches flowSwitch, bool value, CancellationToken cancellationToken = default);

    Task UpdatePcpId(int salesArrangementId, string pcpId, CancellationToken cancellationToken = default);

    Task<SetContractNumberResponse> SetContractNumber(int salesArrangementId, int customerOnSaId, CancellationToken cancellationToken = default);

    Task<ValidateSalesArrangementIdResponse> ValidateSalesArrangementIdWithoutCache(int salesArrangementId, bool throwExceptionIfNotFound, CancellationToken cancellationToken = default);

    Task<ValidateSalesArrangementIdResponse> ValidateSalesArrangementId(int salesArrangementId, bool throwExceptionIfNotFound, CancellationToken cancellationToken = default);

    void ClearSalesArrangementCache();
}

