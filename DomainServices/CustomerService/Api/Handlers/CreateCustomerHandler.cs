using DomainServices.CustomerService.Api.Dto;

namespace DomainServices.CustomerService.Api.Handlers;

internal class CreateCustomerHandler : IRequestHandler<CreateCustomerMediatrRequest, CreateCustomerResponse>
{
    public CreateCustomerHandler()
    {
    }

    public Task<CreateCustomerResponse> Handle(CreateCustomerMediatrRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}