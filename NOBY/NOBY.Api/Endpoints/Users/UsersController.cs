using Microsoft.AspNetCore.Authorization;

namespace NOBY.Api.Endpoints.Users;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) =>  _mediator = mediator;
    
    /// <summary>
    /// Informace o prihlašeném uživateli.
    /// </summary>
    /// <remarks>
    /// Pokud je uživatel přihlášen (existuje platná auth cookie), vrací základní informace o uživateli.
    /// </remarks>
    /// <returns>Instance přihlášeného uživatele.</returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(GetCurrentUser.GetCurrentUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public async Task<GetCurrentUser.GetCurrentUserResponse> GetCurrentUser(CancellationToken cancellationToken)
        => await _mediator.Send(new GetCurrentUser.GetCurrentUserRequest(), cancellationToken);
    
    /// <summary>
    /// Endpoint pro Simple Authentication provider.
    /// </summary>
    /// <remarks>
    /// Slouží pro vytvoření auth cookie v případe, že aplikace není napojena na CAAS.
    /// </remarks>
    [HttpPost("signin")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task SignIn([FromBody] SignIn.SignInRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    [HttpGet("mpss-cookie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<string> GetMpssSecurityCookie(CancellationToken cancellationToken)
        => await _mediator.Send(new GetMpssSecurityCookie.GetMpssSecurityCookieRequest(), cancellationToken);
}