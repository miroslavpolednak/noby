using Asp.Versioning;
using NOBY.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.RealEstateValuation;

[ApiController]
[Route("api/v{v:apiVersion}/case")]
[ApiVersion(1)]
public sealed class RealEstateValuationController(IMediator _mediator)
        : ControllerBase
{
    /// <summary>
    /// Uložení detailů pro online ocenění
    /// </summary>
    /// <remarks>
    /// Průběžné uložení detailů nutných k online ocenění pro pozdější použití.
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/online-preorder-details")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=526BB471-CC8D-4bad-8B6C-E0DC6F4872C1")]
    public async Task<IActionResult> SaveOnlinePreorderDetails(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] RealEstateValuationSaveOnlinePreorderDetailsRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Uložení dat pro místní šetření
    /// </summary>
    /// <remarks>
    /// Průběžné uložení konktaktů pro místní šetření pro pozdější použití.
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/local-survey-data")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=C6362E7C-7CC7-4b1c-86BA-D08BD84FFCA7")]
    public async Task<IActionResult> SaveLocalSurveyDetails(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] RealEstateValuationSaveLocalSurveyDetailsRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Nastavení typu Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Nastavení typu Ocenění. Slouží při možnosti uživatele vybrat typ Ocenění ze Standardu a DTS nebo při přechodu uživatele z Online ocenění na Standard.
    /// </remarks>
    [HttpPut("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/valuation-type-id")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano, RealEstateValuationStates.DoplneniDokumentu)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=740D6F1C-FF1E-437e-96BD-01C6553C0F9A")]
    public async Task<IActionResult> SetValuationTypeId(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] SetValuationTypeId.SetValuationTypeIdRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Založení Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Operace založí nové Ocenění nemovitosti k danému case ID. Jde buď o založení Ocenění nemovitosti k objektu úvěru ze žádosti o úvěr nebo o manuální založení dalšího objektu zajištění.
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations")]
    [NobyAuthorizePreload(NobyAuthorizePreloadAttribute.LoadableEntities.Case)]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("text/plain")]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=5A275F67-67AC-4851-8FE9-51F2B685B990")]
    public async Task<IActionResult> CreateRealEstateValuation(
        [FromRoute] long caseId, 
        [Required] [FromBody] RealEstateValuationCreateRealEstateValuationRequest request, 
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(request.InfuseId(caseId), cancellationToken);
        return Content(id.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }
    
    /// <summary>
    /// Smazání Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Smazání Ocenění konkrétní nemovitosti ještě před odesláním do ACV.
    /// </remarks>
    [HttpDelete("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=6525504B-D598-4113-AA2A-846571769C40")]
    public async Task<IActionResult> DeleteRealEstateValuation(
        [FromRoute] long caseId, 
        [FromRoute] int realEstateValuationId, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteRealEstateValuation.DeleteRealEstateValuationRequest(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Získání detailu Ocenění nemovitostí
    /// </summary>
    /// <remarks>
    /// Operace vrací detail Ocenění nemovitostí.
    /// </remarks>
    [HttpGet("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(typeof(RealEstateValuationGetRealEstateValuationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=7301EE13-E1C2-4795-A5FA-F8A646C4D057")]
    public async Task<RealEstateValuationGetRealEstateValuationDetailResponse> GetRealEstateValuationDetail(
        long caseId, 
        int realEstateValuationId, 
        CancellationToken cancellationToken) 
        => await _mediator.Send(new GetRealEstateValuationDetail.GetRealEstateValuationDetailRequest(caseId, realEstateValuationId), cancellationToken);

    /// <summary>
    /// Získání seznamu Ocenění nemovitostí
    /// </summary>
    /// <remarks>
    /// Operace vrací seznam všech ocenění nemovitostí k danému case ID. Seznam je sloučením ocenění objektů úvěru ze žádosti o úvěr a manuálně zadaných dalších objektů zajištění. Operace nevrací objednávky ocenění, které nevznikly v Noby.
    /// </remarks>
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [HttpGet("{caseId:long}/real-estate-valuations")]
    [NobyAuthorizePreload(NobyAuthorizePreloadAttribute.LoadableEntities.Case)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(typeof(List<RealEstateValuationSharedRealEstateValuationListItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=0DCD023D-ACF8-4744-B198-FE4FC2A84223")]
    public async Task<List<RealEstateValuationSharedRealEstateValuationListItem>> GetRealEstateValuationList(
        [FromRoute] long caseId, 
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetRealEstateValuationList.GetRealEstateValuationListRequest(caseId), cancellationToken);

    /// <summary>
    /// Aktualizace detailu Ocenění nemovitostí
    /// </summary>
    /// <remarks>
    /// Operace edituje detail Ocenění nemovitostí.
    /// </remarks>
    [HttpPut("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=39883A18-AA29-4f7d-9E4E-BC2D5F81B115")]
    public async Task<IActionResult> UpdateRealEstateValuationDetail(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [Required] [FromBody] RealEstateValuationUpdateRealEstateValuationDetailRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Patch developera Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Patch toggle developera na Ocenění konkrétní nemovitosti ještě před odesláním do ACV.
    /// </remarks>
    [HttpPatch("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/developer")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=2BD3A207-7DFB-4c5c-B81C-95E99C2D0C58")]
    public async Task<IActionResult> PatchDeveloperOnRealEstateValuation(
        [FromRoute] long caseId, 
        [FromRoute] int realEstateValuationId, 
        [Required] [FromBody] RealEstateValuationPatchDeveloperOnRealEstateValuationRequest request, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Smazání již připojeného dokumentu LV
    /// </summary>
    /// <remarks>
    /// Smazání/odpojení již připojeného dokumentu listu vlastnictví (LV) od existujícího Ocenění nemovitosti.
    /// </remarks>
    [HttpDelete("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/deed-of-ownership-documents/{deedOfOwnershipDocumentId:int}")]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano)]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=33180E91-2CB3-4d2f-B6AD-5841EC8A836F")]
    public async Task<IActionResult> DeleteDeedOfOwnershipDocument(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromRoute] int deedOfOwnershipDocumentId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteDeedOfOwnershipDocument.DeleteDeedOfOwnershipDocumentRequest(caseId, realEstateValuationId, deedOfOwnershipDocumentId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Spojení souboru přílohy s oceněním nemovitosti
    /// </summary>
    /// <remarks>
    /// Propojí uploadnutý soubor s oceněním a doplní k souboru popisek.
    /// </remarks>
    /// <param name="attachments">Seznam souborů k propojení</param>
    /// <response code="200">Kolekce ID uploadovaných souborů vs. nových ID příloh</response>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/attachments")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano, RealEstateValuationStates.DoplneniDokumentu)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(typeof(List<RealEstateValuationSaveRealEstateValuationAttachmentsResponseItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=B44C60EF-8521-4eec-9CE5-292C279DFE51")]
    public async Task<List<RealEstateValuationSaveRealEstateValuationAttachmentsResponseItem>> SaveRealEstateValuationAttachments(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] List<RealEstateValuationSaveRealEstateValuationAttachmentsRequestItem> attachments,
        CancellationToken cancellationToken)
        => await _mediator.Send(new SaveRealEstateValuationAttachments.SaveRealEstateValuationAttachmentsRequest
        {
            CaseId = caseId,
            RealEstateValuationId = realEstateValuationId,
            Attachments = attachments,
        }, cancellationToken);

    /// <summary>
    /// Smazání přílohy ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Smazání již nahrané přílohy ocenění z databáze NOBY a ACV.
    /// </remarks>
    [HttpDelete("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/attachments/{realEstateValuationAttachmentId:int}")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano, RealEstateValuationStates.DoplneniDokumentu)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=4EFAC9BD-B78E-4219-A801-39E983D3EDAF")]
    public async Task<IActionResult> DeleteRealEstateValuationAttachment(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromRoute] int realEstateValuationAttachmentId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteRealEstateValuationAttachment.DeleteRealEstateValuationAttachmentRequest(caseId, realEstateValuationId, realEstateValuationAttachmentId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Uložení údajů identifikace nemovitosti k danému Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Uložení údajů identifikace nemovitosti k danému Ocenění nemovitosti.
    /// </remarks>
    /// <response code="200">DeedOfOwnershipDocumentId: Noby ID daného záznamu. Určuje jednoznačnou kombinaci cremDeedOfOwnershipDocumentId a RealEstateValuationId (Noby Ocenění) pro případy simulování více možností žádostí s jednou nemovitostí.</response>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/deed-of-ownership-documents")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Neoceneno, RealEstateValuationStates.Rozpracovano)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=536CD827-3140-4a41-8AC2-AF6BB6700539")]
    public async Task<int> AddDeedOfOwnershipDocument(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] [Required] RealEstateValuationAddDeedOfOwnershipDocumentRequest request,
        CancellationToken cancellationToken)
        => await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);

    /// <summary>
    /// Získání typu Ocenění
    /// </summary>
    /// <remarks>
    /// Získání typu Ocenění provoláním systému ACV.
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/valuation-types")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=0FE0440C-1614-47b0-8136-42BF508CE369")]
    public async Task<List<int>> GetRealEstateValuationTypes(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetRealEstateValuationTypes.GetRealEstateValuationTypesRequest(caseId, realEstateValuationId), cancellationToken);

    /// <summary>
    /// Aktualizace připojeného dokumentu LV
    /// </summary>
    /// <remarks>
    /// Operace edituje RealEstateIds na již připojeném dokumentu LV.
    /// </remarks>
    [HttpPut("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/deed-of-ownership-documents/{deedOfOwnershipDocumentId:int}")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=22C309A5-60C5-4ca2-96DE-129CA8178977")]
    public async Task<IActionResult> UpdateDeedOfOwnershipDocument(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromRoute] int deedOfOwnershipDocumentId,
        [FromBody] RealEstateValuationUpdateDeedOfOwnershipDocumentRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId, deedOfOwnershipDocumentId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Předobjednávka online ocenění
    /// </summary>
    /// <remarks>
    /// Předobjednávka online ocenění.
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/preorder-online")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano)]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=CA6B233C-6BE0-45ff-B5F6-F47F9A3ABA62")]
    public async Task<IActionResult> PreorderOnlineValuation(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] [Required] RealEstateValuationPreorderOnlineValuationRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Objednání ocenění
    /// </summary>
    /// <remarks>
    /// Objedná ocenění nemovitosti daného typu.
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/order")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation]
    [SwaggerOperation(Tags = ["Real Estate Valuation"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=68C49245-60DD-48a4-9681-B328C52D86F4")]
    public async Task<IActionResult> OrderRealEstateValuation(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] [Required] RealEstateValuationOrderRealEstateValuationRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }
}
