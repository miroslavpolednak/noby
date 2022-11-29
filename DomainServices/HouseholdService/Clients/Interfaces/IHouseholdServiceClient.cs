using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients;

public interface IHouseholdServiceClient
{
    /// <summary>
    /// Vytvoreni nove domacnosti
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="int" /> (HouseholdId)</returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 16023; HouseholdTypeId does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 16000; Sales arrangement ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> CreateHousehold(CreateHouseholdRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Smazani domacnosti
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 16022; Household ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> DeleteHousehold(int householdId, bool hardDelete = false, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci instanci pozadovane domacnosti
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Household" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 16022; Household ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> GetHousehold(int householdId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Vraci vsechny domacnosti pro dany Sales Arrangement
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="List{}" /> where T : <see cref="Household" /></returns>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> GetHouseholdList(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Update dat o domacnosti
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 16022; Household ID does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> UpdateHousehold(UpdateHouseholdRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Nalinkovani customeru na Household
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult"/></returns>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 16020; CustomerOnSA ID {CustomerOnSAId} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 16022; Household ID {HouseholdId} does not exist.</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">SalesArrangement service unavailable</exception>
    Task<IServiceCallResult> LinkCustomerOnSAToHousehold(int householdId, int? customerOnSAId1, int? customerOnSAId2, CancellationToken cancellationToken = default(CancellationToken));
}