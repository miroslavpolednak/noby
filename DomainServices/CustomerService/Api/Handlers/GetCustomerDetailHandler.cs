using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Api.Dto;
using DomainServices.CustomerService.Api.Services.CustomerSource;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetCustomerDetailHandler : IRequestHandler<GetCustomerDetailMediatrRequest, CustomerDetailResponse>
    {
        private readonly ILogger<GetCustomerDetailHandler> _logger;
        private readonly CustomerSourceManager _customerSource;

        public GetCustomerDetailHandler(CustomerSourceManager customerSource, ILogger<GetCustomerDetailHandler> logger)
        {
            _logger = logger;
            _customerSource = customerSource;
        }

        public Task<CustomerDetailResponse> Handle(GetCustomerDetailMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get detail instance identity: {identity}", request.Request.Identity);

            return _customerSource.GetDetail(request.Request.Identity, cancellationToken);
        }
    }
}
