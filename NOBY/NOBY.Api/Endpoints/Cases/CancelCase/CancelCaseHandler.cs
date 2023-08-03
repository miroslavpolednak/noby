namespace NOBY.Api.Endpoints.Cases.CancelCase;

internal sealed class CancelCaseHandler
    : IRequestHandler<CancelCaseRequest, CancelCaseResponse>
{
    public async Task<CancelCaseResponse> Handle(CancelCaseRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return new CancelCaseResponse
        {
        };
    }

    public CancelCaseHandler()
    {
    }
}