using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.Codebooks.CodebookMap;

public class CodebookEndpoint<TReturn> : ICodebookEndpoint where TReturn : class
{
    private readonly Func<ICodebookServiceClient, CancellationToken, Task<List<TReturn>>> _endpointCallFactory;
    private readonly Func<IEnumerable<object>, IEnumerable<object>>? _resultCustomizer;

    public CodebookEndpoint(string code, Func<ICodebookServiceClient, CancellationToken, Task<List<TReturn>>> endpointCallFactory, Func<IEnumerable<object>, IEnumerable<object>>? resultCustomizer)
    {
        _endpointCallFactory = endpointCallFactory;
        _resultCustomizer = resultCustomizer;
        Code = code;
    }

    public string Code { get; }

    public Type ReturnType => typeof(TReturn);

    public async Task<IEnumerable<object>> GetObjects(ICodebookServiceClient codebookService, CancellationToken cancellationToken)
    {
        IEnumerable<object> codebookList = await CallEndpoint(codebookService, cancellationToken);

        if (_resultCustomizer is not null)
            codebookList = _resultCustomizer(codebookList);

        return codebookList;
    }

    private Task<List<TReturn>> CallEndpoint(ICodebookServiceClient codebookService, CancellationToken cancellationToken)
    {
        return _endpointCallFactory(codebookService, cancellationToken);
    }
}