using CIS.Core.Results;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;
using Microsoft.Extensions.Logging;

namespace DomainServices.CustomerService.Abstraction
{
    internal class CustomerService : ICustomerServiceAbstraction
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly Contracts.V1.CustomerService.CustomerServiceClient _service;

        public CustomerService(
            ILogger<CustomerService> logger,
            Contracts.V1.CustomerService.CustomerServiceClient service)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IServiceCallResult> ProfileCheck(ProfileCheckRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.ProfileCheckAsync(request, cancellationToken: cancellationToken);

            return new SuccessfulServiceCallResult<ProfileCheckResponse>(result);
        }

        public async Task<IServiceCallResult> GetCustomerDetail(Identity identity, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetCustomerDetailAsync(new CustomerDetailRequest { Identity = identity }, cancellationToken: cancellationToken);

            return new SuccessfulServiceCallResult<CustomerDetailResponse>(result);
        }

        public async Task<IServiceCallResult> GetCustomerList(IEnumerable<Identity> identities, CancellationToken cancellationToken = default)
        {
            var request = new CustomerListRequest();
            request.Identities.AddRange(identities);

            var result = await _service.GetCustomerListAsync(request, cancellationToken: cancellationToken);

            return new SuccessfulServiceCallResult<CustomerListResponse>(result);
        }

        public async Task<IServiceCallResult> SearchCustomers(SearchCustomersRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _service.SearchCustomersAsync(request, cancellationToken: cancellationToken);
            return new SuccessfulServiceCallResult<SearchCustomersResponse>(result);
        }
    }
}