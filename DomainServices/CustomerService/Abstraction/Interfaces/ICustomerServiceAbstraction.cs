using CIS.Core.Results;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Abstraction
{
    public interface ICustomerServiceAbstraction
    {
        Task<IServiceCallResult> SearchCustomers(SearchCustomersRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> GetCustomerList(CustomerListRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> GetCustomerDetail(CustomerRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> Create(CreateRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> CreateContact(CreateContactRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> DeleteContact(DeleteContactRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> UpdateAdress(UpdateAdressRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> UpdateBasicData(UpdateBasicDataRequest request, CancellationToken cancellationToken = default(CancellationToken));
    }
}
