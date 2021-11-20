namespace FOMS.Api.Endpoints.Test.Handlers;

internal class BadRequestHandler : IRequestHandler<Dto.BadRequestRequest, string>
{
    public Task<string> Handle(Dto.BadRequestRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult("Validation went fine!");
    }
}
