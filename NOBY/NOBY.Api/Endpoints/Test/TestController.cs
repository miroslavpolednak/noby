using SharedAudit;
using Microsoft.AspNetCore.Authorization;
using NOBY.Api.Endpoints.Test.Rollback;
using SharedTypes;
using Asp.Versioning;
using SharedComponents.Storage;

namespace NOBY.Api.Endpoints.Test;

[ApiController]
[Route("api/test")]
[AllowAnonymous]
[ApiVersion(1)]
public class TestController : ControllerBase
{
    /*public async Task<string> T1()
    {
        var manager = _context.HttpContext.RequestServices.GetRequiredService<IFeatureManager>();
        return await manager.IsEnabledAsync(FeatureFlagsConstants.BlueBang) ? "true" : "false";
    }*/

    private readonly IHttpContextAccessor _context;
    private readonly IMediator _mediator;

    public TestController(IMediator mediator, IHttpContextAccessor context)
    {
        _context = context;
        _mediator = mediator;
    }
}
