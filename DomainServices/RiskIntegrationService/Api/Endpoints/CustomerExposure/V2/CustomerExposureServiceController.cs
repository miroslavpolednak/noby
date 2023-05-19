using _V2 = DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2;

[Authorize]
[ApiController]
[Route("v2/customers-exposure")]
public sealed class CustomerExposureServiceController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerExposureServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Vrátí data související s angažovaností jednotlivých účastníků úvěrové žádosti(Loan Applicaiton).
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/LOAN+APPLICATION+EXPOSURE">https://wiki.kb.cz/display/HT/LOAN+APPLICATION+EXPOSURE</a>
    /// </remarks>
    [HttpPut()]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Customers Exposure" })]
    [ProducesResponseType(typeof(_V2.CustomerExposureCalculateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<_V2.CustomerExposureCalculateResponse> Calculate([FromBody] _V2.CustomerExposureCalculateRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
