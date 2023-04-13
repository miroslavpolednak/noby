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

    /// <summary>
    /// Získání obsahu auth cookie pro ePodpisy.
    /// </summary>
    /// <remarks>
    /// Endpoint vrací obsah MPSS.Security cookie, která slouží pro autentizaci ePodpisů na základě údajů získaných z UserService. Cookie je generovaná z projektu MPSS.Security.Noby, což je klon standardní MPSS.Security dll.
    /// </remarks>
    /// <returns>BASE64 obsah cookie, který se musí předat ePodpis widgetu</returns>
    [HttpGet("mpss-cookie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<string> GetMpssSecurityCookie(CancellationToken cancellationToken)
        => await _mediator.Send(new GetMpssSecurityCookie.GetMpssSecurityCookieRequest(), cancellationToken);
}