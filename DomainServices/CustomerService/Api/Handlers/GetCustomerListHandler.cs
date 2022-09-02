using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Api.Dto;
using DomainServices.CustomerService.Api.Services.CustomerSource;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetCustomerListHandler : IRequestHandler<GetCustomerListMediatrRequest, CustomerListResponse>
    {
        private readonly ILogger<GetCustomerListHandler> _logger;
        private readonly CustomerSourceManager _customerSource;

        public GetCustomerListHandler(CustomerSourceManager customerSource, ILogger<GetCustomerListHandler> logger)
        {
            _logger = logger;
            _customerSource = customerSource;
        }

        public async Task<CustomerListResponse> Handle(GetCustomerListMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get list instance Identities #{id}", string.Join(",", request.Request.Identities));

            var customers = await _customerSource.GetList(request.Request.Identities, cancellationToken);

            var response = new CustomerListResponse();

            response.Customers.AddRange(customers);

            return response;
        }
    }
}
