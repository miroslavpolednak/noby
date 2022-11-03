using DomainServices.CodebookService.Abstraction;

namespace NOBY.Api.Endpoints.Codebooks.CodebookMap;

public interface ICodebookEndpoint
{
    string Code { get; }

    Type ReturnType { get; }

    Task<IEnumerable<object>> GetObjects(ICodebookServiceAbstraction codebookService, CancellationToken cancellationToken);
}