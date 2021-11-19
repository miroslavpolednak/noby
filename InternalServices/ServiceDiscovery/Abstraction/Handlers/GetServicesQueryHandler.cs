using Microsoft.Extensions.Logging;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction.Handlers
{
    internal class GetServicesQueryHandler : IRequestHandler<Dto.GetServicesRequest, List<Contracts.DiscoverableService>>
    {
        private readonly ILogger<GetServiceQueryHandler> _logger;
        private readonly Contracts.v1.DiscoveryService.DiscoveryServiceClient _service;
        private readonly Security.InternalServices.ICisUserContextHelpers _userContext;

        public GetServicesQueryHandler(
            Contracts.v1.DiscoveryService.DiscoveryServiceClient service, 
            ILogger<GetServiceQueryHandler> logger, 
            Security.InternalServices.ICisUserContextHelpers userContext)
        {
            _userContext = userContext;
            _logger = logger;
            _service = service;
        }

        public async Task<List<Contracts.DiscoverableService>> Handle(Dto.GetServicesRequest request, CancellationToken cancellation)
        {
            _logger.LogDebug("Get list from {environment}", request.EnvironmentName.Name);

            var model = new Contracts.GetServicesRequest
            {
                Environment = request.EnvironmentName
            };

            var result = await _userContext.AddUserContext(async () => (await _service.GetServicesAsync(model)));

            return result.Services.ToList();
        }
    }
}
