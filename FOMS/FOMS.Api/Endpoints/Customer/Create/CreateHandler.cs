namespace FOMS.Api.Endpoints.Customer.Create;

internal sealed class CreateHandler
    : IRequestHandler<CreateRequest, CreateResponse>
{
    public async Task<CreateResponse> Handle(CreateRequest request, CancellationToken cancellationToken)
    {
        await _customerService.CreateCustomer(request.ToDomainService(), cancellationToken);
    }

    private readonly ILogger<CreateHandler> _logger;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public CreateHandler(
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        ILogger<CreateHandler> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }
}
