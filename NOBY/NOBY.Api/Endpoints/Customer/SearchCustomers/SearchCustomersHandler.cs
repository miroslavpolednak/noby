using CIS.Core.Types;
using DomainServices.CustomerService.Clients;
using __Customer = DomainServices.CustomerService.Contracts;

namespace NOBY.Api.Endpoints.Customer.SearchCustomers;

internal sealed class SearchCustomersHandler
    : IRequestHandler<CustomerSearchCustomersRequest, CustomerSearchCustomersResponse?>
{
    public async Task<CustomerSearchCustomersResponse?> Handle(CustomerSearchCustomersRequest request, CancellationToken cancellationToken)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination)
            .EnsureAndTranslateSortFields(sortingMapper)
            .SetDefaultSort("LastName", true);

        var dsRequest = request.SearchData?.ToDomainServiceRequest() ?? new __Customer.SearchCustomersRequest();
        
        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var searchResult = await _customerService.SearchCustomers(dsRequest, cancellationToken);

        if (searchResult.Customers.Count != 0)
        {
            return new CustomerSearchCustomersResponse
            {
                Rows = searchResult.Customers.ToApiResponse(),
                Pagination = new SharedTypesPaginationResponse(request.Pagination as IPaginableRequest ?? paginable, searchResult.Customers.Count)
            };
        }

        return null;
    }

    private static List<Paginable.MapperField> sortingMapper = new()
    {
        new ("lastName", "LastName")
    };

    private readonly ICustomerServiceClient _customerService;

    public SearchCustomersHandler(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}