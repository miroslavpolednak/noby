using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using NOBY.Services.FileAntivirus;
using SharedAudit;

namespace NOBY.Api.Endpoints.Test;

[ApiController]
[ApiVersion(2)]
[Route("api/test")]
[AllowAnonymous]
public class V2TestController:ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _context;

    public V2TestController(IMediator mediator, IHttpContextAccessor context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet("t2")]
    public async Task T2()
    {
        var logger = _context.HttpContext.RequestServices.GetRequiredService<IAuditLogger>();
        logger.Log(
            SharedAudit.AuditEventTypes.Noby001,
            "Nejaka fajn zprava",
            identities: new List<AuditLoggerHeaderItem> { new("aaa", "bbb") },
            products: new List<AuditLoggerHeaderItem> { new("111", "Uver") },
            operation: new("111", "CreateCase"),
            bodyBefore: new Dictionary<string, string> { { "aaa", "bbb" }, { "ccc", "dddd" } }
        );
    }

    [HttpGet("t3")]
    public async Task<string> T3()
    {
        var client = _context.HttpContext.RequestServices.GetRequiredService<IFileAntivirusService>();
        var file = System.Text.Encoding.ASCII.GetBytes("X5O!P%@AP[4\\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*");
        var result = await client.CheckFile(file);
        return result.ToString();
    }
}
