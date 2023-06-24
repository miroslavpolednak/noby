using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;
using UpdateCustomerRequest = DomainServices.CustomerService.Contracts.UpdateCustomerRequest;

namespace DomainServices.HouseholdService.Clients;

public interface ICustomerChangeDataMerger
{
    void MergeAll(CustomerDetailResponse customer, CustomerOnSA customerOnSA);
    void MergeClientData(CustomerDetailResponse customer, CustomerOnSA customerOnSA);
    void MergeClientData(UpdateCustomerRequest customer, CustomerOnSA customerOnSA);
    void MergeTaxResidence(NaturalPersonTaxResidence? taxResidence, CustomerOnSA customerOnSA);
}