using CIS.Core.Results;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Abstraction
{
    public interface ICustomerServiceAbstraction
    {
        /// <summary>
        /// Vyhledani customeru
        /// </summary>
        /// <returns><see cref="SuccessfulServiceCallResult{}"/> of <see cref="SearchCustomersResponse"/> - OK;</returns>
        /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 17000; Validation problem</exception>
        /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 17001; Customer Management error</exception>
        /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
        /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
        Task<IServiceCallResult> SearchCustomers(SearchCustomersRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Vyhledani customeru podle identities
        /// </summary>
        /// <returns><see cref="SuccessfulServiceCallResult{}"/> of <see cref="CustomerListResponse"/> - OK;</returns>
        /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 17000; Validation problem</exception>
        /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 17001; Customer Management error</exception>
        /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
        /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
        Task<IServiceCallResult> GetCustomerList(CustomerListRequest request, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Detail customera podle identity
        /// </summary>
        /// <returns><see cref="SuccessfulServiceCallResult{}"/> of <see cref="CustomerResponse"/> - OK;</returns>
        /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 17000; Validation problem</exception>
        /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 17001; Customer Management error</exception>
        /// <exception cref="CIS.Core.Exceptions.CisNotFoundException">Code: 17002; Customer not found: {}</exception>
        /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">CaseService unavailable</exception>
        /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Some of underlying services are not available or failed to call</exception>
        Task<IServiceCallResult> GetCustomerDetail(CustomerRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> Create(CreateRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> CreateContact(CreateContactRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> DeleteContact(DeleteContactRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> UpdateAdress(UpdateAdressRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<IServiceCallResult> UpdateBasicData(UpdateBasicDataRequest request, CancellationToken cancellationToken = default(CancellationToken));
    }
}
