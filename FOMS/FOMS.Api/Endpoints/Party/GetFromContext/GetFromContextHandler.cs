
namespace FOMS.Api.Endpoints.Party.Handlers;

internal class GetFromContextHandler
    : IRequestHandler<Dto.GetFromContextRequest, Dto.GetFromContextResponse>
{
    public async Task<Dto.GetFromContextResponse> Handle(Dto.GetFromContextRequest request, CancellationToken cancellationToken)
    {
        /*var result = await _customerService.GetBasicDataByIdentifier(new DomainServices.CustomerService.Contracts.GetBasicDataByIdentifierRequest()
        {
            Identifier = ""
        });*/

        return new Dto.GetFromContextResponse
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1978, 11, 8)
        };
    }

    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public GetFromContextHandler(DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}
