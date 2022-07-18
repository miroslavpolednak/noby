using DomainServices.CodebookService.Endpoints;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CodebookService.Api.Services;

[Authorize]
public partial class CodebookService
{
    private readonly IMediator _mediator;

    public ValueTask Reset(CancellationToken cancellationToken = default)
    {
        FastMemoryCache.Reset();

        return ValueTask.CompletedTask;
    }

    public CodebookService(IMediator mediator)
        => this._mediator = mediator;
}
