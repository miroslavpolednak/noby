using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace NOBY.Api.Endpoints.Test;

[ApiController]
[Route("api/v{v:apiVersion}/test")]
[AllowAnonymous]
[ApiVersion(1)]
public class TestController(IMediator _mediator, IHttpContextAccessor _context) 
    : ControllerBase
{
    /*[HttpGet("test2")]
    public async Task<RefinancingSharedResponseCode> Test2()
    {
        return new RefinancingSharedResponseCode
        {
            CreatedTime = DateTime.Now,
            DataDateTime = DateTime.Now
        };
    }*/
}
