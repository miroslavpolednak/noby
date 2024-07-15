using Asp.Versioning;
using NOBY.Api.Endpoints.GeneralDocument.GetGeneralDocuments;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.GeneralDocument;

[ApiController]
[Route("api/v{v:apiVersion}/general-documents")]
[ApiVersion(1)]
public class GeneralDocumentController : ControllerBase
{
    /// <summary>
    /// Seznam obecných dokumentů ke stažení
    /// </summary>
    /// <remarks>
    /// Načtení seznamu obecných dokumentů, které je možné stáhnout a vyplnit. <br /><br />
    /// </remarks>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Dokument"])]
    [ProducesResponseType(typeof(List<GeneralDocumentGetGeneralDocumentsDocument>), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=EA4655D8-5314-469b-8C92-5D2324EF1824")]
    public async Task<List<GeneralDocumentGetGeneralDocumentsDocument>> GetGeneralDocuments(CancellationToken cancellationToken)
        => await _mediator.Send(new GetGeneralDocumentsRequest(), cancellationToken);
    
    private readonly IMediator _mediator;
    
    public GeneralDocumentController(IMediator mediator) =>  _mediator = mediator;
}