using DomainServices.CaseService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CaseService.Api.Endpoints;

[Authorize]
internal sealed class CaseService 
    : Contracts.v1.CaseService.CaseServiceBase
{
    public override async Task<Empty> CompleteTask(CompleteTaskRequest request, ServerCallContext context)
    {
        await _mediator.Send(request, context.CancellationToken);

        return new Empty();
    }

    public override async Task<ValidateCaseIdResponse> ValidateCaseId(ValidateCaseIdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> UpdateOfferContacts(UpdateOfferContactsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<CreateCaseResponse> CreateCase(CreateCaseRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Case> GetCaseDetail(GetCaseDetailRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<SearchCasesResponse> SearchCases(SearchCasesRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> LinkOwnerToCase(LinkOwnerToCaseRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> UpdateCaseData(UpdateCaseDataRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> UpdateCaseState(UpdateCaseStateRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> UpdateCustomerData(UpdateCustomerDataRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetCaseCountsResponse> GetCaseCounts(GetCaseCountsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> DeleteCase(DeleteCaseRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override Task<GetTaskDetailResponse> GetTaskDetail(GetTaskDetailRequest request, ServerCallContext context) => 
        _mediator.Send(request, context.CancellationToken);

    public override async Task<GetTaskListResponse> GetTaskList(GetTaskListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override Task<GetProcessListResponse> GetProcessList(GetProcessListRequest request, ServerCallContext context) => 
        _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> UpdateActiveTasks(UpdateActiveTasksRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> NotifyStarbuild(NotifyStarbuildRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> CancelTask(CancelTaskRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;
    public CaseService(IMediator mediator)
        => _mediator = mediator;
}
