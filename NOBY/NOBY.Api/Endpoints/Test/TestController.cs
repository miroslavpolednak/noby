using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using DomainServices.CaseService.Clients.v1;

namespace NOBY.Api.Endpoints.Test;

[ApiController]
[Route("api/v{v:apiVersion}/test")]
[AllowAnonymous]
[ApiVersion(1)]
public class TestController : ControllerBase
{
    [HttpGet("test2")]
    public async Task T2()
    {
        var service = _context.HttpContext.RequestServices.GetRequiredService<ICaseServiceClient>();

        var i1 = await service.ValidateCaseId(1);
        var i2 = await service.ValidateCaseId(1);

        var s = "";
    }

    private readonly IHttpContextAccessor _context;
    private readonly IMediator _mediator;

    public TestController(IMediator mediator, IHttpContextAccessor context)
    {
        _context = context;
        _mediator = mediator;
    }
}
