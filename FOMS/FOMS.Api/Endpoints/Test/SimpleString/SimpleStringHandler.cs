namespace FOMS.Api.Endpoints.Test.Handlers;

internal class SimpleStringHandler : IRequestHandler<Dto.SimpleStringRequest, string>
{
    public Task<string> Handle(Dto.SimpleStringRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult("OK, we've got your request");
    }
}
