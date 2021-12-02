namespace FOMS.Api.Endpoints.Party.Handlers;

internal class SearchHandler
    : IRequestHandler<Dto.SearchRequest, List<Dto.SearchResponse>>
{
    public async Task<List<Dto.SearchResponse>> Handle(Dto.SearchRequest request, CancellationToken cancellationToken)
    {
        return new List<Dto.SearchResponse>
        {
            new Dto.SearchResponse { Id = 1, Name = "John Doe" },
            new Dto.SearchResponse { Id = 2, Name = "Marky Mark" },
            new Dto.SearchResponse { Id = 3, Name = "Franky Moore" },
        };
    }

    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public SearchHandler(DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}
