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
    Task<IServiceCallResult> CreateSalesArrangement(long caseId, int salesArrangementType, int? offerInstanceId = null);

    /// <summary>
    /// Vraci detail Sales Arrangement bez JSON dat
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Contracts.GetSalesArrangementResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> GetSalesArrangement(int salesArrangementId);

    /// <summary>
    /// Vraci JSON data pro dany Sales Arrangement
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[GetSalesArrangementDataResponse] - OK;
    /// </returns>
    Task<IServiceCallResult> GetSalesArrangementData(int salesArrangementId);

    /// <summary>
    /// Napojeni OfferInstance na SA
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> LinkModelationToSalesArrangement(int salesArrangementId, int offerInstanceId);

    Task<IServiceCallResult> GetSalesArrangementsByCaseId(long caseId, IEnumerable<int>? states);

    /// <summary>
    /// Update stavu SA
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateSalesArrangementState(int salesArrangementType, int state);
    
    Task<IServiceCallResult> ValidateSalesArrangement(int salesArrangementId);

    /// <summary>
    /// Update obsahu SA
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    Task<IServiceCallResult> UpdateSalesArrangementData(int salesArrangementId, string data);
}
