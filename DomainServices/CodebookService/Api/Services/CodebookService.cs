using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CodebookService.Api.Services;

[Authorize]
public partial class CodebookService
{
    private readonly MediatR.IMediator _mediator;

    public CodebookService(MediatR.IMediator mediator)
        => this._mediator = mediator;
}
