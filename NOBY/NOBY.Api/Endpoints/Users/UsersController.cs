using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using NOBY.Infrastructure.Swagger;

namespace NOBY.Api.Endpoints.Users;

[ApiController]
[Route("api/v{v:apiVersion}")]
[ApiVersion(1)]
public class UsersController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Informace a oprávnění o přihlášeném uživateli
    /// </summary>
    /// <remarks>
    /// Vrací o uživateli data z XXVVSS - základní údaje, identity (počítáme s tím, že je alespoň jedna, vetšinou více) a set oprávnění pro identitu, kterou se přihlásil. <br /><br />
    /// </remarks>
    /// <returns>Instance přihlášeného uživatele.</returns>
    [HttpGet("logged-in-user")]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea?m=1&o=FC765415-4D2E-4b7c-B1C8-3F4B78A005DD")]
    [ProducesResponseType(typeof(GetLoggedInUser.GetLoggedInUserResponse), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
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
    /// Odhlaseni uzivatele v pripade Simple Authentication provider
    /// </summary>
    /// /// <remarks>
    /// Slouží pro odsranění auth cookie v případe, že aplikace není napojena na CAAS.
    /// </remarks>
    [HttpGet("users/signout")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SignOut([FromServices] HttpContextAccessor context)
    {
        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
       
    
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

    /// <summary>
    /// Seznam upozornění na úvodní obrazovce
    /// </summary>
    /// <remarks>
    /// Vrátí kolekci upozornění pro uživatele seřazené dle závažnosti (závažnější dříve) a data konce viditelnosti. Vrátí se pouze prvních 5 upozornění.
    /// </remarks>
    [HttpGet("current-banners")]
    [SwaggerConfluenceLink("https://wiki.kb.cz/pages/viewpage.action?pageId=542682957")]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=85F351A3-3F81-4b9e-9F49-06B93B653AA0")]
    [ProducesResponseType(typeof(GetCurrentBannerList.GetCurrentBannerListResponse), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    [AllowAnonymous]
    public async Task<GetCurrentBannerList.GetCurrentBannerListResponse> GetCurrentBannerList(CancellationToken cancellationToken)
        => await _mediator.Send(new GetCurrentBannerList.GetCurrentBannerListRequest(), cancellationToken);
}