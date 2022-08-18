using DomainServices.CodebookService.Abstraction;

namespace FOMS.Api.Endpoints.Codebooks.GetAll.CodebookMap;

public interface ICodebookEndpoint
{
    string Code { get; }

    Type ReturnType { get; }

    Task<IEnumerable<object>> GetObjects(ICodebookServiceAbstraction codebookService, CancellationToken cancellationToken);
}