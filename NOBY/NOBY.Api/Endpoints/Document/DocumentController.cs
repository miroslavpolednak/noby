using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;
using NOBY.Api.Endpoints.Document.Shared;

namespace NOBY.Api.Endpoints.Document;

[ApiController]
[Route("api/document/template")]
public class DocumentController : ControllerBase
{
    private readonly DocumentManager _documentManager;
    private readonly IMediator _mediator;

    public DocumentController(DocumentManager documentManager, IMediator mediator)
    {
        _documentManager = documentManager;
        _mediator = mediator;
    }

    /// <summary>
    /// Vygenerování dokumentu nabídka ze šablony
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu nabídka ze šablony k dané SalesArrangement. Pokud dokument pro tuto nabídku byl již vygenerován a byl uložen do eArchiv-u, tak je dokument načten z eArchiv-u.<br />Výstup je bez vodoznaku<br/><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=01EE50D6-556E-47e8-ADD8-673A844864C2"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">Sales Arrangement ID</param>
    [HttpGet("offer/sales-arrangement/{salesArrangementId:int}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOffer(int salesArrangementId, CancellationToken cancellationToken)
    {
        var request = new Offer.GetOfferRequest
        {
            DocumentType = DocumentType.NABIDKA,
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
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=5BA041DC-7D58-4d1d-8E00-DFD8C42B2B4C"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="offerId">Offer ID</param>
    [HttpGet("calculation/offer/{offerId:int}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetCalculation(int offerId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetOfferInput(offerId);

        return GenerateGeneralDocument(DocumentType.KALKULHU, input, false, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu splátkový kalendář ze šablony
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu splátkový kalendář ze šablony k dané Offer.<br />Výstup je bez vodoznaku<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=67D55B92-E47A-47ab-8BEC-AE377E5AA56F"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="offerId">Offer ID</param>
    [HttpGet("payment-schedule/offer/{offerId:int}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetPaymentSchedule(int offerId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetOfferInput(offerId);

        return GenerateGeneralDocument(DocumentType.SPLKALHU, input, false, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu Žádost o čerpání
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu Žádost o čerpání ze šablony k dané SalesArrangement.<br />Slouží pro náhledy, výstup obsahuje vodoznak. <br/><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=FF4A4806-9638-4287-8A4F-4CA027677E2B"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20\" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">Sales Arrangement ID</param>
    [HttpGet("drawing/sales-arrangement/{salesArrangementId:int}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetDrawing(int salesArrangementId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetSalesArrangementInput(salesArrangementId);

        return GenerateGeneralDocument(DocumentType.ZADOCERP, input, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu Žádost o obecnou změnu
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu Žádost o obecnou změnu ze šablony k dané SalesArrangement.<br />Slouží pro náhledy, výstup obsahuje vodoznak. <br/><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=176277CE-66F6-4abd-93E6-57F113B5AF16"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">Sales Arrangement ID</param>
    [HttpGet("general-change/sales-arrangement/{salesArrangementId:int}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetGeneralChange(int salesArrangementId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetSalesArrangementInput(salesArrangementId);

        return GenerateGeneralDocument(DocumentType.ZAOZMPAR, input, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu Žádost o HUBN
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu Žádost o HUBN ze šablony k dané SalesArrangement.<br />Slouží pro náhledy, výstup obsahuje vodoznak. <br/><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=0E17AE8A-C137-415b-B4FB-2C0D3995E0DD"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">Sales Arrangement ID</param>
    [HttpGet("HUBN/sales-arrangement/{salesArrangementId:int}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetHUBN(int salesArrangementId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetSalesArrangementInput(salesArrangementId);

        return GenerateGeneralDocument(DocumentType.ZAODHUBN, input, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu Změna dlužníka (uvolnění/přistoupení/převzetí)
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu Změna dlužníka (uvolnění/přistoupení/převzetí) ze šablony k dané SalesArrangement.<br />Slouží pro náhledy, výstup obsahuje vodoznak.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=B7ED8950-BAA7-44a2-A069-2593B6D5121E"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("customer-change/sales-arrangement/{salesArrangementId:int}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetCustomerChange(int salesArrangementId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetSalesArrangementInput(salesArrangementId);

        return GenerateGeneralDocument(DocumentType.ZAOZMDLU, input, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu Žádost o úvěr F3601
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu Žádost o úvěr F3601 (dlužnická domácnost) ze šablony k dané SalesArrangement.<br />Slouží pro náhledy, výstup obsahuje vodoznak.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=DD734EDD-1344-43b2-B45E-3407255B993A"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">Sales Arrangement ID</param>
    [HttpGet("loan-application/main-household/sales-arrangement/{salesArrangementId:int}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetLoanApplicationMain(int salesArrangementId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetSalesArrangementInput(salesArrangementId);

        return GenerateGeneralDocument(DocumentType.ZADOSTHU, input, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu Žádost o úvěr F3602
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu Žádost o úvěr F3602 (spoludlužnická domácnost) k dané SalesArrangement.<br />Slouží pro náhledy, výstup obsahuje vodoznak.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=D5159266-11E9-4959-BDFA-71C1FCF46092"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="salesArrangementId">Sales Arrangement ID</param>
    [HttpGet("loan-application/codebtor-household/sales-arrangement/{salesArrangementId:int}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetLoanApplicationCodebtor(int salesArrangementId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetSalesArrangementInput(salesArrangementId);

        return GenerateGeneralDocument(DocumentType.ZADOSTHD, input, cancellationToken);
    }

    [Obsolete]
    [HttpGet("type/{documentTypeId:int}/sales-arrangement/{salesArrangementId:int}")]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> TestDocument(int documentTypeId, int salesArrangementId, CancellationToken cancellationToken)
    {
        var input = _documentManager.GetSalesArrangementInput(salesArrangementId);

        return GenerateGeneralDocument((DocumentType)documentTypeId, input, cancellationToken);
    }

    private Task<IActionResult> GenerateGeneralDocument(DocumentType documentType, InputParameters inputParameters, CancellationToken cancellationToken)
    {
        return GenerateGeneralDocument(documentType, inputParameters, false, cancellationToken);
    }

    private async Task<IActionResult> GenerateGeneralDocument(DocumentType documentType, InputParameters inputParameters, bool forPreview, CancellationToken cancellationToken)
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

        return File(_documentManager.GetByteArray(memory), MediaTypeNames.Application.Pdf, fileName);
    }
}