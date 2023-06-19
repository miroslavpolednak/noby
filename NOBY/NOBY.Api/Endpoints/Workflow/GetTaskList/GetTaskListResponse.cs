﻿namespace NOBY.Api.Endpoints.Workflow.GetTaskList;

public sealed class GetTaskListResponse
{
    public List<Dto.Workflow.WorkflowTask> Tasks { get; set; } = new();

    public List<Dto.Workflow.WorkflowProcess> Processes { get; set; } = new();
}