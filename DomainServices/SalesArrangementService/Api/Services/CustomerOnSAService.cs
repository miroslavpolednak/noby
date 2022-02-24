using DomainServices.SalesArrangementService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.SalesArrangementService.Api.Services;

[Authorize]
internal class CustomerOnSAService : Contracts.v1.CustomerOnSAService.CustomerOnSAServiceBase
{
    private readonly IMediator _mediator;

    public CustomerOnSAService(IMediator mediator)
        => _mediator = mediator;
    
    public override async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CreateCustomerMediatrRequest(request));
    
    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteCustomer(CustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.DeleteCustomerMediatrRequest(request.CustomerOnSAId));
    
    public override async Task<CustomerOnSA> GetCustomer(CustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetCustomerMediatrRequest(request.CustomerOnSAId));
    
    public override async Task<GetCustomerListResponse> GetCustomerList(GetCustomerListRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetCustomerListMediatrRequest(request.SalesArrangementId));
}