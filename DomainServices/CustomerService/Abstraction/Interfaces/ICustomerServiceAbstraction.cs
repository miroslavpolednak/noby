using CIS.Core.Exceptions;
using CIS.Core.Results;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Abstraction;

public interface ICustomerServiceAbstraction
{
    /// <summary>
    /// Kontrola zda klient v KB CM splňuje plně identifikovaný profil.
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{TModel}"/> of <see cref="ProfileCheckResponse"/> - OK;</returns>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="CisNotFoundException">Requested customer was not found.</exception>
    /// <exception cref="Grpc.Core.RpcException">CustomerManagement call ended in an internal error (500).</exception>
    /// <exception cref="CisServiceUnavailableException">CustomerService or some of underlying services are not available or failed to call.</exception>
    Task<IServiceCallResult> ProfileCheck(ProfileCheckRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vytvoreni klienta v CM nebo MP (podle schematu)
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{TModel}"/> of <see cref="CreateCustomerResponse"/> - OK;</returns>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="CisAlreadyExistsException">KonsDB - 11017 Partner already exists in KonsDB.</exception>
    /// <exception cref="Grpc.Core.RpcException">Customer already exists in KB CM or state registry is unavailable, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="Grpc.Core.RpcException">CustomerManagement call ended in an internal error (500).</exception>
    /// <exception cref="CisServiceUnavailableException">CustomerService or some of underlying services are not available or failed to call.</exception>
    Task<IServiceCallResult> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Detail customera podle identity
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{TModel}"/> of <see cref="CustomerDetailResponse"/> - OK;</returns>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="CisNotFoundException">Requested customer was not found.</exception>
    /// <exception cref="Grpc.Core.RpcException">CustomerManagement call ended in an internal error (500).</exception>
    /// <exception cref="CisServiceUnavailableException">CustomerService or some of underlying services are not available or failed to call.</exception>
    Task<IServiceCallResult> GetCustomerDetail(Identity identity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vyhledaní customeru podle identities
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{TModel}"/> of <see cref="CustomerListResponse"/> - OK;</returns>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="CisNotFoundException">Requested customer was not found.</exception>
    /// <exception cref="Grpc.Core.RpcException">CustomerManagement call ended in an internal error (500).</exception>
    /// <exception cref="CisServiceUnavailableException">CustomerService or some of underlying services are not available or failed to call.</exception>
    Task<IServiceCallResult> GetCustomerList(IEnumerable<Identity> identities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vyhledaní customeru
    /// </summary>
    /// <returns><see cref="SuccessfulServiceCallResult{TModel}"/> of <see cref="SearchCustomersResponse"/> - OK;</returns>
    /// <exception cref="CisArgumentException">Validations error, see more <see href="https://wiki.kb.cz/display/HT/CustomerService+errors">here</see></exception>
    /// <exception cref="CisNotFoundException">Requested customer was not found.</exception>
    /// <exception cref="Grpc.Core.RpcException">CustomerManagement call ended in an internal error (500).</exception>
    /// <exception cref="CisServiceUnavailableException">CustomerService or some of underlying services are not available or failed to call.</exception>
    Task<IServiceCallResult> SearchCustomers(SearchCustomersRequest request, CancellationToken cancellationToken = default);
}