namespace FOMS.Api.Endpoints.Customer.Handlers;

internal class GetFromTokenHandler
    : IRequestHandler<Dto.GetFromTokenRequest, Dto.GetFromTokenResponse>
{
    public async Task<Dto.GetFromTokenResponse> Handle(Dto.GetFromTokenRequest request, CancellationToken cancellationToken)
    {
        return new Dto.GetFromTokenResponse
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateTime.Now
        };
    }

    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public GetFromTokenHandler(DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}
