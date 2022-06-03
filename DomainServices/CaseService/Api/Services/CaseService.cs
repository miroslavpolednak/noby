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
        => await _mediator.Send(new Dto.CreateCaseMediatrRequest(request), context.CancellationToken);

    public override async Task<Case> GetCaseDetail(GetCaseDetailRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetCaseDetailMediatrRequest(request), context.CancellationToken);

    public override async Task<SearchCasesResponse> SearchCases(SearchCasesRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.SearchCasesMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkOwnerToCase(LinkOwnerToCaseRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.LinkOwnerToCaseMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCaseData(UpdateCaseDataRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateCaseDataMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCaseState(UpdateCaseStateRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateCaseStateMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCaseCustomer(UpdateCaseCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateCaseCustomerMediatrRequest(request), context.CancellationToken);

    public override async Task<GetCaseCountsResponse> GetCaseCounts(GetCaseCountsRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetCaseCountsMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteCase(DeleteCaseRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.DeleteCaseMediatrRequest(request), context.CancellationToken);

    public override async Task<GetTaskListResponse> GetTaskList(GetTaskListRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetTaskListMediatrRequest(request.CaseId), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateActiveTasks(UpdateActiveTasksRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateActiveTasksMediatrRequest(request), context.CancellationToken);
}

