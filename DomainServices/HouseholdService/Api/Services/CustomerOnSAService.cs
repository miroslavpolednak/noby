using DomainServices.HouseholdService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.HouseholdService.Api.Services;

[Authorize]
internal class CustomerOnSAService : Contracts.v1.CustomerOnSAService.CustomerOnSAServiceBase
{
    private readonly IMediator _mediator;

    public CustomerOnSAService(IMediator mediator)
        => _mediator = mediator;
    
    public override async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.CreateCustomer.CreateCustomerMediatrRequest(request), context.CancellationToken);
    
    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteCustomer(CustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.DeleteCustomer.DeleteCustomerMediatrRequest(request.CustomerOnSAId), context.CancellationToken);
    
    public override async Task<CustomerOnSA> GetCustomer(CustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.GetCustomer.GetCustomerMediatrRequest(request.CustomerOnSAId), context.CancellationToken);
    
    public override async Task<GetCustomerListResponse> GetCustomerList(GetCustomerListRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.GetCustomerList.GetCustomerListMediatrRequest(request.SalesArrangementId), context.CancellationToken);
    
    public override async Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.UpdateCustomer.UpdateCustomerMediatrRequest(request), context.CancellationToken);

    // obligations -----------------------------------------------------
    public override async Task<CreateObligationResponse> CreateObligation(CreateObligationRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.CreateObligation.CreateObligationMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateObligation(Obligation request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.UpdateObligation.UpdateObligationMediatrRequest(request), context.CancellationToken);

    public override async Task<Obligation> GetObligation(ObligationIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.GetObligation.GetObligationMediatrRequest(request.ObligationId), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteObligation(ObligationIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.DeleteObligation.DeleteObligationMediatrRequest(request.ObligationId), context.CancellationToken);

    public override async Task<GetObligationListResponse> GetObligationList(CustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.GetObligationList.GetObligationListMediatrRequest(request.CustomerOnSAId), context.CancellationToken);

    // incomes -----------------------------------------------------
    public override async Task<CreateIncomeResponse> CreateIncome(CreateIncomeRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.CreateIncome.CreateIncomeMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteIncome(IncomeIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.DeleteIncome.DeleteIncomeMediatrRequest(request.IncomeId), context.CancellationToken);

    public override async Task<Income> GetIncome(IncomeIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.GetIncome.GetIncomeMediatrRequest(request.IncomeId), context.CancellationToken);

    public override async Task<GetIncomeListResponse> GetIncomeList(GetIncomeListRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.GetIncomeList.GetIncomeListMediatrRequest(request.CustomerOnSAId), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateIncome(UpdateIncomeRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.UpdateIncome.UpdateIncomeMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateIncomeBaseData(UpdateIncomeBaseDataRequest request, ServerCallContext context)
        => await _mediator.Send(new Endpoints.CustomerOnSA.UpdateIncomeBaseData.UpdateIncomeBaseDataMediatrRequest(request), context.CancellationToken);

}