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

    [HttpGet("extsvc")]
    public async Task ExtSvc()
    {
    }

    private readonly IHttpContextAccessor _context;
    private readonly IMediator _mediator;

    public TestController(IMediator mediator, IHttpContextAccessor context)
    {
        _context = context;
        _mediator = mediator;
    }
}
