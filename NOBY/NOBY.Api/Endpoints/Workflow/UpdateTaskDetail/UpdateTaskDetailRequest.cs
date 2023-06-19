﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Workflow.UpdateTaskDetail;

public class UpdateTaskDetailRequest : IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal long TaskId;

    /// <summary>
    /// SB ID úkolu, není zobrazeno na FE UI
    /// </summary>
    /// <example>22456</example>
    [Required]
    public int TaskIdSB { get; set; }

    /// <summary>
    /// ID typu úkolu podle <a href="https://wiki.kb.cz/display/HT/WorkflowTaskProcessType+%28CIS_WFL_CISELNIKY_HODNOTY+s+CISELNIK_ID+%3D+10%29+-+MOCK">číselníku typu úkolů</a>
    /// </summary>
    /// <example>1</example>
    [Required]
    public int TaskTypeId { get; set; }

    /// <summary>
    /// ID typu odpovědi. 0 - V pořádku, 1 - Závada - upravený dokument, 2 - Závada - chybějící podpis
    /// </summary>
    /// <example>0</example>
    public int? TaskResponseTypeId { get; set; }

    /// <summary>
    /// Text odpovědi
    /// </summary>
    /// <example>Vyplněno, co bylo potřeba.</example>
    public string? TaskUserResponse { get; set; }

    public List<NOBY.Dto.Documents.DocumentInformation>? Attachments { get; set; }

    internal UpdateTaskDetailRequest InfuseIds(long caseId, long taskId)
    {
        CaseId = caseId;
        TaskId = taskId;
        return this;
    }
}