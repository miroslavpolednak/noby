using DomainServices.SalesArrangementService.Contracts;

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
    Task<int> CreateSalesArrangement(long caseId, int salesArrangementTypeId, int? offerInstanceId = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vytvoreni Sales Arrangement
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16001; OfferInstance ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16002; Case ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16005; SalesArrangementType #{} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<int> CreateSalesArrangement(CreateSalesArrangementRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci detail Sales Arrangement bez JSON dat
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<SalesArrangement> GetSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    Task<(int SalesArrangementId, int? OfferId)> GetProductSalesArrangement(long caseId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci detail nalinkovaneho Sales Arrangement na zaklade OfferId
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<SalesArrangement?> GetSalesArrangementByOfferId(int offerId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Napojeni OfferInstance na SA
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16001; OfferInstance ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task LinkModelationToSalesArrangement(int salesArrangementId, int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vrati seznam SalesArrangementu pro dane CaseId - s moznosti filtrovani na stavy
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<GetSalesArrangementListResponse> GetSalesArrangementList(long caseId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update stavu SA
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16006; SalesArrangementState #{} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16007; SalesArrangement {} is already in state {}</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task UpdateSalesArrangementState(int salesArrangementId, int state, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update obsahu SA
    /// </summary>
    Task UpdateSalesArrangement(int salesArrangementId, string? contractNumber, string? riskBusinessCaseId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update parametru SA
    /// </summary>
    Task UpdateSalesArrangementParameters(Contracts.UpdateSalesArrangementParametersRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Předání žádosti ke zpracování.
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task SendToCmp(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Validace žádosti (CheckForm).
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    /// <exception cref="System.Exception">Unexpected error during validation process</exception>
    Task<ValidateSalesArrangementResponse> ValidateSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update LAID
    /// </summary>
    Task UpdateLoanAssessmentParameters(int salesArrangementId, string? loanApplicationAssessmentId, string? riskSegment, string? commandId, DateTime? riskBusinessCaseExpirationDate, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Smazani zadosti
    /// </summary>
    Task DeleteSalesArrangement(int salesArrangementId, bool hardDelete = false, CancellationToken cancellationToken = default(CancellationToken));

    Task UpdateOfferDocumentId(int salesArrangementId, string offerDocumentId, CancellationToken cancellationToken = default(CancellationToken));

    Task<List<FlowSwitch>> GetFlowSwitches(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    Task SetFlowSwitches(int salesArrangementId, List<EditableFlowSwitch> flowSwitches, CancellationToken cancellationToken = default(CancellationToken));

    Task<SetContractNumberResponse> SetContractNumber(int salesArrangementId, int customerOnSaId, CancellationToken cancellationToken = default);
}

