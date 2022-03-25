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
    
    public override async Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateCustomerMediatrRequest(request), context.CancellationToken);

    // obligations -----------------------------------------------------
    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateObligations(UpdateObligationsRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateObligationsMediatrRequest(request), context.CancellationToken);

    // incomes -----------------------------------------------------
    public override async Task<CreateIncomeResponse> CreateIncome(CreateIncomeRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CreateIncomeMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteIncome(IncomeIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.DeleteIncomeMediatrRequest(request.IncomeId), context.CancellationToken);

    public override async Task<Income> GetIncome(IncomeIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetIncomeMediatrRequest(request.IncomeId), context.CancellationToken);

    public override async Task<GetIncomeListResponse> GetIncomeList(GetIncomeListRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetIncomeListMediatrRequest(request.CustomerOnSAId), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateIncome(UpdateIncomeRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateIncomeMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateIncomeBaseData(UpdateIncomeBaseDataRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateIncomeBaseDataMediatrRequest(request), context.CancellationToken);

}