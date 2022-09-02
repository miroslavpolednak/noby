using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Api.Dto;
using DomainServices.CustomerService.Api.Services.CustomerSource;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class SearchCustomersHandler : IRequestHandler<SearchCustomersMediatrRequest, SearchCustomersResponse>
    {
        private readonly CustomerSourceManager _customerSource;
        private readonly ILogger<SearchCustomersHandler> _logger;

        public SearchCustomersHandler(CustomerSourceManager customerSource, ILogger<SearchCustomersHandler> logger)
        {
            _customerSource = customerSource;
            _logger = logger;
        }

        public async Task<SearchCustomersResponse> Handle(SearchCustomersMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Search customers by request {request}", request.Request);

            var response = new SearchCustomersResponse();

            var result = await _customerSource.Search(request.Request, cancellationToken);

            response.Customers.AddRange(result);

            return response;
        }
    }
}
