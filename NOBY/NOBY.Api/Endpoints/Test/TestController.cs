using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace NOBY.Api.Endpoints.Test;

[ApiController]
[Route("api/v{v:apiVersion}/test")]
[AllowAnonymous]
[ApiVersion(1)]
public class TestController : ControllerBase
{

    //[ApiVersion(2)]
    //[HttpGet("test2")]
    //public async Task<string> T2()
    //{
    //    return "test v2";
    //}

    private readonly IHttpContextAccessor _context;
    private readonly IMediator _mediator;

    public TestController(IMediator mediator, IHttpContextAccessor context)
    {
        _context = context;
        _mediator = mediator;
    }
}
