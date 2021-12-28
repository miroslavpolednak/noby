using DomainServices.SalesArrangementService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.SalesArrangementService.Api.Services;

[Authorize]
internal class SalesArrangementService : Contracts.v1.SalesArrangementService.SalesArrangementServiceBase
{
    private readonly IMediator _mediator;

    public SalesArrangementService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<CreateSalesArrangementResponse> CreateSalesArrangement(CreateSalesArrangementRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CreateSalesArrangementMediatrRequest(request));

    public override async Task<GetSalesArrangementResponse> GetSalesArrangement(SalesArrangementIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetSalesArrangementMediatrRequest(request));

    public override async Task<GetSalesArrangementDataResponse> GetSalesArrangementData(SalesArrangementIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetSalesArrangementDataMediatrRequest(request));

    public override async Task<GetSalesArrangementsByCaseIdResponse> GetSalesArrangementsByCaseId(GetSalesArrangementsByCaseIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetSalesArrangementsByCaseIdMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateSalesArrangementState(UpdateSalesArrangementStateRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateSalesArrangementStateMediatrRequest(request));

    public override async Task<ValidateSalesArrangementResponse> ValidateSalesArrangement(SalesArrangementIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.ValidateSalesArrangementMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateSalesArrangementData(UpdateSalesArrangementDataRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateSalesArrangementDataMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkModelationToSalesArrangement(LinkModelationToSalesArrangementRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.LinkModelationToSalesArrangementMediatrRequest(request));
}
