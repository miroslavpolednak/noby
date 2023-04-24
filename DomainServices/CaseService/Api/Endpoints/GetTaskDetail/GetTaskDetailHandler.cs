﻿using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;
using DomainServices.CodebookService.Clients;
using System.Text.RegularExpressions;

namespace DomainServices.CaseService.Api.Endpoints.GetTaskDetail;

internal class GetTaskDetailHandler : IRequestHandler<GetTaskDetailRequest, GetTaskDetailResponse>
{
    private readonly SbWebApiCommonDataProvider _commonDataProvider;
    private readonly ISbWebApiClient _sbWebApiClient;
    private readonly ICodebookServiceClients _codebookService;

    public GetTaskDetailHandler(SbWebApiCommonDataProvider commonDataProvider, ISbWebApiClient sbWebApiClient, ICodebookServiceClients codebookService)
    {
        _commonDataProvider = commonDataProvider;
        _sbWebApiClient = sbWebApiClient;
        _codebookService = codebookService;
    }

    public async Task<GetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var sbRequest = new FindByTaskIdRequest
        {
            TaskIdSb = request.TaskIdSb,
            TaskStates = await _commonDataProvider.GetValidTaskStateIds(cancellationToken)
        };

        var tasks = await _sbWebApiClient.FindTasksByTaskId(sbRequest, cancellationToken);

        if (tasks.ItemsFound == 0)
            throw new CisValidationException(13026, "Task ID not found in SB");

        var response = new GetTaskDetailResponse();

        foreach (var taskData in tasks.Tasks)
        {
            response.TaskDetails.Add(new TaskDetailResponse
            {
                TaskObject = taskData.ToWorkflowTask(),
                TaskDetail = await CreateTaskDetail(taskData, cancellationToken)
            });
        }

        return response;
    }

    private async Task<TaskDetailItem> CreateTaskDetail(IReadOnlyDictionary<string, string> taskData, CancellationToken cancellationToken)
    {
        var taskDetail = taskData.ToTaskDetail();

        var performer = await _codebookService.GetOperator(taskData["ukol_op_zpracovatel"], cancellationToken);

        taskDetail.PerformanName = performer.PerformerName;
        taskDetail.TaskCommunication.AddRange(ParseTaskCommunications(taskData));

        return taskDetail;
    }

    private static IEnumerable<TaskCommunicationItem> ParseTaskCommunications(IReadOnlyDictionary<string, string> taskData)
    {
        const string Pattern = @"(?=#Separator(Request|Response)#|$)(.*?)(?=#Separator(Request|Response)#|$)";

        var text = taskData.GetInteger("ukol_typ_noby") switch
        {
            1 => taskData["ukol_dozadani_noby"],
            3 => taskData["ukol_konzultace_noby"],
            7 => taskData["ukol_predanihs_noby"]
        };

        var matches = Regex.Matches(text, Pattern);

        for (var i = 0; i <= matches.Count - 1; i += 2)
        {
            if (matches[i].Groups[1].Value == "Request")
            {
                yield return new TaskCommunicationItem
                {
                    TaskRequest = matches[i].Value
                };

                i -= 1;

                continue;
            }

            yield return new TaskCommunicationItem
            {
                TaskResponse = matches[i].Value,
                TaskRequest = matches[i + 1].Value
            };
        }
    }
}