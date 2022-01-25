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
        => await _mediator.Send(new Dto.CreateCaseMediatrRequest(request));

    public override async Task<Case> GetCaseDetail(GetCaseDetailRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetCaseDetailMediatrRequest(request));

    public override async Task<SearchCasesResponse> SearchCases(SearchCasesRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.SearchCasesMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkOwnerToCase(LinkOwnerToCaseRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.LinkOwnerToCaseMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCaseData(UpdateCaseDataRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateCaseDataMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCaseState(UpdateCaseStateRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateCaseStateMediatrRequest(request));

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateCaseCustomer(UpdateCaseCustomerRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateCaseCustomerMediatrRequest(request));
}

