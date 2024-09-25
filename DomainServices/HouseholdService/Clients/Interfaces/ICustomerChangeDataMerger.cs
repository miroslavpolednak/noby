using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients;

public interface ICustomerChangeDataMerger
{
    void MergeAll(DomainServices.CustomerService.Contracts.Customer customer, CustomerOnSA customerOnSA);
    void MergeClientData(DomainServices.CustomerService.Contracts.Customer customer, CustomerOnSA customerOnSA);
    void MergeTaxResidence(DomainServices.CustomerService.Contracts.NaturalPerson naturalPerson, CustomerOnSA customerOnSA);
}