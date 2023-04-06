using DomainServices.CodebookService.Endpoints;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Api.Services;

[Authorize]
public partial class CodebookService
{
    private readonly IMediator _mediator;

    public async Task<List<Contracts.Endpoints.DeveloperSearch.DeveloperSearchItem>> DeveloperSearch(Contracts.Endpoints.DeveloperSearch.DeveloperSearchRequest request, CallContext context = default)
        => await _mediator.Send(request, context.CancellationToken);

    public async Task<Contracts.Endpoints.GetDeveloper.DeveloperItem> GetDeveloper(Contracts.Endpoints.GetDeveloper.GetDeveloperRequest request, CallContext context = default)
        => await _mediator.Send(request, context.CancellationToken);

    public async Task<Contracts.Endpoints.GetDeveloperProject.DeveloperProjectItem> GetDeveloperProject(Contracts.Endpoints.GetDeveloperProject.GetDeveloperProjectRequest request, CallContext context = default)
        => await _mediator.Send(request, context.CancellationToken);

    public async Task<Contracts.Endpoints.GetOperator.GetOperatorItem> GetOperator(Contracts.Endpoints.GetOperator.GetOperatorRequest request, CallContext context = default)
        => await _mediator.Send(request, context.CancellationToken);

    public ValueTask Reset(CancellationToken cancellationToken = default)
    {
        FastMemoryCache.Reset();

        return ValueTask.CompletedTask;
    }

    public CodebookService(IMediator mediator)
        => this._mediator = mediator;
}
