using Newtonsoft.Json;
using NOBY.Api.Endpoints.Shared;

namespace NOBY.Api.Endpoints.Cases.UpdateTaskDetail;

public class UpdateTaskDetailRequest : IRequest
{
    [JsonIgnore]
    internal long CaseId;
    
    [JsonIgnore]
    internal long TaskId;
    
    /// <summary>
    /// SB ID úkolu, není zobrazeno na FE UI
    /// </summary>
    /// <example>22456</example>[
    public int TaskIdSB { get; set; }

    public string TaskUserResponse { get; set; } = null!;

    public List<DocumentInformation> Attachments { get; set; } = null!;

    internal UpdateTaskDetailRequest InfuseIds(long caseId, long taskId)
    {
        CaseId = caseId;
        TaskId = taskId;
        return this;
    }
}