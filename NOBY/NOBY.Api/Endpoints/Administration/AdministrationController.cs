using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Administration;

[ApiController]
[Route("api/v{v:apiVersion}/admin")]
[ApiVersion(1)]
public class AdministrationController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Seznam upozornění na úvodní obrazovce.
    /// </summary>
    /// <remarks>
    /// Načtení seznamu upozornění na úvodní obrazovce v rámci administrace těchto upozornění
    /// </remarks>
    [HttpGet("fe-banners")]
    //[NobyAuthorize(UserPermissions.ADMIN_FeBannersManage)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Administration"])]
    [ProducesResponseType(typeof(List<AdministrationFeBannerDetail>), StatusCodes.Status200OK)]
    public async Task<List<AdministrationFeBannerDetail>> GetAdminFeBanners()
        => await _mediator.Send(new GetAdminFeBanners.GetAdminFeBannersRequest());

    /// <summary>
    /// Vytvoření upozornění
    /// </summary>
    /// <remarks>
    /// Vytvoří nové upozornění na úvodní obrazovce.
    /// </remarks>
    [HttpPost("fe-banners")]
    //[NobyAuthorize(UserPermissions.ADMIN_FeBannersManage)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Administration"])]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<int> CreateAdminFeBanner([FromBody] AdminCreateAdminFeBannerRequest request)
        => await _mediator.Send(request);

    /// <summary>
    /// Smazání upozornění na úvodní obrazovce
    /// </summary>
    /// <remarks>
    /// Smaže upozornění.
    /// </remarks>
    [HttpDelete("fe-banners/{feBannerId:int}")]
    //[NobyAuthorize(UserPermissions.ADMIN_FeBannersManage)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Text.Plain)]
    [SwaggerOperation(Tags = ["Administration"])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task DeleteAdminFeBanner([FromRoute] int feBannerId)
        => await _mediator.Send(new DeleteAdminFeBanner.DeleteAdminFeBannerRequest(feBannerId));

    /// <summary>
    /// Detail upozornění na úvodní obrazovce.
    /// </summary>
    /// <remarks>
    /// Načtení detailu upozornění
    /// </remarks>
    [HttpGet("fe-banners/{feBannerId:int}")]
    //[NobyAuthorize(UserPermissions.ADMIN_FeBannersManage)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Administration"])]
    [ProducesResponseType(typeof(AdministrationFeBannerDetail), StatusCodes.Status200OK)]
    public async Task<AdministrationFeBannerDetail> GetAdminFeBanner([FromRoute] int feBannerId)
        => await _mediator.Send(new GetAdminFeBanner.GetAdminFeBannerRequest(feBannerId));

    /// <summary>
    /// Uložení upozornění
    /// </summary>
    /// <remarks>
    /// Ukládá změn v existujícím upozornění.
    /// </remarks>
    [HttpPut("fe-banners/{feBannerId:int}")]
    //[NobyAuthorize(UserPermissions.ADMIN_FeBannersManage)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Administration"])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task UpdateAdminFeBanner([FromRoute] int feBannerId, [FromBody] AdminUpdateAdminFeBannerRequest request)
        => await _mediator.Send(request.InfuseId(feBannerId));
}
