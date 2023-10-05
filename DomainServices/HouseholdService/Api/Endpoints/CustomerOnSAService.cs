﻿using DomainServices.HouseholdService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.HouseholdService.Api.Endpoints;

[Authorize]
internal sealed class CustomerOnSAService
    : Contracts.v1.CustomerOnSAService.CustomerOnSAServiceBase
{
    public override async Task<GetCustomerChangeMetadataResponse> GetCustomerChangeMetadata(GetCustomerChangeMetadataRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteCustomer(DeleteCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Contracts.CustomerOnSA> GetCustomer(GetCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetCustomersByIdentityResponse> GetCustomersByIdentity(GetCustomersByIdentityRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetCustomerListResponse> GetCustomerList(GetCustomerListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCustomerDetail(UpdateCustomerDetailRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    // obligations -----------------------------------------------------
    public override async Task<CreateObligationResponse> CreateObligation(CreateObligationRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateObligation(Contracts.Obligation request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Contracts.Obligation> GetObligation(GetObligationRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteObligation(DeleteObligationRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetObligationListResponse> GetObligationList(GetObligationListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    // incomes -----------------------------------------------------
    public override async Task<CreateIncomeResponse> CreateIncome(CreateIncomeRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteIncome(DeleteIncomeRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Contracts.Income> GetIncome(GetIncomeRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetIncomeListResponse> GetIncomeList(GetIncomeListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateIncome(UpdateIncomeRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateIncomeBaseData(UpdateIncomeBaseDataRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<ValidateCustomerOnSAIdResponse> ValidateCustomerOnSAId(ValidateCustomerOnSAIdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;

    public CustomerOnSAService(IMediator mediator)
        => _mediator = mediator;
}