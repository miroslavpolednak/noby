﻿using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.Admin;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly CIS.InternalServices.ServiceDiscovery.Abstraction.IDiscoveryServiceAbstraction _discoveryService;

    public AdminController(CIS.InternalServices.ServiceDiscovery.Abstraction.IDiscoveryServiceAbstraction discoveryService)
    {
        _discoveryService = discoveryService;
    }

    /// <summary>
    /// Seznam URL sluzeb zaregistrovanych v ServiceDiscovery
    /// </summary>
    [AllowAnonymous]
    [HttpGet("discovery-service")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "System Administration" })]
    [ProducesResponseType(typeof(IEnumerable<CIS.InternalServices.ServiceDiscovery.Abstraction.DiscoverableService>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<CIS.InternalServices.ServiceDiscovery.Abstraction.DiscoverableService>> GetDiscoveryServices(CancellationToken cancellationToken)
        => await _discoveryService.GetServices(cancellationToken);
}
