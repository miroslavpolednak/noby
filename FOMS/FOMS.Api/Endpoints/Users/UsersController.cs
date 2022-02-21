using Microsoft.AspNetCore.Authorization;

namespace FOMS.Api.Endpoints.Users;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) =>  _mediator = mediator;
    
    /// <summary>
    /// Informace o prihlasenem uzivateli.
    /// </summary>
    /// <remarks>
    /// Pokud je uzivatel prihlasen (existuje platna auth cookie), vraci zakladni informace o uzivateli.
    /// </remarks>
    /// <returns>Instance prihlaseneho uzivatele.</returns>
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
    /// Slouzi pro vytvoreni auth cookie v pripade, ze aplikace neni napojena na CAAS.
    /// </remarks>
    [HttpPost("signin")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task SignIn([FromBody] SignIn.SignInRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}