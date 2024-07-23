using DomainServices.CaseService.Contracts;
using ExternalServices.SbWebApi.Dto.FindTasks;
using ExternalServices.SbWebApi.V1;
using DomainServices.CodebookService.Clients;
using System.Text.RegularExpressions;

namespace DomainServices.CaseService.Api.Endpoints.v1.GetTaskDetail;

internal sealed partial class GetTaskDetailHandler(
    ISbWebApiClient _sbWebApiClient, 
    ICodebookServiceClient _codebookService)
        : IRequestHandler<GetTaskDetailRequest, GetTaskDetailResponse>
{
    public async Task<GetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var taskStateIds = (await _codebookService.WorkflowTaskStates(cancellationToken))
            .Select(i => i.Id)
            .ToList();

        var sbRequest = new FindByTaskIdRequest
        {
            TaskIdSb = request.TaskIdSb,
            TaskStates = taskStateIds
        };

        var tasks = await _sbWebApiClient.FindTasksByTaskId(sbRequest, cancellationToken);

        if (!tasks.Any())
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.TaskIdNotFound, request.TaskIdSb);
        }

        // pry nikdy nemuze byt vic tasku
        var task = tasks.First();
        return new GetTaskDetailResponse
        {
            TaskObject = task.ToWorkflowTask(),
            TaskDetail = await createTaskDetail(task, cancellationToken)
        };
    }

    private async Task<TaskDetailItem> createTaskDetail(IReadOnlyDictionary<string, string> taskData, CancellationToken cancellationToken)
    {
        var taskDetail = taskData.ToTaskDetail();
        parseTaskCommunications(taskDetail, taskData);

        // operator neni povinny
        try
        {
            var performer = await _codebookService.GetOperator(taskData["ukol_op_zpracovatel"], cancellationToken);
            taskDetail.PerformanName = performer.PerformerName;
            taskDetail.PerformerCode = performer.PerformerCode;
        }
        catch { }

        return taskDetail;
    }

    private static void parseTaskCommunications(TaskDetailItem taskDetail, IReadOnlyDictionary<string, string> taskData)
    {
        var taskType = taskData.GetInteger("ukol_typ_noby");

        if (taskType == 2)
        {
            if (!string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_overeni_pozadavek")) || !string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_overeni_odpoved")))
            {
                taskDetail.TaskCommunication.Add(new TaskCommunicationItem()
                {
                    TaskResponse = taskData.GetValueOrDefault("ukol_overeni_odpoved") ?? "",
                    TaskRequest = taskData.GetValueOrDefault("ukol_overeni_pozadavek") ?? ""
                });
            }
        }
        else
        {
            var text = taskType switch
            {
                1 when !string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_dozadani_noby")) => taskData["ukol_dozadani_noby"],
                3 when !string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_konzultace_noby")) => taskData["ukol_konzultace_noby"],
                6 when !string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_podpis_noby")) => taskData["ukol_podpis_noby"],
                7 when !string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_predanihs_noby")) => taskData["ukol_predanihs_noby"],
                _ => null
            };

            if (string.IsNullOrEmpty(text))
                return;

            var matches = getMessagePatternRegex().Matches(text);

            for (var i = 0; i < matches.Count; i += 2)
            {
                if (matches[i].Groups[0].Value.Trim().StartsWith("#SeparatorRequest#", StringComparison.OrdinalIgnoreCase))
                {
                    taskDetail.TaskCommunication.Add(new TaskCommunicationItem
                    {
                        TaskRequest = matches[i].Groups[1].Value.Trim()
                    });

                    i--;

                    continue;
                }

                taskDetail.TaskCommunication.Add(new TaskCommunicationItem
                {
                    TaskResponse = matches[i].Groups[1].Value.Trim(),
                    TaskRequest = matches.Count > i + 1 ? matches[i + 1].Groups[1].Value.Trim() : default
                });
            }
        }
    }

    [GeneratedRegex(@"#Separator(?:Response|Request)#\s*([\s\S]*?)(?=\s*#Separator(?:Response|Request)#|$)", RegexOptions.Compiled)]
    private static partial Regex getMessagePatternRegex();
}