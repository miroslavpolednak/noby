using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients;

public interface ICustomerChangeDataMerger
{
    /// <summary>
    /// Throw away locally stored CRS data(keep client changes)
    /// </summary>
    /// <returns>json string</returns>
    string? TrowAwayLocallyStoredCrsData(CustomerOnSA customerOnSA);

    /// <summary>
    /// Throw away locally stored Client data (keep CRS changes)
    /// </summary>
    /// <returns>json string</returns>
    string? TrowAwayLocallyStoredClientData(CustomerOnSA customerOnSA);
    void MergeAll(CustomerDetailResponse customer, CustomerOnSA customerOnSA);
    void MergeClientData(CustomerDetailResponse customer, CustomerOnSA customerOnSA);
    void MergeTaxResidence(NaturalPerson naturalPerson, CustomerOnSA customerOnSA);
}