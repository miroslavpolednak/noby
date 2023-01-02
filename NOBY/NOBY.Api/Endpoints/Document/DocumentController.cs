using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using CIS.Foms.Enums;
using NOBY.Api.Endpoints.Document.Shared;

namespace NOBY.Api.Endpoints.Document;

[ApiController]
[Route("api/document")]
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
    /// Vygenerování dokumentu kalkulace ze šablony
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu kalkulace ze šablony k dané Offer.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=5BA041DC-7D58-4d1d-8E00-DFD8C42B2B4C"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20"/>Diagram v EA</a>
    /// </remarks>
    /// <param name="offerId">Offer ID</param>
    [HttpGet("template/calculation/offer/{offerId:int}")]
    [SwaggerOperation(Tags = new[] { "Document" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), 200)]
    public async Task<IActionResult> GetCalculation(int offerId, CancellationToken cancellationToken)
    {
        var request = await _documentManager.CreateRequest<Calculation.GetCalculationRequest>(DocumentTemplateType.KALKULHU, _documentManager.GetOfferInput(offerId), cancellationToken);

        var memory = await _mediator.Send(request, cancellationToken);

        return await File(request, memory, cancellationToken);
    }

    /// <summary>
    /// Vygenerování dokumentu splátkový kalendář ze šablony
    /// </summary>
    /// <remarks>
    ///Vygenerování dokumentu splátkový kalendář ze šablony k dané Offer.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=67D55B92-E47A-47ab-8BEC-AE377E5AA56F"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20"/>Diagram v EA</a>
    /// </remarks>
    /// <param name="offerId">Offer ID</param>
    [HttpGet("template/payment-schedule/offer/{offerId:int}")]
    [SwaggerOperation(Tags = new[] { "Document" })]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileResult), 200)]
    public async Task<IActionResult> GetPaymentSchedule(int offerId, CancellationToken cancellationToken)
    {
        var request = await _documentManager.CreateRequest<PaymentSchedule.GetPaymentScheduleRequest>(DocumentTemplateType.SPLKALHU, _documentManager.GetOfferInput(offerId), cancellationToken);

        var memory = await _mediator.Send(request, cancellationToken);

        return await File(request, memory, cancellationToken);
    }

    private async Task<FileContentResult> File(GetDocumentBaseRequest baseRequest, ReadOnlyMemory<byte> memory, CancellationToken cancellationToken)
    {
        var fileName = await _documentManager.GetFileName(baseRequest, cancellationToken);

        return File(_documentManager.GetByteArray(memory), MediaTypeNames.Application.Pdf, fileName);
    }
}