using Asp.Versioning;
using NOBY.Api.Endpoints.Workflow.GetCurrentHandoverTask;
using NOBY.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NOBY.Api.Endpoints.Workflow;

[ApiController]
[Route("api/v{v:apiVersion}/case")]
[ApiVersion(1)]
public class WorkflowController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Získání relevantních typů konzultace
    /// </summary>
    /// <remarks>
    /// Získání typů konzultací, které jsou povolené pro daný typ procesu a danou fázi procesu.
    /// </remarks>
    /// <param name="processId">Noby proces ID. Jde o ID sady úkolů generované Starbuildem.</param>
    [HttpGet("{caseId:long}/tasks/consultation-type")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Workflow Task"])]
    [ProducesResponseType(typeof(List<WorkflowGetConsultationTypesResponseItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=2B1DEBE7-BFDB-44f4-8733-DE0A3F7A994C")]
    public async Task<List<WorkflowGetConsultationTypesResponseItem>> GetConsultationTypes([FromRoute] long caseId, [FromQuery][Required] long processId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetConsultationTypes.GetConsultationTypesRequest(caseId, processId), cancellationToken);

    /// <summary>
    /// Stornování úkolu ve SB
    /// </summary>
    /// <remarks>
    /// Stornování úkolu ve SB.
    /// </remarks>
    [HttpPost("{caseId:long}/tasks/{taskId:int}/cancel")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Workflow Task"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=C77A111D-090F-410c-A1B2-B0E4E3EA59CF")]
    public async Task<IActionResult> CancelTask([FromRoute] long caseId, [FromRoute] long taskId, [FromBody] WorkflowCancelTaskRequest? request)
    {
        await _mediator.Send((request ?? new WorkflowCancelTaskRequest()).InfuseId(caseId, taskId));
        return NoContent();
    }

    /// <summary>
    /// Začít podepisování dokumentu z workflow
    /// </summary>
    /// <remarks>
    /// Spustí podepisovací proces pro zvolený podúkol.
    /// </remarks>
    [HttpPost("{caseId:long}/tasks/{taskId:long}/signing/start")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Podepisování"])]
    [ProducesResponseType(typeof(WorkflowStartTaskSigningResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=7A23BD1E-668C-4ddc-BB82-550DC6C34C7F")]
    public async Task<WorkflowStartTaskSigningResponse> StartTaskSigning([FromRoute] long caseId, [FromRoute] long taskId)
        => await _mediator.Send(new StartTaskSigning.StartTaskSigningRequest(caseId, taskId));

    /// <summary>
    /// Vytvoření nového workflow tasku do SB.
    /// </summary>
    /// <remarks>
    /// Vytvoření nového workflow tasku do SB.
    /// </remarks>
    /// <response code="200">Noby task ID. Jde o ID sady úkolů generované Starbuildem.</response>
    /// <returns>Noby task ID. Jde o ID sady úkolů generované Starbuildem.</returns>
    [HttpPost("{caseId:long}/tasks")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("text/plain")]
    [SwaggerOperation(Tags = ["Workflow Task"])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=882E85E3-F6EA-4774-812E-3328006E8893")]
    public async Task<IActionResult> CreateTask([FromRoute] long caseId, [FromBody][Required] WorkflowCreateTaskRequest request)
        => Content((await _mediator.Send(request.InfuseId(caseId))).ToString(CultureInfo.InvariantCulture));

    /// <summary>
    /// Seznam workflow tasků dotažený z SB.
    /// </summary>
    /// <remarks>
    /// Operace získá ze Starbuildu seznam úkolů a procesů k danému case ID.
    /// </remarks>
    /// <returns>Seznam wf tasks z SB.</returns>
    [HttpGet("{caseId:long}/tasks")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Workflow Task"])]
    [ProducesResponseType(typeof(WorkflowGetTaskListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=0460E3F9-7DE1-48e9-BAF6-CD5D1AC60F82")]
    public async Task<WorkflowGetTaskListResponse> GetTaskList([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetTaskList.GetTaskListRequest(caseId), cancellationToken);

    /// <summary>
    /// Detail workflow tasku dotažený ze SB.
    /// </summary>
    /// <remarks>
    /// Detail workflow tasku dotažený ze SB.
    /// </remarks>
    /// <returns></returns>
    [HttpGet("{caseId:long}/tasks/{taskId:long}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Workflow Task"])]
    [ProducesResponseType(typeof(WorkflowGetTaskDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=90CD722E-8955-43e6-9924-DC5FDDF6ED15")]
    public async Task<WorkflowGetTaskDetailResponse> GetTaskDetail([FromRoute] long caseId, [FromRoute] long taskId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetTaskDetail.GetTaskDetailRequest(caseId, taskId), cancellationToken);

    /// <summary>
    /// Update workflow tasku do SB
    /// </summary>
    /// <remarks>
    /// Update workflow tasku do SB.
    /// </remarks>
    /// <response code="404">Task or case not found</response>
    [HttpPut("{caseId:long}/tasks/{taskId:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Workflow Task"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=D1B83124-CCEE-4a22-A82C-64F462BA3A9B")]
    public async Task UpdateTaskDetail([FromRoute] long caseId, [FromRoute] long taskId, [FromBody] WorkflowUpdateTaskDetailRequest? request, CancellationToken cancellationToken)
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
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Workflow Task"])]
    [ProducesResponseType(typeof(WorkflowGetCurrentPriceExceptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=F8C65CF5-6A02-4d2a-B068-C3EDE901DAE5")]
    public async Task<WorkflowGetCurrentPriceExceptionResponse> GetCurrentPriceException([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetCurrentPriceException.GetCurrentPriceExceptionRequest(caseId), cancellationToken);

    /// <summary>
    /// Získání aktuálního úkolu Předání na specialistu
    /// </summary>
    /// <remarks>
    /// Operace vrátí detail úkolu Předání na specialistu<br /> <br />
    /// Pokud ještě ve Starbuild neexistuje workflow úkol Předání na specialistu, bude zobrazen formulář pro založení úkolu a uživatel dostane možnost založit workflow úkol Předání na specialistu. <br /> <br />
    /// Pokud již workflow úkol ve Starbuildu existuje, ale je stornovaný nebo dokončený, bude zobrazen formulář pro založení úkolu a uživatel dostane možnost založení nového workflow úkolu Předání na specialistu. <br /> <br />
    /// Pokud již workflow úkol ve Starbuildu existuje a je ve stavu Odesláno, bude zobrazen read-only detail tohoto odeslaného úkolu.
    /// </remarks>
    [HttpGet("{caseId:long}/tasks/current-handover-task")]
    [ProducesResponseType(typeof(WorkflowGetCurrentHandoverTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Tags = ["Workflow Task"])]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.WFL_TASK_DETAIL_OtherView)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=50158427-351C-4cd2-B508-9705AADB9821")]
    public async Task<WorkflowGetCurrentHandoverTaskResponse> GetCurrentHandoverTask([FromRoute] long caseId, CancellationToken cancellationToken)
      => await _mediator.Send(new GetCurrentHandoverTaskRequest(caseId), cancellationToken);
}