using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;
using DomainServices.CodebookService.Clients;
using System.Text.RegularExpressions;

namespace DomainServices.CaseService.Api.Endpoints.GetTaskDetail;

internal sealed class GetTaskDetailHandler 
    : IRequestHandler<GetTaskDetailRequest, GetTaskDetailResponse>
{
    public async Task<GetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var sbRequest = new FindByTaskIdRequest
        {
            TaskIdSb = request.TaskIdSb,
            TaskStates = await _commonDataProvider.GetValidTaskStateIds(cancellationToken)
        };

        var tasks = await _sbWebApiClient.FindTasksByTaskId(sbRequest, cancellationToken);

        if (!tasks.Any())
        {
            throw new CisValidationException(ErrorCodeMapper.TaskIdNotFound, ErrorCodeMapper.GetMessage(ErrorCodeMapper.TaskIdNotFound, request.TaskIdSb));
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
        
        var performer = await _codebookService.GetOperator(taskData["ukol_op_zpracovatel"], cancellationToken);
        taskDetail.PerformanName = performer.PerformerName;

        parseTaskCommunications(taskDetail, taskData);

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
                    TaskResponse = taskData.GetValueOrDefault("ukol_overeni_odpoved"),
                    TaskRequest = taskData.GetValueOrDefault("ukol_overeni_pozadavek")
                });
            }
        }
        else
        {
            string? text = taskType switch
            {
                1 when !string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_dozadani_noby")) => taskData["ukol_dozadani_noby"],
                3 when !string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_konzultace_noby")) => taskData["ukol_konzultace_noby"],
                6 when !string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_podpis_noby")) => taskData["ukol_podpis_noby"],
                7 when !string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_predanihs_noby")) => taskData["ukol_predanihs_noby"],
                _ => null
            };

            if (!string.IsNullOrEmpty(text))
            {
                var matches = _messagePatternRegex.Matches(text);

                for (var i = 0; i <= matches.Count - 1; i += 2)
                {
                    if (matches[i].Groups[1].Value == "Request")
                    {
                        taskDetail.TaskCommunication.Add(new TaskCommunicationItem()
                        {
                            TaskRequest = matches[i].Value
                        });

                        i -= 1;

                        continue;
                    }

                    taskDetail.TaskCommunication.Add(new TaskCommunicationItem()
                    {
                        TaskResponse = matches[i].Value,
                        TaskRequest = matches[i + 1].Value
                    });
                }
            }
        }
    }

    private static Regex _messagePatternRegex = new Regex(@"(?=#Separator(Request|Response)#|$)(.*?)(?=#Separator(Request|Response)#|$)", RegexOptions.Compiled);

    private readonly SbWebApiCommonDataProvider _commonDataProvider;
    private readonly ISbWebApiClient _sbWebApiClient;
    private readonly ICodebookServiceClient _codebookService;

    public GetTaskDetailHandler(SbWebApiCommonDataProvider commonDataProvider, ISbWebApiClient sbWebApiClient, ICodebookServiceClient codebookService)
    {
        _commonDataProvider = commonDataProvider;
        _sbWebApiClient = sbWebApiClient;
        _codebookService = codebookService;
    }
}