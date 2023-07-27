using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients;

public interface ICustomerChangeDataMerger
{
    void MergeAll(CustomerDetailResponse customer, CustomerOnSA customerOnSA);
    void MergeClientData(CustomerDetailResponse customer, CustomerOnSA customerOnSA);
    void MergeTaxResidence(NaturalPerson naturalPerson, CustomerOnSA customerOnSA);
}