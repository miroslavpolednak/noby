using DomainServices.CaseService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CaseService.Api.Services;

[Authorize]
internal class SalesArrangementService : Contracts.v1.SalesArrangementService.SalesArrangementServiceBase
{
    private readonly IMediator _mediator;

    public SalesArrangementService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<CreateSalesArrangementResponse> CreateSalesArrangement(CreateSalesArrangementRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.SalesArrangement.CreateSalesArrangementMediatrRequest(request));

    public override async Task<GetSalesArrangementDetailResponse> GetSalesArrangementDetail(SalesArrangementIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.SalesArrangement.GetSalesArrangementDetailMediatrRequest(request));

    public override async Task<GetSalesArrangementsByCaseIdResponse> GetSalesArrangementsByCaseId(GetSalesArrangementsByCaseIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.SalesArrangement.GetSalesArrangementsByCaseIdMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateSalesArrangementState(UpdateSalesArrangementStateRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.SalesArrangement.UpdateSalesArrangementStateMediatrRequest(request));

    public override async Task<ValidateSalesArrangementResponse> ValidateSalesArrangement(SalesArrangementIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.SalesArrangement.ValidateSalesArrangementMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateSalesArrangementData(UpdateSalesArrangementDataRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.SalesArrangement.UpdateSalesArrangementDataMediatrRequest(request));
}
