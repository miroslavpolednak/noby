using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using Google.Protobuf.WellKnownTypes;
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

    public override async Task<SearchCustomersResponse> SearchCustomers(SearchCustomersRequest request, ServerCallContext context)
        => await _mediator.Send(new SearchCustomersMediatrRequest(request));

    public override async Task<CustomerListResponse> GetCustomerList(CustomerListRequest request, ServerCallContext context)
        => await _mediator.Send(new GetCustomerListMediatrRequest(request));

    public override async Task<CustomerResponse> GetCustomerDetail(CustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new GetCustomerDetailMediatrRequest(request));

    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
        => await _mediator.Send(new CreateMediatrRequest(request));

    public override async Task<CreateContactResponse> CreateContact(CreateContactRequest request, ServerCallContext context)
        => await _mediator.Send(new CreateContactMediatrRequest(request));

    public override async Task<Empty> DeleteContact(DeleteContactRequest request, ServerCallContext context)
        => await _mediator.Send(new DeleteContactMediatrRequest(request));

    public override async Task<Empty> UpdateAdress(UpdateAdressRequest request, ServerCallContext context)
        => await _mediator.Send(new UpdateAdressMediatrRequest(request));

    public override async Task<Empty> UpdateBasicData(UpdateBasicDataRequest request, ServerCallContext context)
        => await _mediator.Send(new UpdateBasicDataMediatrRequest(request));

}
