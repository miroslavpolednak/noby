namespace FOMS.Api.Endpoints.SalesArrangement.Handlers;

internal class GetCustomersHandler
    : IRequestHandler<Dto.GetCustomersRequest, List<Dto.CustomerListItem>>
{
    public async Task<List<Dto.CustomerListItem>> Handle(Dto.GetCustomersRequest request, CancellationToken cancellationToken)
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