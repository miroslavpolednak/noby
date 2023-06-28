using Microsoft.AspNetCore.Authorization;

namespace NOBY.Api.Endpoints.Users;

[ApiController]
[Route("api")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) =>  _mediator = mediator;

    /// <summary>
    /// Informace a oprávnění o přihlášeném uživateli
    /// </summary>
    /// <returns>Instance přihlášeného uživatele.</returns>
    [HttpGet("users")]
    [ProducesResponseType(typeof(GetCurrentUser.GetCurrentUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [Obsolete]
    public async Task<GetCurrentUser.GetCurrentUserResponse> GetCurrentUser(CancellationToken cancellationToken)
        => await _mediator.Send(new GetCurrentUser.GetCurrentUserRequest(), cancellationToken);

    /// <summary>
    /// Informace a oprávnění o přihlášeném uživateli
    /// </summary>
    /// <remarks>
    /// Vrací o uživateli data z XXVVSS - základní údaje, identity (počítáme s tím, že je alespoň jedna, vetšinou více) a set oprávnění pro identitu, kterou se přihlásil. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=FC765415-4D2E-4b7c-B1C8-3F4B78A005DD"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <returns>Instance přihlášeného uživatele.</returns>
    [HttpGet("logged-in-user")]
    [ProducesResponseType(typeof(GetLoggedInUser.GetLoggedInUserResponse), StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<GetLoggedInUser.GetLoggedInUserResponse> GetLoggedInUser(CancellationToken cancellationToken)
        => await _mediator.Send(new GetLoggedInUser.GetLoggedInUserRequest(), cancellationToken);

    /// <summary>
    /// Endpoint pro Simple Authentication provider.
    /// </summary>
    /// <remarks>
    /// Slouží pro vytvoření auth cookie v případe, že aplikace není napojena na CAAS.
    /// </remarks>
    [HttpPost("users/signin")]
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
    [HttpGet("users/mpss-cookie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<string> GetMpssSecurityCookie(CancellationToken cancellationToken)
        => await _mediator.Send(new GetMpssSecurityCookie.GetMpssSecurityCookieRequest(), cancellationToken);
}