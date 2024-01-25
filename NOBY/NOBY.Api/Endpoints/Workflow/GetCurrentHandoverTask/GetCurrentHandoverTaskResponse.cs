﻿namespace NOBY.Api.Endpoints.Workflow.GetCurrentHandoverTask;

public class GetCurrentHandoverTaskResponse
{
    public Dto.Workflow.WorkflowTask? Task { get; set; }

    public Dto.Workflow.WorkflowTaskDetail? TaskDetail { get; set; }

    public List<NOBY.Dto.Documents.DocumentsMetadata>? Documents { get; set; }
}
