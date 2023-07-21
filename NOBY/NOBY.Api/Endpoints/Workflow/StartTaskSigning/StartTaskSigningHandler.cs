namespace NOBY.Api.Endpoints.Workflow.StartTaskSigning;

internal sealed class StartTaskSigningHandler : IRequestHandler<StartTaskSigningRequest, StartTaskSigningResponse>
{
    public async Task<StartTaskSigningResponse> Handle(StartTaskSigningRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);

        return new StartTaskSigningResponse();
    }
}