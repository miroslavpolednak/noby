using CIS.Core.Results;
using CIS.DomainServicesSecurity.Abstraction;
using DomainServices.CustomerService.Contracts;
using Microsoft.Extensions.Logging;

namespace DomainServices.CustomerService.Abstraction
{
    internal class CustomerService : ICustomerServiceAbstraction
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly Contracts.V1.CustomerService.CustomerServiceClient _service;
        private readonly ICisUserContextHelpers _userContext;

        public CustomerService(
            ILogger<CustomerService> logger,
            Contracts.V1.CustomerService.CustomerServiceClient service,
            ICisUserContextHelpers userContext)
        {
            _userContext = userContext;
            _service = service;
            _logger = logger;
        }

        public async Task<IServiceCallResult> SearchCustomers(SearchCustomersRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _userContext.AddUserContext(async () => await _service.SearchCustomersAsync(request, cancellationToken: cancellationToken));
            return new SuccessfulServiceCallResult<SearchCustomersResponse>(result);
        }

        public async Task<IServiceCallResult> GetCustomerList(CustomerListRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _userContext.AddUserContext(async () => await _service.GetCustomerListAsync(request, cancellationToken: cancellationToken));
            return new SuccessfulServiceCallResult<CustomerListResponse>(result);
        }

        public async Task<IServiceCallResult> GetCustomerDetail(CustomerRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _userContext.AddUserContext(async () => await _service.GetCustomerDetailAsync(request, cancellationToken: cancellationToken));
            return new SuccessfulServiceCallResult<CustomerResponse>(result);
        }

        public async Task<IServiceCallResult> Create(CreateRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _userContext.AddUserContext(async () => await _service.CreateAsync(request, cancellationToken: cancellationToken));
            return new SuccessfulServiceCallResult<CreateResponse>(result);
        }

        public async Task<IServiceCallResult> CreateContact(CreateContactRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _userContext.AddUserContext(async () => await _service.CreateContactAsync(request, cancellationToken: cancellationToken));
            return new SuccessfulServiceCallResult<CreateContactResponse>(result);
        }

        public async Task<IServiceCallResult> DeleteContact(DeleteContactRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _userContext.AddUserContext(async () => await _service.DeleteContactAsync(request, cancellationToken: cancellationToken));
            return new SuccessfulServiceCallResult();
        }

        public async Task<IServiceCallResult> UpdateAdress(UpdateAdressRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _userContext.AddUserContext(async () => await _service.UpdateAdressAsync(request, cancellationToken: cancellationToken));
            return new SuccessfulServiceCallResult();
        }

        public async Task<IServiceCallResult> UpdateBasicData(UpdateBasicDataRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _userContext.AddUserContext(async () => await _service.UpdateBasicDataAsync(request, cancellationToken: cancellationToken));
            return new SuccessfulServiceCallResult();
        }
    }
}