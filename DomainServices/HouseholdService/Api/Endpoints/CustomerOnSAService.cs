using _C = DomainServices.HouseholdService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.HouseholdService.Api.Endpoints;

[Authorize]
internal class CustomerOnSAService
    : _C.v1.CustomerOnSAService.CustomerOnSAServiceBase
{
    public override async Task<_C.CreateCustomerResponse> CreateCustomer(_C.CreateCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.CreateCustomer.CreateCustomerMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteCustomer(_C.DeleteCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.DeleteCustomer.DeleteCustomerMediatrRequest(request.CustomerOnSAId, request.HardDelete), context.CancellationToken);

    public override async Task<_C.CustomerOnSA> GetCustomer(_C.CustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.GetCustomer.GetCustomerMediatrRequest(request.CustomerOnSAId), context.CancellationToken);

    public override async Task<_C.GetCustomerListResponse> GetCustomerList(_C.GetCustomerListRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.GetCustomerList.GetCustomerListMediatrRequest(request.SalesArrangementId), context.CancellationToken);

    public override async Task<_C.UpdateCustomerResponse> UpdateCustomer(_C.UpdateCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.UpdateCustomer.UpdateCustomerMediatrRequest(request), context.CancellationToken);

    // obligations -----------------------------------------------------
    public override async Task<_C.CreateObligationResponse> CreateObligation(_C.CreateObligationRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.CreateObligation.CreateObligationMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateObligation(_C.Obligation request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.UpdateObligation.UpdateObligationMediatrRequest(request), context.CancellationToken);

    public override async Task<_C.Obligation> GetObligation(_C.ObligationIdRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.GetObligation.GetObligationMediatrRequest(request.ObligationId), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteObligation(_C.ObligationIdRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.DeleteObligation.DeleteObligationMediatrRequest(request.ObligationId), context.CancellationToken);

    public override async Task<_C.GetObligationListResponse> GetObligationList(_C.CustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.GetObligationList.GetObligationListMediatrRequest(request.CustomerOnSAId), context.CancellationToken);

    // incomes -----------------------------------------------------
    public override async Task<_C.CreateIncomeResponse> CreateIncome(_C.CreateIncomeRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.CreateIncome.CreateIncomeMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteIncome(_C.IncomeIdRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.DeleteIncome.DeleteIncomeMediatrRequest(request.IncomeId), context.CancellationToken);

    public override async Task<_C.Income> GetIncome(_C.IncomeIdRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.GetIncome.GetIncomeMediatrRequest(request.IncomeId), context.CancellationToken);

    public override async Task<_C.GetIncomeListResponse> GetIncomeList(_C.GetIncomeListRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.GetIncomeList.GetIncomeListMediatrRequest(request.CustomerOnSAId), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateIncome(_C.UpdateIncomeRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.UpdateIncome.UpdateIncomeMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateIncomeBaseData(_C.UpdateIncomeBaseDataRequest request, ServerCallContext context)
        => await _mediator.Send(new CustomerOnSA.UpdateIncomeBaseData.UpdateIncomeBaseDataMediatrRequest(request), context.CancellationToken);

    private readonly IMediator _mediator;

    public CustomerOnSAService(IMediator mediator)
        => _mediator = mediator;
}