using CIS.Infrastructure.Telemetry;
using CIS.Infrastructure.Telemetry.AuditLog;
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

    /// <summary>
    /// Summary endpointu
    /// </summary>
    /// <remarks>Toto jsou remarks</remarks>
    [HttpGet("t1")]
    [NobyAuthorize(UserPermissions.UC_getWflSigningAttachments, UserPermissions.CASEDETAIL_APPLICANT_ViewPersonInfo)]
    [Infrastructure.Swagger.SwaggerEaDiagram("https://eadiagram.com/neco")]
    public async Task T1()
    {
        throw new CisValidationException(111, "moje chybova hlaska");
    }

    [HttpGet("t2")]
    public async Task T2()
    {
        var logger = _context.HttpContext.RequestServices.GetRequiredService<IAuditLogger>();
        logger.Log(
            CIS.Infrastructure.Telemetry.AuditLog.AuditEventTypes.Prvni,
            identities: new List<AuditLoggerHeaderItem> { new() { Id = "aaa", Type = "bbb" } }
        );
    }

    [HttpGet("t3")]
    public async Task T3()
    {
        var list = new List<CIS.Infrastructure.gRPC.CisTypes.Identity>
        {
            new CIS.Infrastructure.gRPC.CisTypes.Identity(951061749, CIS.Foms.Enums.IdentitySchemes.Kb),
            new CIS.Infrastructure.gRPC.CisTypes.Identity(300522530, CIS.Foms.Enums.IdentitySchemes.Mp)
        };
        var notification = new Notifications.MainCustomerUpdatedNotification(2987188, 45, 60, list);
        await _mediator.Publish(notification);
    }

    private readonly IHttpContextAccessor _context;
    private readonly IMediator _mediator;

    public TestController(IMediator mediator, IHttpContextAccessor context)
    {
        _context = context;
        _mediator = mediator;
    }
}
