﻿using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.Codebooks.CodebookMap;

public class CodebookEndpoint<TReturn> : ICodebookEndpoint where TReturn : class
{
    private readonly Func<ICodebookServiceClients, Delegate> _endpointCallFactory;
    private readonly Func<IEnumerable<object>, IEnumerable<object>>? _resultCustomizer;

    public CodebookEndpoint(string code, Func<ICodebookServiceClients, Delegate> endpointCallFactory, Func<IEnumerable<object>, IEnumerable<object>>? resultCustomizer)
    {
        _endpointCallFactory = endpointCallFactory;
        _resultCustomizer = resultCustomizer;
        Code = code;
    }

    public string Code { get; }

    public Type ReturnType => typeof(TReturn);

    public async Task<IEnumerable<object>> GetObjects(ICodebookServiceClients codebookService, CancellationToken cancellationToken)
    {
        IEnumerable<object> codebookList = await CallEndpoint(codebookService, cancellationToken);

        if (_resultCustomizer is not null)
            codebookList = _resultCustomizer(codebookList);

        return codebookList;
    }

    private Task<List<TReturn>> CallEndpoint(ICodebookServiceClients codebookService, CancellationToken cancellationToken)
    {
        var endpointCall = (Func<CancellationToken, Task<List<TReturn>>>)_endpointCallFactory(codebookService);

        return endpointCall(cancellationToken);
    }
}