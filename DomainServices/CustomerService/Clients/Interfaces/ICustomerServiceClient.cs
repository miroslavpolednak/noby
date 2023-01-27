using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Clients;

public interface ICustomerServiceClient
{
    /// <summary>
    /// Kontrola zda klient v KB CM splňuje plně identifikovaný profil.
    /// </summary>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="CisNotFoundException">Requested customer was not found.</exception>
    /// <exception cref="Grpc.Core.RpcException">CustomerManagement call ended in an internal error (500).</exception>
    /// <exception cref="CisServiceUnavailableException">CustomerService or some of underlying services are not available or failed to call.</exception>
    Task<ProfileCheckResponse> ProfileCheck(ProfileCheckRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vytvoreni klienta v CM nebo MP (podle schematu)
    /// </summary>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="CisAlreadyExistsException">KonsDB - 11017 Partner already exists in KonsDB.</exception>
    /// <exception cref="Grpc.Core.RpcException">Customer already exists in KB CM or state registry is unavailable, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="Grpc.Core.RpcException">CustomerManagement call ended in an internal error (500).</exception>
    /// <exception cref="CisServiceUnavailableException">CustomerService or some of underlying services are not available or failed to call.</exception>
    Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uprava klienta v CM nebo MP (podle schematu)
    /// </summary>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Detail customera podle identity
    /// </summary>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="CisNotFoundException">Requested customer was not found.</exception>
    /// <exception cref="Grpc.Core.RpcException">CustomerManagement call ended in an internal error (500).</exception>
    /// <exception cref="CisServiceUnavailableException">CustomerService or some of underlying services are not available or failed to call.</exception>
    Task<CustomerDetailResponse> GetCustomerDetail(Identity identity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vyhledaní customeru podle identities
    /// </summary>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="CisNotFoundException">Requested customer was not found.</exception>
    /// <exception cref="Grpc.Core.RpcException">CustomerManagement call ended in an internal error (500).</exception>
    /// <exception cref="CisServiceUnavailableException">CustomerService or some of underlying services are not available or failed to call.</exception>
    Task<CustomerListResponse> GetCustomerList(IEnumerable<Identity> identities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vyhledaní customeru
    /// </summary>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="CisNotFoundException">Requested customer was not found.</exception>
    /// <exception cref="Grpc.Core.RpcException">CustomerManagement call ended in an internal error (500).</exception>
    /// <exception cref="CisServiceUnavailableException">CustomerService or some of underlying services are not available or failed to call.</exception>
    Task<SearchCustomersResponse> SearchCustomers(SearchCustomersRequest request, CancellationToken cancellationToken = default);
}