using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Clients;

public interface ICustomerChangeDataMerger
{
    void Merge(CustomerDetailResponse customer, CustomerOnSA customerOnSA);
}