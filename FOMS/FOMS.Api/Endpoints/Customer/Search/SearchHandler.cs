namespace FOMS.Api.Endpoints.Customer.Handlers;

internal class SearchHandler
    : IRequestHandler<Dto.SearchRequest, Dto.SearchResponse>
{
    public async Task<Dto.SearchResponse> Handle(Dto.SearchRequest request, CancellationToken cancellationToken)
    {
        return new Dto.SearchResponse
        {
            Pagination = new CIS.Infrastructure.WebApi.Types.PaginationResponse()
            {
                PageSize = request.Pagination.PageSize,
                RecordsTotalSize = 2,
                RecordOffset = request.Pagination.RecordOffset
            },
            Rows = new List<Dto.SearchResponse.Customer>()
            {
                new Dto.SearchResponse.Customer { Id = 4, FullName = "Václav  Horažďovský", City = "Praha", Street = "josefova 1", Phone = "999 999 999" },
                new Dto.SearchResponse.Customer { Id = 5, FullName = "Roman   Tichý", City = "Praha", Street = "josefova 1", Phone = "999 999 999" },
                new Dto.SearchResponse.Customer { Id = 8, FullName = "Jindřich    Šimůnek", City = "Praha", Street = "josefova 1", Phone = "999 999 999" }
            }
        };
    }

    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public SearchHandler(DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}
