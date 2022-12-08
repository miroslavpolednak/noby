using DomainServices.CaseService.Contracts;
using ExternalServices.SbWebApi.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using NOBY.Api.Endpoints.Test.Rollback;
using Serilog;

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
        var svc = _context.HttpContext.RequestServices.GetRequiredService<ExternalServices.SbWebApi.V1.ISbWebApiClient>();
        await svc.CaseStateChanged(new ExternalServices.SbWebApi.Dto.CaseStateChangedRequest
        {
            Login = "990614w",
            CaseId = 1,
            ContractNumber = "xxxx",
            ClientFullName = "franta pepa",
            CaseStateName = "dalsi",
            ProductTypeId = 2001,
            OwnerUserCpm = "9999",
            OwnerUserIcp = "87877",
            Mandant = CIS.Foms.Enums.Mandants.Kb,
            RiskBusinessCaseId = "xxxx"
        });
    }

    [HttpGet("opt")]
    public async Task<string> OptionsTest()
    {
        var config = _context.HttpContext.RequestServices.GetRequiredService<IOptions<CIS.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration<ISbWebApiClient>>>();
        config.Value.ServiceUrl = "test";
        return config.Value.ServiceUrl;
    }

    private readonly IHttpContextAccessor _context;
    private readonly IMediator _mediator;

    public TestController(IMediator mediator, IHttpContextAccessor context)
    {
        _context = context;
        _mediator = mediator;
    }
}
