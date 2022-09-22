using DomainServices.CustomerService.Api.Dto;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CustomerService.Api.Services;

[Authorize]
internal class CustomerService : Contracts.V1.CustomerService.CustomerServiceBase
{
    private readonly IMediator _mediator;

    public CustomerService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override Task<ProfileCheckResponse> ProfileCheck(ProfileCheckRequest request, ServerCallContext context)
        => _mediator.Send(new ProfileCheckMediatrRequest(request), context.CancellationToken);

    public override Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
    {
        return _mediator.Send(new CreateCustomerMediatrRequest(request), context.CancellationToken);
    }

    public override Task<CustomerDetailResponse> GetCustomerDetail(CustomerDetailRequest request, ServerCallContext context)
        => _mediator.Send(new GetCustomerDetailMediatrRequest(request.Identity), context.CancellationToken);

    public override async Task<CustomerListResponse> GetCustomerList(CustomerListRequest request, ServerCallContext context)
        => await _mediator.Send(new GetCustomerListMediatrRequest(request.Identities), context.CancellationToken);

    public override async Task<SearchCustomersResponse> SearchCustomers(SearchCustomersRequest request, ServerCallContext context)
        => await _mediator.Send(new SearchCustomersMediatrRequest(request), context.CancellationToken);
}
