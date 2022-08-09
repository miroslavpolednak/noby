using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Abstraction;

public interface ISalesArrangementServiceAbstraction
{
    /// <summary>
    /// Vytvoreni Sales Arrangement
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="int" /> (SalesArrangementId)</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16001; OfferInstance ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16002; Case ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16005; SalesArrangementType #{} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> CreateSalesArrangement(long caseId, int salesArrangementTypeId, int? offerInstanceId = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci detail Sales Arrangement bez JSON dat
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="SalesArrangement" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> GetSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci detail nalinkovaneho Sales Arrangement na zaklade OfferId
    /// </summary>
    /// <returns>
    /// <see cref="SuccessfulServiceCallResult{}"/> of type <see cref="GetSalesArrangementListResponse" />
    /// <see cref="EmptyServiceCallResult"/> - neexistuje zadny nalinkovany SA pro toto OfferId 
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> GetSalesArrangementByOfferId(int offerId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Napojeni OfferInstance na SA
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16001; OfferInstance ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> LinkModelationToSalesArrangement(int salesArrangementId, int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vrati seznam SalesArrangementu pro dane CaseId - s moznosti filtrovani na stavy
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="GetSalesArrangementListResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> GetSalesArrangementList(long caseId, IEnumerable<int>? states = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update stavu SA
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16006; SalesArrangementState #{} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16007; SalesArrangement {} is already in state {}</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> UpdateSalesArrangementState(int salesArrangementType, int state, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update obsahu SA
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    Task<IServiceCallResult> UpdateSalesArrangement(int salesArrangementId, string? contractNumber, string? riskBusinessCaseId, DateTime? firstSignedDate, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update parametru SA
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    Task<IServiceCallResult> UpdateSalesArrangementParameters(Contracts.UpdateSalesArrangementParametersRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Předání žádosti ke zpracování.
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Google.Protobuf.WellKnownTypes.Empty] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> SendToCmp(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Validace žádosti (CheckForm).
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="ValidateSalesArrangementResponse" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement unavailable</exception>
    /// <exception cref="System.Exception">Unexpected error during validation process</exception>
    Task<IServiceCallResult> ValidateSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update LAID
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    Task<IServiceCallResult> UpdateLoanAssessmentParameters(int salesArrangementId, string? loanApplicationAssessmentId, string? riskSegment, string? commandId, CancellationToken cancellationToken = default(CancellationToken));
}

