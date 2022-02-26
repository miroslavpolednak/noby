using CIS.Core.Results;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Abstraction;

public interface IHouseholdServiceAbstraction
{
    /// <summary>
    /// Vytvoreni nove domacnosti
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[int (HouseholdId)] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16023; HouseholdTypeId does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> CreateHousehold(CreateHouseholdRequest request, CancellationToken cancellationToken = default(CancellationToken));
    
    /// <summary>
    /// Smazani domacnosti
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16022; Household ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> DeleteHousehold(int householdId, CancellationToken cancellationToken = default(CancellationToken));
    
    /// <summary>
    /// Vraci instanci pozadovane domacnosti
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[Household] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16022; Household ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> GetHousehold(int householdId, CancellationToken cancellationToken = default(CancellationToken));
    
    /// <summary>
    /// Vraci vsechny domacnosti pro dany Sales Arrangement
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult[List[Household]] - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> GetHouseholdList(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));
    
    /// <summary>
    /// Update dat o domacnosti
    /// </summary>
    /// <returns>
    /// SuccessfulServiceCallResult - OK;
    /// </returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16022; Household ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.ServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> UpdateHousehold(UpdateHouseholdRequest request, CancellationToken cancellationToken = default(CancellationToken));
}