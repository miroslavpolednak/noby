using Microsoft.AspNetCore.Authorization;
using NOBY.Api.Endpoints.Test.Rollback;

namespace NOBY.Api.Endpoints.Test;

[ApiController]
[Route("api/test")]
[AllowAnonymous]
public class TestController : ControllerBase
{
    [HttpPost("rollback")]
    public async Task<RollbackResponse> SendToCmp([FromQuery] int? id)
        => await _mediator.Send(new RollbackRequest(id));

    [HttpGet("t1")]
    public async Task T1()
    {
        throw new CisValidationException(111, "moje chybova hlaska");
    }

    [HttpGet("t2")]
    public async Task T2()
    {
        throw new CisNotFoundException(111, "moje chybova hlaska");
    }

    private readonly IHttpContextAccessor _context;
    private readonly IMediator _mediator;

    public TestController(IMediator mediator, IHttpContextAccessor context)
    {
        _context = context;
        _mediator = mediator;
    }
}
