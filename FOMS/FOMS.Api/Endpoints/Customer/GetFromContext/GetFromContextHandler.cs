namespace FOMS.Api.Endpoints.Customer.Handlers;

internal class GetFromContextHandler
    : IRequestHandler<Dto.GetFromContextRequest, Dto.GetFromContextResponse>
{
    public async Task<Dto.GetFromContextResponse> Handle(Dto.GetFromContextRequest request, CancellationToken cancellationToken)
    {
        return new Dto.GetFromContextResponse
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateTime.Now
        };
    }

    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public GetFromContextHandler(DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}
