using _V2 = DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2;

[Authorize]
[ApiController]
[Route("v2/customers-exposure")]
public sealed class CustomersExposureServiceController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersExposureServiceController(IMediator mediator)
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
    [ProducesResponseType(typeof(_V2.CustomersExposureCalculateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<_V2.CustomersExposureCalculateResponse> Calculate([FromBody] _V2.CustomersExposureCalculateRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
