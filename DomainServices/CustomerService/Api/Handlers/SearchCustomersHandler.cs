using System.ComponentModel;
using DomainServices.CustomerService.Api.Dto;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using DomainServices.CustomerService.Api.Services.KonsDb;

namespace DomainServices.CustomerService.Api.Handlers;

internal class SearchCustomersHandler : IRequestHandler<SearchCustomersMediatrRequest, SearchCustomersResponse>
{
    private readonly CustomerManagementSearchProvider _cmSearchProvider;
    private readonly KonsDbSearchProvider _konsDbSearchProvider;
    private readonly ILogger<SearchCustomersHandler> _logger;

    public SearchCustomersHandler(CustomerManagementSearchProvider cmSearchProvider, KonsDbSearchProvider konsDbSearchProvider, ILogger<SearchCustomersHandler> logger)
    {
        _cmSearchProvider = cmSearchProvider;
        _konsDbSearchProvider = konsDbSearchProvider;
        _logger = logger;
    }

    public async Task<SearchCustomersResponse> Handle(SearchCustomersMediatrRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Search customers by request {request}", request.Request);

        var response = new SearchCustomersResponse();

        var customers = request.Request.Mandant switch
        {
            Mandants.Kb => await _cmSearchProvider.Search(request.Request, cancellationToken),
            Mandants.Mp => await _konsDbSearchProvider.Search(request.Request, cancellationToken),
            _ => throw new InvalidEnumArgumentException()
        };

        response.Customers.AddRange(customers);

        return response;
    }
}