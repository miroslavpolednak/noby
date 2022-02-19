using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Contracts;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;

namespace DomainServices.CustomerService.Abstraction
{
    internal class CustomerService : ICustomerServiceAbstraction
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly Contracts.V1.CustomerService.CustomerServiceClient _service;
        private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;

        public CustomerService(
            ILogger<CustomerService> logger,
            Contracts.V1.CustomerService.CustomerServiceClient service,
            CIS.Security.InternalServices.ICisUserContextHelpers userContext)
        {
            _userContext = userContext;
            _service = service;
            _logger = logger;
        }

        public async Task<IServiceCallResult> SearchCustomers(SearchCustomersRequest request)
        {
            var result = await _userContext.AddUserContext(async () => await _service.SearchCustomersAsync(request));
            return new SuccessfulServiceCallResult<SearchCustomersResponse>(result);
        }

        public async Task<IServiceCallResult> GetCustomerList(CustomerListRequest request)
        {
            var result = await _userContext.AddUserContext(async () => await _service.GetCustomerListAsync(request));
            return new SuccessfulServiceCallResult<CustomerListResponse>(result);
        }

        public async Task<IServiceCallResult> GetCustomerDetail(CustomerRequest request)
        {
            var result = await _userContext.AddUserContext(async () => await _service.GetCustomerDetailAsync(request));
            return new SuccessfulServiceCallResult<CustomerResponse>(result);
        }

        public async Task<IServiceCallResult> Create(CreateRequest request)
        {
            var result = await _userContext.AddUserContext(async () => await _service.CreateAsync(request));
            return new SuccessfulServiceCallResult<CreateResponse>(result);
        }

        public async Task<IServiceCallResult> CreateContact(CreateContactRequest request)
        {
            var result = await _userContext.AddUserContext(async () => await _service.CreateContactAsync(request));
            return new SuccessfulServiceCallResult<CreateContactResponse>(result);
        }

        public async Task<IServiceCallResult> DeleteContact(DeleteContactRequest request)
        {
            await _userContext.AddUserContext(async () => await _service.DeleteContactAsync(request));
            return new SuccessfulServiceCallResult();
        }

        public async Task<IServiceCallResult> UpdateAdress(UpdateAdressRequest request)
        {
            await _userContext.AddUserContext(async () => await _service.UpdateAdressAsync(request));
            return new SuccessfulServiceCallResult();
        }

        public async Task<IServiceCallResult> UpdateBasicData(UpdateBasicDataRequest request)
        {
            await _userContext.AddUserContext(async () => await _service.UpdateBasicDataAsync(request));
            return new SuccessfulServiceCallResult();
        }
    }
}