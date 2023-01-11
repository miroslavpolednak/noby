namespace DomainServices.CustomerService.Api.Endpoints.UpdateCustomer;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResponse>
{
    public Task<UpdateCustomerResponse> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}