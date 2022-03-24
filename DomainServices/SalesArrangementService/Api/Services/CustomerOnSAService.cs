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
        => await _mediator.Send(new Dto.CreateCustomerMediatrRequest(request), context.CancellationToken);
    
    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteCustomer(CustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.DeleteCustomerMediatrRequest(request.CustomerOnSAId), context.CancellationToken);
    
    public override async Task<CustomerOnSA> GetCustomer(CustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetCustomerMediatrRequest(request.CustomerOnSAId), context.CancellationToken);
    
    public override async Task<GetCustomerListResponse> GetCustomerList(GetCustomerListRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetCustomerListMediatrRequest(request.SalesArrangementId), context.CancellationToken);
    
    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateCustomerMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateObligations(UpdateObligationsRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateObligationsMediatrRequest(request), context.CancellationToken);
}