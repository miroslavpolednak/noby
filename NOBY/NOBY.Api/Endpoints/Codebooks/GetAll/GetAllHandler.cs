using DomainServices.CodebookService.Clients;
using NOBY.Api.Endpoints.Codebooks.CodebookMap;

namespace NOBY.Api.Endpoints.Codebooks.GetAll;

internal sealed class GetAllHandler(
    ICodebookServiceClient _codebooks, 
    ICodebookMap _codebookMap, 
    ILogger<GetAllHandler> _logger)
        : IRequestHandler<GetAllRequest, List<CodebooksGetAllResponseItem>>
{
    public async Task<List<CodebooksGetAllResponseItem>> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        List<CodebooksGetAllResponseItem> model = [];

        _logger.CodebooksGetAllStarted(request.CodebookCodes);

        foreach (var (original, key) in request.CodebookCodes)
        {
            var objects = await _codebookMap[key].GetObjects(_codebooks, cancellationToken);

            model.Add(new CodebooksGetAllResponseItem
            {
                Code = original,
                Codebook = objects.ToList()
            });
        }

        //foreach (var code in request.CodebookCodes)
        //    model.Add(await fillCodebook(code.Key, code.Original, cancellationToken));
        
        return model;
    }
}
