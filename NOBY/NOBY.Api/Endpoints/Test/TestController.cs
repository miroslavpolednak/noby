using CIS.Infrastructure.Audit;
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
            CIS.Infrastructure.Audit.AuditEventTypes.Noby001,
            "Nejaka fajn zprava",
            identities: new List<AuditLoggerHeaderItem> { new("aaa", "bbb") },
            products: new List<AuditLoggerHeaderItem> { new("111", "Uver") },
            operation: new("111", "CreateCase"),
            bodyBefore: new Dictionary<string, string> { { "aaa", "bbb" }, { "ccc", "dddd" } }
        );
    }

    [HttpGet("t3")]
    public async Task T3()
    {
        var client = _context.HttpContext.RequestServices.GetRequiredService<DomainServices.CaseService.Clients.ICaseServiceClient>();
        var c = await client.GetCaseDetail(1);
         
    }

    private readonly IHttpContextAccessor _context;
    private readonly IMediator _mediator;

    public TestController(IMediator mediator, IHttpContextAccessor context)
    {
        _context = context;
        _mediator = mediator;
    }
}
