using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.Codebooks.CodebookMap;

public interface ICodebookEndpoint
{
    string Code { get; }

    Type ReturnType { get; }

    Task<IEnumerable<object>> GetObjects(ICodebookServiceClient codebookService, CancellationToken cancellationToken);
}