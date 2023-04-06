namespace NOBY.Api.Endpoints.Cases.GetTaskList.Dto;

public enum State
{
    ForProcessing = 1,
    OperationalSupport,
    Sent,
    Completed,
    Cancelled
}