using DomainServices.CaseService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CaseService.Api.Services;

[Authorize]
internal class CaseService : Contracts.v1.CaseService.CaseServiceBase
{
    private readonly IMediator _mediator;

    public CaseService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<CreateCaseResponse> CreateCase(CreateCaseRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CaseService.CreateCaseMediatrRequest(request));

    public override async Task<GetCaseDetailResponse> GetCaseDetail(GetCaseDetailRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CaseService.GetCaseDetailMediatrRequest(request));

    public override async Task<GetCaseListResponse> GetCaseList(GetCaseListRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CaseService.GetCaseListMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkOwnerToCase(LinkOwnerToCaseRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CaseService.LinkOwnerToCaseMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCaseData(UpdateCaseDataRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CaseService.UpdateCaseDataMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCaseState(UpdateCaseStateRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CaseService.UpdateCaseStateMediatrRequest(request));
}

