using CIS.Core.Results;

namespace DomainServices.SalesArrangementService.Abstraction;

public interface ISalesArrangementServiceAbstraction
{
    /// <summary>
    /// Vytvoreni Sales Arrangement
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[int (SalesArrangementId)] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16001; OfferInstance ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16002; Case ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16005; SalesArrangementType #{} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> CreateSalesArrangement(long caseId, int salesArrangementTypeId, int? offerInstanceId = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci detail Sales Arrangement bez JSON dat
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Contracts.SalesArrangement] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> GetSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));
    
    /// <summary>
    /// Vraci detail nalinkovaneho Sales Arrangement na zaklade OfferId
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Contracts.SalesArrangement] - nalinkovani SA;
    /// EmptyServiceCallResult - neexistuje zadny nalinkovany SA pro toto OfferId 
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> GetSalesArrangementByOfferId(int offerId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Napojeni OfferInstance na SA
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16001; OfferInstance ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> LinkModelationToSalesArrangement(int salesArrangementId, int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vrati seznam SalesArrangementu pro dane CaseId - s moznosti filtrovani na stavy
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetSalesArrangementListResponse] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> GetSalesArrangementList(long caseId, IEnumerable<int>? states = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update stavu SA
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID {} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16006; SalesArrangementState #{} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16007; SalesArrangement {} is already in state {}</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement unavailable</exception>
    Task<IServiceCallResult> UpdateSalesArrangementState(int salesArrangementType, int state, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update obsahu SA
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateSalesArrangement(int salesArrangementId, string? contractNumber, CancellationToken cancellationToken = default(CancellationToken));
}
