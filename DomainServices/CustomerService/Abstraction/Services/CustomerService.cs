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
        private readonly Contracts.CustomerService.CustomerServiceClient _service;
        private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;

        public CustomerService(
            ILogger<CustomerService> logger,
            Contracts.CustomerService.CustomerServiceClient service,
            CIS.Security.InternalServices.ICisUserContextHelpers userContext)
        {
            _userContext = userContext;
            _service = service;
            _logger = logger;
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

        public async Task<IServiceCallResult> GetBasicDataByFullIdentification(GetBasicDataByFullIdentificationRequest request)
        {
            var result = await _userContext.AddUserContext(async () => await _service.GetBasicDataByFullIdentificationAsync(request));
            return new SuccessfulServiceCallResult<GetBasicDataResponse>(result);
        }

        public async Task<IServiceCallResult> GetBasicDataByIdentifier(GetBasicDataByIdentifierRequest request)
        {
            var result = await _userContext.AddUserContext(async () => await _service.GetBasicDataByIdentifierAsync(request));
            return new SuccessfulServiceCallResult<GetBasicDataResponse>(result);
        }

        public async Task<IServiceCallResult> GetDetail(GetDetailRequest request)
        {
            var result = await _userContext.AddUserContext(async () => await _service.GetDetailAsync(request));
            return new SuccessfulServiceCallResult<GetDetailResponse>(result);
        }

        public async Task<IServiceCallResult> GetList(GetListRequest request)
        {
            var result = await _userContext.AddUserContext(async () => await _service.GetListAsync(request));
            return new SuccessfulServiceCallResult<GetListResponse>(result);
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