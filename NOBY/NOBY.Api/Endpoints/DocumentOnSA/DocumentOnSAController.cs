﻿using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;
using Swashbuckle.AspNetCore.Annotations;
using NOBY.Api.Endpoints.DocumentOnSA.StopSigning;
using NOBY.Api.Endpoints.DocumentOnSA.SignDocumentManually;
using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSA;
using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSADetail;
using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAPreview;
using NOBY.Api.Endpoints.DocumentOnSA.RefreshElectronicDocument;
using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAStatus;
using Asp.Versioning;
using NOBY.Api.Endpoints.DocumentOnSA.SendDocumentOnSAPreview;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

namespace NOBY.Api.Endpoints.DocumentOnSA;

[ApiController]
[Route("api/v{v:apiVersion}")]
[ApiVersion(1)]
public class DocumentOnSAController(IMediator _mediator) 
    : ControllerBase
{
    /// <summary>
    /// Dokumenty Sales Arrangement-u k podpisu / v podepisovacím procesu.
    /// </summary>
    /// <remarks>
    ///Vrací seznam rozpodepsaných a podepsaných dokumentů.<br /><br />
    ///Řadí dokumenty primárně podle documentTypeId a sekundárně podle customerOnSAId.
    /// </remarks>
    /// <param name="salesArrangementId">ID Sales Arrangement</param>
    [HttpGet("sales-arrangement/{salesArrangementId}/signing/document-list")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [ProducesResponseType(typeof(DocumentOnSAGetDocumentsSignListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=294380C5-4F9D-4dce-9D32-C1AD842ED84B")]
    public async Task<DocumentOnSAGetDocumentsSignListResponse> GetDocumentsSignList(
        [FromRoute] int salesArrangementId,
        CancellationToken cancellationToken)
    => await _mediator.Send(new GetDocumentsSignListRequest(salesArrangementId), cancellationToken);

    /// <summary>
    /// Začít podepisování dokumentu
    /// </summary>
    /// <remarks>
    /// Provede checkform pro vybrané žádosti a spustí podepisovací proces pro zvolený typ dokumentu.
    /// </remarks>
    /// <param name="salesArrangementId"> ID Sales Arrangement </param>
    [HttpPost("sales-arrangement/{salesArrangementId}/signing/start")]
    [NobyAuthorize(UserPermissions.DOCUMENT_SIGNING_Manage, UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [ProducesResponseType(typeof(DocumentOnSaStartSigningResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=C65CE06A-6702-4d2d-ADCF-A8DC7F1902A4")]
    public async Task<DocumentOnSaStartSigningResponse> StartSigning(
         [FromRoute] int salesArrangementId,
         [FromBody] DocumentOnSAStartSigningRequest request)
    => await _mediator.Send(request.InfuseSalesArrangementId(salesArrangementId));

    /// <summary>
    /// Odeslání náhledu klientovi
    /// </summary>
    /// <remarks>
    /// Odešle klientovi náhled podepisovaného dokumentu v případě elektronického podepisování
    /// </remarks>
    [HttpPost("sales-arrangement/{salesArrangementId}/signing/{documentOnSAId}/send-document-preview")]
    [NobySkipCaseStateAndProductSAValidation]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=A81B1C2A-B1DF-49da-8048-C574DFACA5DB")]
    public async Task<IActionResult> SendDocumentOnSAPreview([FromRoute] int salesArrangementId, [FromRoute] int documentOnSAId)
    {
        await _mediator.Send(new SendDocumentOnSAPreviewRequest(salesArrangementId, documentOnSAId));
        return NoContent();
    }
        

    /// <summary>
    /// Zrušit podepisování dokumentu
    /// </summary>
    /// <remarks>
    /// Zkontroluje, zda je pro daný dokument zahájen podepisovací proces a pokud ano, tak ho ukončí.
    /// </remarks>
    /// <param name="salesArrangementId"></param>
    /// <param name="documentOnSAId"></param>
    [HttpPost("sales-arrangement/{salesArrangementId}/signing/{documentOnSAId}/stop")]
    [NobySkipCaseStateAndProductSAValidation]
    [NobyAuthorize(UserPermissions.DOCUMENT_SIGNING_Manage, UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=AA714DE2-ACD8-48ed-B146-41D50B7D3BE1")]
    public async Task<IActionResult> StopSigning(
        [FromRoute] int salesArrangementId,
        [FromRoute] int documentOnSAId)
    {
        await _mediator.Send(new StopSigningRequest(salesArrangementId, documentOnSAId));
        return NoContent();
    }
    

    /// <summary>
    /// Vygenerování náhledu rozpodepsaného dokumentu
    /// </summary>
    /// <remarks>
    /// Dle zdroje podepisovaného dokumentu, typu podepisování a stavu podepisování dojde k vrácení PDF pro náhled dokumentu.
    /// </remarks>
    [HttpGet("document/sales-arrangement/{salesArrangementId}/document-on-sa/{documentOnSAId}/preview")]
    [Produces(MediaTypeNames.Application.Pdf)]
    [NobySkipCaseStateAndProductSAValidation]
    [SwaggerOperation(Tags = ["Dokument"])]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=A1B54B66-9AF8-4e5c-A240-93FEF635449F")]
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
    /// Označí daný DocumentOnSA jako manuálně podepsaný a provede aktualizaci dat v KB CM.
    /// </remarks>
    [HttpPost("sales-arrangement/{salesArrangementId}/document-on-sa/{documentOnSAId}/sign-manually")]
    [NobySkipCaseStateAndProductSAValidation]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=FB2ED39E-233F-4b4c-A855-12CA1AC3A0B9")]
    public async Task<IActionResult> SignDocumentManually(
        [FromRoute] int salesArrangementId,
        [FromRoute] int documentOnSAId)
    {
        await _mediator.Send(new SignDocumentManuallyRequest(salesArrangementId, documentOnSAId));
        return NoContent();
    }
    

    /// <summary>
    /// Stažení dokumentu k fyzickému podepisování
    /// </summary>
    /// <remarks>
    /// Pro podepisovací procesy, které jsou validní, poskytuje dokument ke stažení pro fyzické podepsání (nelze použít pro Starbuild (WF) dokumenty).
    /// </remarks>
    [HttpGet("document/sales-arrangement/{salesArrangementId}/document-on-sa/{documentOnSAId}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Pdf)]
    [SwaggerOperation(Tags = ["Dokument"])]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=F950B198-2C67-48e5-B1FE-C091131E6A63")]
    public async Task<IActionResult> GetDocumentOnSA(
       [FromRoute] int salesArrangementId,
       [FromRoute] int documentOnSAId,
       CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetDocumentOnSARequest(salesArrangementId, documentOnSAId), cancellationToken);
        return File(response.FileData, response.ContentType, response.Filename);
    }

    /// <summary>
    /// Detail podepisovaného dokumentu
    /// </summary>
    /// <remarks>
    /// Vrátí informace o podepisovaném dokumentu.
    /// </remarks>
    [HttpGet("sales-arrangement/{salesArrangementId}/signing/{documentOnSAId}")]
    [NobySkipCaseStateAndProductSAValidation]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=FA259E3F-5B8D-4ade-9F1A-7C1A943F1029")]
    public async Task<DocumentOnSaGetDocumentOnSADetailResponse> GetDocumentOnSaDetail(
        [FromRoute] int salesArrangementId,
        [FromRoute] int documentOnSAId,
        CancellationToken cancellationToken)
    => await _mediator.Send(new GetDocumentOnSADetailRequest(salesArrangementId, documentOnSAId), cancellationToken);

    /// <summary>
    /// Vyhledání dokumentů na základě hlavního hesla (eArchivu)
    /// </summary>
    /// <remarks>
    /// Vyhledání formId (businessovém identifikátoru dokumentů) na sales arrangementu dle hlavního hesla (eArchivu).
    /// </remarks>
    [HttpPost("sales-arrangement/{salesArrangementId}/document-on-sa/search")]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(DocumentOnSaSearchDocumentsOnSaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=0C28E9E5-7AC1-4265-8342-FCE63B33967F")]
    public async Task<IActionResult> SearchDocumentsOnSa(
             [FromRoute] int salesArrangementId,
             [FromBody] DocumentOnSaSearchDocumentsOnSaRequest request,
             CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.InfuseSalesArrangementId(salesArrangementId), cancellationToken);

        if (result.FormIds?.Any() != true)
            return NoContent();

        return Ok(result);
    }
    /// <summary>
    /// Vyhledání dokumentů na všech žádostech daného Case dle EACode
    /// </summary>
    /// <remarks>
    /// Vyhledání formId (businessovém identifikátoru dokumentů) na všech sales arrangementech na Case dle hlavního hesla (eArchivu).
    /// </remarks>
    [HttpPost("case/{caseId:long}/document-on-sa/search")]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(DocumentOnSaSearchDocumentsOnSaOnCaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=85D55F65-BA7A-4d86-8E2B-523EA1918A0A")]
    public async Task<IActionResult> SearchDocumentsOnSaOnCase(
         [FromRoute] long caseId,
         [FromBody] DocumentOnSaSearchDocumentsOnSaOnCaseRequest request,
         CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request.InfuseCaseId(caseId), cancellationToken);
        if (result.FormIds?.Any() != true)
            return NoContent();

        return Ok(result);
    }

    /// <summary>
    /// Aktualizace stavu dokumentu
    /// </summary>
    /// <remarks>
    /// Pokud zjistí změnu stavu dokumentu v ePodpisech, dojde k provedení všech návazných akcí (podpis dokumentu, zrušení podepisování dokumentu a jiné).
    /// </remarks>
    [HttpPost("sales-arrangement/{salesArrangementId}/signing/{documentOnSaId}/refresh")]
    [NobySkipCaseStateAndProductSAValidation]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [ProducesResponseType(typeof(DocumentOnSaRefreshElectronicDocumentResponse), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=73549CB9-547D-4b15-BEED-BE2E563459D9")]
    public async Task<DocumentOnSaRefreshElectronicDocumentResponse> RefreshElectronicDocument(
          [FromRoute] int salesArrangementId,
          [FromRoute] int documentOnSaId,
          CancellationToken cancellationToken)
      => await _mediator.Send(new RefreshElectronicDocumentRequest(salesArrangementId, documentOnSaId), cancellationToken);

    /// <summary>
    /// Stav podepisovaného dokumentu 
    /// </summary>
    /// <remarks>
    /// Zjistí stav aktuálně podepisovaného dokumentu.
    /// </remarks>
    [HttpGet("sales-arrangement/{salesArrangementId}/signing/{documentOnSAId}/status")]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [NobySkipCaseStateAndProductSAValidation]
    [ProducesResponseType(typeof(DocumentOnSaGetDocumentOnSAStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=A3D2BB37-E844-4117-B316-A649C39D82F6")]
    public async Task<DocumentOnSaGetDocumentOnSAStatusResponse> GetDocumentOnSAStatus(
          [FromRoute] int salesArrangementId,
          [FromRoute] int documentOnSAId,
          CancellationToken cancellationToken
        )
        => await _mediator.Send(new GetDocumentOnSAStatusRequest(salesArrangementId, documentOnSAId), cancellationToken);
}
