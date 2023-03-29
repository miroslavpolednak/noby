﻿using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Admin;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly CIS.InternalServices.ServiceDiscovery.Clients.IDiscoveryServiceClient _discoveryService;

    public AdminController(CIS.InternalServices.ServiceDiscovery.Clients.IDiscoveryServiceClient discoveryService)
    {
        _discoveryService = discoveryService;
    }

    /// <summary>
    /// Seznam URL služeb zaregistrovaných v ServiceDiscovery
    /// </summary>
    [AllowAnonymous]
    [HttpGet("discovery-service")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "System Administration" })]
    [ProducesResponseType(typeof(IEnumerable<CIS.InternalServices.ServiceDiscovery.Clients.DiscoverableService>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<CIS.InternalServices.ServiceDiscovery.Clients.DiscoverableService>> GetDiscoveryServices(CancellationToken cancellationToken)
        => await _discoveryService.GetServices(cancellationToken);
}
