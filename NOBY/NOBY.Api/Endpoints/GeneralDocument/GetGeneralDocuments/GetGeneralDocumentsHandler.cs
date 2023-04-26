using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.GeneralDocument.GetGeneralDocuments;

public class GetGeneralDocumentsHandler : IRequestHandler<GetGeneralDocumentsRequest, List<Document>>
{
    public async Task<List<Document>> Handle(GetGeneralDocumentsRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<Document>();
    }

    private readonly ICodebookServiceClients _codebookService;
    
    public GetGeneralDocumentsHandler(ICodebookServiceClients codebookService)
    {
        _codebookService = codebookService;
    }
}