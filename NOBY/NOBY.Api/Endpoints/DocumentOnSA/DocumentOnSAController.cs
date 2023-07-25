using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;
using Swashbuckle.AspNetCore.Annotations;
using NOBY.Api.Endpoints.DocumentOnSA.StartSigning;
using NOBY.Api.Endpoints.DocumentOnSA.StopSigning;
using NOBY.Api.Endpoints.DocumentOnSA.SignDocumentManually;
using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAData;
using System.Net.Mime;
using NOBY.Api.Endpoints.DocumentOnSA.Search;
using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSADetail;
using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAPreview;
using NOBY.Api.Endpoints.DocumentOnSA.SendDocumentPreview;

namespace NOBY.Api.Endpoints.DocumentOnSA;

[ApiController]
[Route("api")]
public class DocumentOnSAController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentOnSAController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Dokumenty Sales Arrangement-u k podpisu / v podepisovacím procesu.
    /// </summary>
    /// <remarks>
    ///Provolá <i>DS:DocumentOnSAService/getDocumentsToSignList</i> a zafiltruje na dokumenty s příznakem IsValid = true.<br /><br />
    ///Stav podepisovacího procesu (signatureState) je vyhodnocován logikou popsanou v společných algoritmech viz <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=401893151">Stavy podepisovacího procesu</a> 
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    [HttpGet("sales-arrangement/{salesArrangementId}/signing/document-list")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Podepisování" })]
    [ProducesResponseType(typeof(GetDocumentsSignListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetDocumentsSignListResponse> GetDocumentsSignList(
        [FromRoute] int salesArrangementId,
        CancellationToken cancellationToken)
    => await _mediator.Send(new GetDocumentsSignListRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Začít podepisování dokumentu
    /// </summary>
    /// <remarks>
    /// Provede checkform pro vybrané žádosti a spustí podepisovací proces pro zvolený typ dokumentu.<br /><br />
    /// <i>DS:DocumentOnSAService/startSigning</i>
    /// </remarks>
    /// <param name="salesArrangementId"> ID Sales Arrangement </param>
    [HttpPost("sales-arrangement/{salesArrangementId}/signing/start")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Podepisování" })]
    [ProducesResponseType(typeof(StartSigningResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<StartSigningResponse> StartSigning(
         [FromRoute] int salesArrangementId,
         [FromBody] StartSigningRequest request)
    => await _mediator.Send(request.InfuseSalesArrangementId(salesArrangementId));

    /// <summary>
    /// Odeslání náhledu klientovi
    /// </summary>
    /// <remarks>
    /// Odešle klientovi náhled podepisovaného dokumentu v případě elektronického podepisování.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=A81B1C2A-B1DF-49da-8048-C574DFACA5DB"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("sales-arrangement/{salesArrangementId}/signing/{documentOnSAId}/send-document-preview")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = new[] { "Podepisování" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task SendDocumentOnSAPreview([FromRoute] int salesArrangementId, [FromRoute] int documentOnSAId)
        => await _mediator.Send(new SendDocumentOnSAPreviewRequest(salesArrangementId, documentOnSAId));
    
    /// <summary>
    /// Zrušit podepisování dokumentu
    /// </summary>
    /// <remarks>
    /// Zkontroluje, zda je pro daný dokument zahájen podepisovací proces a pokud ano, tak ho ukončí.<br /><br />
    /// <i>DS:DocumentOnSAService/getDocumentsToSignList</i><br />
    /// <i>DS:DocumentOnSAService/stopDocumentOnSASigning</i>
    /// </remarks>
    /// <param name="salesArrangementId"></param>
    /// <param name="documentOnSAId"></param>
    [HttpPost("sales-arrangement/{salesArrangementId}/signing/{documentOnSAId}/stop")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = new[] { "Podepisování" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task StopSigning(
        [FromRoute] int salesArrangementId,
        [FromRoute] int documentOnSAId)
    => await _mediator.Send(new StopSigningRequest(salesArrangementId, documentOnSAId));

    /// <summary>
    /// Vygenerování náhledu rozpodepsaného dokumentu
    /// </summary>
    /// <remarks>
    /// Dle zdroje podepisovaného dokumentu, typu podepisování a stavu podepisování dojde k vrácení PDF pro náhled dokumentu.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=A1B54B66-9AF8-4e5c-A240-93FEF635449F"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("sales-arrangement/{salesArrangementId}/document-on-sa/{documentOnSaId}/preview")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Pdf)]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocumentOnSAPreview(
        [FromRoute] int salesArrangementId,
        [FromRoute] int documentOnSAId,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new GetDocumentOnSAPreviewRequest(salesArrangementId, documentOnSAId), cancellationToken);
        return File(response.FileData, response.ContentType, response.Filename);
    }

    /// <summary>
    /// Manuální podpis DocumentOnSA
    /// </summary>
    /// <remarks>
    /// Označí daný DocumentOnSA jako manuálně podepsaný a provede aktualizaci dat v KB CM.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=FB2ED39E-233F-4b4c-A855-12CA1AC3A0B9"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("sales-arrangement/{salesArrangementId}/document-on-sa/{documentOnSAId}/sign-manually")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = new[] { "Sales Arrangement" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task SignDocumentManually(
        [FromRoute] int salesArrangementId,
        [FromRoute] int documentOnSAId)
     => await _mediator.Send(new SignDocumentManuallyRequest(salesArrangementId, documentOnSAId));

    /// <summary>
    /// Stažení dokumentu k fyzickému podepisování
    /// </summary>
    /// <remarks>
    /// Pro podepisovací procesy, které jsou validní, poskytuje dokument ke stažení pro fyzické podepsání.<br /><br />
    /// <a href = "https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=F950B198-2C67-48e5-B1FE-C091131E6A63"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId"></param>
    /// <param name="documentOnSAId"></param>
    [HttpGet("document/template/sales-arrangement/{salesArrangementId}/document-on-sa/{documentOnSAId}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Pdf)]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocumentOnSa(
       [FromRoute] int salesArrangementId,
       [FromRoute] int documentOnSAId,
       CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetDocumentOnSADataRequest(salesArrangementId, documentOnSAId), cancellationToken);
        return File(response.FileData, response.ContentType, response.Filename);
    }

    /// <summary>
    /// Detail podepisovaného dokumentu
    /// </summary>
    /// <remarks>
    /// Vrátí informace o podepisovaném dokumentu.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=FA259E3F-5B8D-4ade-9F1A-7C1A943F1029"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId"></param>
    /// <param name="documentOnSAId"></param>
    [HttpGet("sales-arrangement/{salesArrangementId}/signing/{documentOnSAId}")]
    [SwaggerOperation(Tags = new[] { "Podepisování" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetDocumentOnSADetailResponse> GetDocumentOnSaDetail(
        [FromRoute] int salesArrangementId,
        [FromRoute] int documentOnSAId,
        CancellationToken cancellationToken)
    => await _mediator.Send(new GetDocumentOnSADetailRequest(salesArrangementId, documentOnSAId), cancellationToken);

    /// <summary>
    /// Vyhledání dokumentů na základě hlavního hesla (eArchivu)
    /// </summary>
    /// <remarks>
    /// Vyhledání formId (businessovém identifikátoru dokumentů) na sales arrangementu dle hlavního hesla (eArchivu).<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=0C28E9E5-7AC1-4265-8342-FCE63B33967F"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a><br /><br />
    /// </remarks>
    /// <param name="salesArrangementId"></param>
    /// <param name="request"></param>
    [HttpPost("sales-arrangement/{salesArrangementId}/document-on-sa/search")]
    [SwaggerOperation(Tags = new[] { "Podepisování" })]
    [ProducesResponseType(typeof(SearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchDocumentsOnSa(
             [FromRoute] int salesArrangementId,
             [FromBody] SearchRequest request)
    {
        var result = await _mediator.Send(request.InfuseSalesArrangementId(salesArrangementId));

        if (result.FormIds is null || !result.FormIds.Any())
            return NoContent();

        return Ok(result);
    }



}
