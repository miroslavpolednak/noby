using NOBY.Api.Endpoints.GeneralDocument.GetGeneralDocuments;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.GeneralDocument;

[ApiController]
[Route("api/general-documents")]
public class GeneralDocumentController : ControllerBase
{
    /// <summary>
    /// Seznam obecných dokumentů ke stažení
    /// </summary>
    /// <remarks>
    /// Načtení seznamu obecných dokumentů, které je možné stáhnout a vyplnit. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&o=EA4655D8-5314-469b-8C92-5D2324EF1824"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [ProducesResponseType(typeof(List<GetGeneralDocuments.Document>), StatusCodes.Status200OK)]
    public async Task<List<GetGeneralDocuments.Document>> GetGeneralDocuments(CancellationToken cancellationToken)
        => await _mediator.Send(new GetGeneralDocumentsRequest(), cancellationToken);
    
    private readonly IMediator _mediator;
    
    public GeneralDocumentController(IMediator mediator) =>  _mediator = mediator;
}