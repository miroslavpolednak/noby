using System.ComponentModel;
using DomainServices.CustomerService.Api.Dto;
using DomainServices.CustomerService.Api.Services.CustomerSource.CustomerManagement;
using DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetCustomerDetailHandler : IRequestHandler<GetCustomerDetailMediatrRequest, CustomerDetailResponse>
    {
        private readonly ILogger<GetCustomerDetailHandler> _logger;
        private readonly CustomerManagementDetailProvider _cmDetailProvider;
        private readonly KonsDbDetailProvider _konsDbDetailProvider;

        public GetCustomerDetailHandler(CustomerManagementDetailProvider cmDetailProvider, KonsDbDetailProvider konsDbDetailProvider, ILogger<GetCustomerDetailHandler> logger)
        {
            _logger = logger;
            _cmDetailProvider = cmDetailProvider;
            _konsDbDetailProvider = konsDbDetailProvider;
        }

        public Task<CustomerDetailResponse> Handle(GetCustomerDetailMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get detail instance identity: {identity}", request.Identity);

            return request.Identity.IdentityScheme switch
            {
                Identity.Types.IdentitySchemes.Kb => _cmDetailProvider.GetDetail(request.Identity.IdentityId, cancellationToken),
                Identity.Types.IdentitySchemes.Mp => _konsDbDetailProvider.GetDetail(request.Identity.IdentityId, cancellationToken),
                _ => throw new InvalidEnumArgumentException()
            };
        }
    }
}
