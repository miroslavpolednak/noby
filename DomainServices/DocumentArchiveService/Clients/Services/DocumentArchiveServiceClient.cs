using DomainServices.DocumentArchiveService.Contracts;

namespace DomainServices.DocumentArchiveService.Clients.Services;

internal sealed class DocumentArchiveServiceClient
    : IDocumentArchiveServiceClient
{
    public async Task<IServiceCallResult> GenerateDocumentId(EnvironmentNames environmentName, int? environmentIndex = 0, CancellationToken cancellationToken = default)
    {
        var result = await _service.GenerateDocumentId(
            new()
            {
                EnvironmentName = environmentName,
                EnvironmentIndex = environmentIndex
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<string>(result.DocumentId!);
    }

    private readonly IDocumentArchiveService _service;

    public DocumentArchiveServiceClient(
        IDocumentArchiveService service)
    {
        _service = service;
    }
}
