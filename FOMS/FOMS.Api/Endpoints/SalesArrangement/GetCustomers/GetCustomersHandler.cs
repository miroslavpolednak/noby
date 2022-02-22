namespace FOMS.Api.Endpoints.SalesArrangement.GetCustomers;

internal class GetCustomersHandler
    : IRequestHandler<GetCustomersRequest, List<Dto.CustomerListItem>>
{
    public async Task<List<Dto.CustomerListItem>> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCustomersHandler), request.SalesArrangementId);

        return new List<Dto.CustomerListItem>
        {
            new() {Id = 1, FirstName = "John", LastName = "Doe"}
        };
    }

    private readonly ILogger<GetCustomersHandler> _logger;

    public GetCustomersHandler(ILogger<GetCustomersHandler> logger)
    {
        _logger = logger;
    }
}