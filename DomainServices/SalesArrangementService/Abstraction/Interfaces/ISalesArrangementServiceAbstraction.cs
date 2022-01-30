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
    Task<IServiceCallResult> CreateSalesArrangement(long caseId, int salesArrangementTypeId, int? offerInstanceId = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci detail Sales Arrangement bez JSON dat
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Contracts.SalesArrangement] - OK;
    /// </returns>
    Task<IServiceCallResult> GetSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci JSON data pro dany Sales Arrangement
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetSalesArrangementDataResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> GetSalesArrangementData(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Napojeni OfferInstance na SA
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> LinkModelationToSalesArrangement(int salesArrangementId, int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken));

    Task<IServiceCallResult> GetSalesArrangementsByCaseId(long caseId, IEnumerable<int>? states, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update stavu SA
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateSalesArrangementState(int salesArrangementType, int state, CancellationToken cancellationToken = default(CancellationToken));
    
    Task<IServiceCallResult> ValidateSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update obsahu SA
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateSalesArrangementData(int salesArrangementId, string data, CancellationToken cancellationToken = default(CancellationToken));
}
