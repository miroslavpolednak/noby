﻿using Swashbuckle.AspNetCore.Annotations;
using CIS.InternalServices.DataAggregatorService.Contracts;
using Asp.Versioning;
using NOBY.Api.Endpoints.Document.SharedDto;

namespace NOBY.Api.Endpoints.Document;

[ApiController]
[Route("api/v{v:apiVersion}/document")]
[ApiVersion(1)]
public class DocumentController(DocumentManager _documentManager, IMediator _mediator) 
    : ControllerBase
{
    /// <summary>
    /// Vygenerování dokumentu nabídka ze šablony
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu nabídka ze šablony k dané SalesArrangement. Pokud dokument pro tuto nabídku byl již vygenerován a byl uložen do eArchiv-u, tak je dokument načten z eArchiv-u.<br />Výstup je bez vodoznaku<br/><br />
    /// </remarks>
    /// <param name="salesArrangementId">Sales Arrangement ID</param>
    [HttpGet("offer/sales-arrangement/{salesArrangementId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Dokument"])]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=01EE50D6-556E-47e8-ADD8-673A844864C2")]
    public async Task<IActionResult> GetOffer(int salesArrangementId, CancellationToken cancellationToken)
    {
        var request = new GetOffer.GetOfferRequest
        {
            DocumentType = DocumentTypes.NABIDKA,
            InputParameters = _documentManager.GetSalesArrangementInput(salesArrangementId),
            ForPreview = false
        };

        var memory = await _mediator.Send(request, cancellationToken);

        return await File(request, memory, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu kalkulace ze šablony
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu kalkulace ze šablony k dané Offer.<br />Výstup je bez vodoznaku<br /><br />
    /// </remarks>
    /// <param name="offerId">Offer ID</param>
    [HttpGet("calculation/offer/{offerId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Dokument"])]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=5BA041DC-7D58-4d1d-8E00-DFD8C42B2B4C")]
    public Task<IActionResult> GetCalculation(int offerId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetOfferInput(offerId);

        return GenerateGeneralDocument(DocumentTypes.KALKULHU, input, false, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu splátkový kalendář ze šablony
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu splátkový kalendář ze šablony k dané Offer.<br />Výstup je bez vodoznaku<br /><br />
    /// </remarks>
    /// <param name="offerId">Offer ID</param>
    [HttpGet("payment-schedule/offer/{offerId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Dokument"])]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=67D55B92-E47A-47ab-8BEC-AE377E5AA56F")]
    public Task<IActionResult> GetPaymentSchedule(int offerId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetOfferInput(offerId);

        return GenerateGeneralDocument(DocumentTypes.SPLKALHU, input, false, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu ukončení žádosti o úvěr
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu ukončení žádosti o úvěr ze šablony podle CustomerOnSAId na vstupu. <br /><br />
    /// </remarks>
    /// <param name="customerOnSAId">Customer on SA ID</param>
    [HttpGet("cancel-confirmation/customer-on-sa/{customerOnSAId}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Dokument"])]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Obsolete("Odstranit casem?")]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=91D71957-A737-4ee8-9EFC-A3B62878153C")]
    public async Task<IActionResult> GetCancelConfirmationDocument([FromRoute] int customerOnSAId, CancellationToken cancellationToken)
    {
        var request = new CancelConfirmation.GetSalesArrangementCancelConfirmationRequest
        {
            DocumentType = DocumentTypes.ODSTOUP,
            InputParameters = new InputParameters
            {
                CustomerOnSaId = customerOnSAId
            },
            ForPreview = false
        };
        
        var memory = await _mediator.Send(request, cancellationToken);

        return await File(request, memory, cancellationToken);
    }
    
    /// <summary>
    /// Vygenerování náhledu dokumentu
    /// </summary>
    /// <remarks>
    /// Vrací se steam binárních dat.<br /><br />
    /// </remarks>
    [SwaggerOperation(Tags = ["Dokument"])]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("sales-arrangement/{salesArrangementId:int}/document-type/{documentTypeId:int}/preview")]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=258EEA87-9394-42ec-B51F-C13F091686E0")]
    public Task<IActionResult> GetDocumentPreview(int salesArrangementId, int documentTypeId, [FromQuery] int? customerOnSaId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetSalesArrangementInput(salesArrangementId, customerOnSaId);

        return GenerateGeneralDocument((DocumentTypes)documentTypeId, input, cancellationToken);
    }

    private Task<IActionResult> GenerateGeneralDocument(DocumentTypes documentType, InputParameters inputParameters, CancellationToken cancellationToken)
    {
        return GenerateGeneralDocument(documentType, inputParameters, true, cancellationToken);
    }

    private async Task<IActionResult> GenerateGeneralDocument(DocumentTypes documentType, InputParameters inputParameters, bool forPreview, CancellationToken cancellationToken)
    {
        var request = new GeneralDocument.GetGeneralDocumentRequest
        {
            DocumentType = documentType,
            ForPreview = forPreview,
            InputParameters = inputParameters
        };

        var memory = await _mediator.Send(request, cancellationToken);

        return await File(request, memory, cancellationToken);
    }

    private async Task<FileContentResult> File(GetDocumentBaseRequest baseRequest, ReadOnlyMemory<byte> memory, CancellationToken cancellationToken)
    {
        var fileName = await _documentManager.GetFileName(baseRequest, cancellationToken);

        return File(DocumentManager.GetByteArray(memory), MediaTypeNames.Application.Pdf, fileName);
    }
}