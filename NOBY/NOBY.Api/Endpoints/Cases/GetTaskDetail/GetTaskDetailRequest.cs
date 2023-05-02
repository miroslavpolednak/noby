﻿namespace NOBY.Api.Endpoints.Cases.GetTaskDetail;

public class GetTaskDetailRequest : IRequest<GetTaskDetailResponse>
{
    public long CaseId { get; }
    public long TaskId { get; }

    public GetTaskDetailRequest(long caseId, long taskId)
    {
        CaseId = caseId;
        TaskId = taskId;
    }
}