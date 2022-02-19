using CIS.Core.Results;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Abstraction
{
    public interface ICustomerServiceAbstraction
    {
        Task<IServiceCallResult> SearchCustomers(SearchCustomersRequest request);

        Task<IServiceCallResult> GetCustomerList(CustomerListRequest request);

        Task<IServiceCallResult> GetCustomerDetail(CustomerRequest request);

        Task<IServiceCallResult> Create(CreateRequest request);

        Task<IServiceCallResult> CreateContact(CreateContactRequest request);

        Task<IServiceCallResult> DeleteContact(DeleteContactRequest request);

        Task<IServiceCallResult> UpdateAdress(UpdateAdressRequest request);

        Task<IServiceCallResult> UpdateBasicData(UpdateBasicDataRequest request);
    }
}
