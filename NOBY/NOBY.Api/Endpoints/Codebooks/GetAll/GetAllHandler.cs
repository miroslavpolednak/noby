using DomainServices.CodebookService.Clients;
using NOBY.Api.Endpoints.Codebooks.CodebookMap;

namespace NOBY.Api.Endpoints.Codebooks.GetAll;

internal sealed class GetAllHandler
    : IRequestHandler<GetAllRequest, List<GetAllResponseItem>>
{
    public async Task<List<GetAllResponseItem>> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        List<GetAllResponseItem> model = new();

        _logger.CodebooksGetAllStarted(request.CodebookCodes);

        foreach (var (original, key) in request.CodebookCodes)
        {
            var objects = await _codebookMap[key].GetObjects(_codebooks, cancellationToken);

            model.Add(new GetAllResponseItem(original, objects));
        }

        //foreach (var code in request.CodebookCodes)
        //    model.Add(await fillCodebook(code.Key, code.Original, cancellationToken));
        
        return model;
    }

    private readonly ICodebookServiceClients _codebooks;
    private readonly ICodebookMap _codebookMap;
    private readonly ILogger<GetAllHandler> _logger;

    public GetAllHandler(ICodebookServiceClients codebooks, ICodebookMap codebookMap, ILogger<GetAllHandler> logger)
    {
        _logger = logger;
        _codebooks = codebooks;
        _codebookMap = codebookMap;
    }
}
