using DomainServices.CodebookService.Abstraction;

namespace FOMS.Api.Endpoints.Codebooks.GetAll.CodebookMap;

public class CodebookEndpoint<TReturn> : ICodebookEndpoint where TReturn : class
{
    private readonly Func<ICodebookServiceAbstraction, Delegate> _endpointCallFactory;
    private readonly Func<IEnumerable<object>, IEnumerable<object>>? _resultCustomizer;

    public CodebookEndpoint(string code, Func<ICodebookServiceAbstraction, Delegate> endpointCallFactory, Func<IEnumerable<object>, IEnumerable<object>>? resultCustomizer)
    {
        _endpointCallFactory = endpointCallFactory;
        _resultCustomizer = resultCustomizer;
        Code = code;
    }

    public string Code { get; }

    public Type ReturnType => typeof(TReturn);

    public async Task<IEnumerable<object>> GetObjects(ICodebookServiceAbstraction codebookService, CancellationToken cancellationToken)
    {
        IEnumerable<object> codebookList = await CallEndpoint(codebookService, cancellationToken);

        if (_resultCustomizer is not null)
            codebookList = _resultCustomizer(codebookList);

        return codebookList;
    }

    private Task<List<TReturn>> CallEndpoint(ICodebookServiceAbstraction codebookService, CancellationToken cancellationToken)
    {
        var endpointCall = (Func<CancellationToken, Task<List<TReturn>>>)_endpointCallFactory(codebookService);

        return endpointCall(cancellationToken);
    }
}