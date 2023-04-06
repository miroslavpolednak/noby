using DomainServices.CaseService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CaseService.Api.Endpoints;

[Authorize]
internal sealed class CaseService 
    : Contracts.v1.CaseService.CaseServiceBase
{
    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateOfferContacts(UpdateOfferContactsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<CreateCaseResponse> CreateCase(CreateCaseRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Case> GetCaseDetail(GetCaseDetailRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<SearchCasesResponse> SearchCases(SearchCasesRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkOwnerToCase(LinkOwnerToCaseRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCaseData(UpdateCaseDataRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCaseState(UpdateCaseStateRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCustomerData(UpdateCustomerDataRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetCaseCountsResponse> GetCaseCounts(GetCaseCountsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteCase(DeleteCaseRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetTaskListResponse> GetTaskList(GetTaskListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override Task<GetProcessListResponse> GetProcessList(GetProcessListRequest request, ServerCallContext context) => 
        _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateActiveTasks(UpdateActiveTasksRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> NotifyStarbuild(NotifyStarbuildRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;
    public CaseService(IMediator mediator)
        => _mediator = mediator;
}
