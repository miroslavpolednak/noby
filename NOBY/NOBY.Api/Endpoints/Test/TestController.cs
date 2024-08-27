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
    public async Task<IActionResult> T2()
    {
        var service = _context.HttpContext.RequestServices.GetRequiredService<ICaseServiceClient>();

        var i1 = await service.ValidateCaseId(3092450);
        //var i2 = await service.ValidateCaseId(3092450);

        await service.LinkOwnerToCase(3092450, 5248);

        return Content(System.Text.Json.JsonSerializer.Serialize(i1));
    }*/
}
