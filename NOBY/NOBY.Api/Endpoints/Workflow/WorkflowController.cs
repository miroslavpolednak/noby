using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Workflow;

[ApiController]
[Route("api/case")]
public class WorkflowController : ControllerBase
{
    private readonly IMediator _mediator;
    public WorkflowController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Získání relevantních typů konzultace
    /// </summary>
    /// <remarks>
    /// Získání typů konzultací, které jsou povolené pro daný typ procesu a danou fázi procesu.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=2B1DEBE7-BFDB-44f4-8733-DE0A3F7A994C"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="processId">Noby proces ID. Jde o ID sady úkolů generované Starbuildem.</param>
    [HttpGet("{caseId:long}/tasks/consultation-type")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Workflow Task" })]
    [ProducesResponseType(typeof(List<GetConsultationTypes.GetConsultationTypesResponseItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<GetConsultationTypes.GetConsultationTypesResponseItem>> GetConsultationTypes([FromRoute] long caseId, [FromQuery][Required] long processId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetConsultationTypes.GetConsultationTypesRequest(caseId, processId), cancellationToken);

    /// <summary>
    /// Stornování úkolu ve SB
    /// </summary>
    /// <remarks>
    /// Stornování úkolu ve SB. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=C77A111D-090F-410c-A1B2-B0E4E3EA59CF"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("{caseId:long}/tasks/{taskId:int}/cancel")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "Workflow Task" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTask([FromRoute] long caseId, [FromRoute] long taskId, [FromBody][Required] CancelTask.CancelTaskRequest request)
    {
        await _mediator.Send(request.InfuseId(caseId, taskId));
        return NoContent();
    }

    /// <summary>
    /// Vytvoření nového workflow tasku do SB.
    /// </summary>
    /// <remarks>
    /// Vytvoření nového workflow tasku do SB.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=882E85E3-F6EA-4774-812E-3328006E8893"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <response code="200">Noby task ID. Jde o ID sady úkolů generované Starbuildem.</response>
    /// <returns>Noby task ID. Jde o ID sady úkolů generované Starbuildem.</returns>
    [HttpPost("{caseId:long}/tasks")]
    [Consumes("application/json")]
    [Produces("text/plain")]
    [SwaggerOperation(Tags = new[] { "Workflow Task" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<long> CreateTask([FromRoute] long caseId, [FromBody][Required] CreateTask.CreateTaskRequest request)
        => await _mediator.Send(request.InfuseId(caseId));

    /// <summary>
    /// Seznam workflow tasků dotažený z SB.
    /// </summary>
    /// <remarks>
    /// Operace získá ze Starbuildu seznam úkolů a procesů k danému case ID. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=0460E3F9-7DE1-48e9-BAF6-CD5D1AC60F82"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <returns>Seznam wf tasks z SB.</returns>
    [HttpGet("{caseId:long}/tasks")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Workflow Task" })]
    [ProducesResponseType(typeof(GetTaskList.GetTaskListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetTaskList.GetTaskListResponse> GetTaskList([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetTaskList.GetTaskListRequest(caseId), cancellationToken);

    /// <summary>
    /// Detail workflow tasku dotažený ze SB.
    /// </summary>
    /// <remarks>
    /// Detail workflow tasku dotažený ze SB. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=90CD722E-8955-43e6-9924-DC5FDDF6ED15"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <returns></returns>
    [HttpGet("{caseId:long}/tasks/{taskId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Workflow Task" })]
    [ProducesResponseType(typeof(GetTaskDetail.GetTaskDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetTaskDetail.GetTaskDetailResponse> GetTaskDetail([FromRoute] long caseId, [FromRoute] long taskId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetTaskDetail.GetTaskDetailRequest(caseId, taskId), cancellationToken);

    /// <summary>
    /// Update workflow tasku do SB
    /// </summary>
    /// <remarks>
    /// Update workflow tasku do SB. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=D1B83124-CCEE-4a22-A82C-64F462BA3A9B"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <response code="404">Task or case not found</response>
    [HttpPut("{caseId:long}/tasks/{taskId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Workflow Task" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateTaskDetail([FromRoute] long caseId, [FromRoute] long taskId, [FromBody] UpdateTaskDetail.UpdateTaskDetailRequest? request, CancellationToken cancellationToken)
        => await _mediator.Send(request?.InfuseIds(caseId, taskId) ?? throw new NobyValidationException("Payload is empty"), cancellationToken);

    /// <summary>
    /// Získání aktuální cenové výjimky
    /// </summary>
    /// <remarks>
    /// Operace vrátí detail Cenové výjimky. <br /> 
    /// Pokud ještě ve Starbuild neexistuje workflow úkol, budou výsledkem data z Noby a uživatel dostane možnost založit workflow úkolu. <br /> 
    /// Pokud již workflow úkol existuje, ale je stornovaný, budou výsledkem data z Noby a uživatel dostane možnost založení nového workflow úkolu. <br /> 
    /// Pokud již workflow úkol existuje a není stornovaný, budou výsledkem data ze Starbuild. U dokončeného úkolu Ocenění nehraje roli, zda bylo Ocenění schváleno či zamítnuto. Uživatel uvidí detail tohoto ukončeného úkolu.
    /// </remarks>
    [HttpGet("{caseId:long}/tasks/current-price-exception")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Workflow Task" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetCurrentPriceException.GetCurrentPriceExceptionResponse> GetCurrentPriceException([FromRoute] long caseId, [FromQuery][Required] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCurrentPriceException.GetCurrentPriceExceptionRequest(caseId, salesArrangementId), cancellationToken);
}