namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAPreview;

public class GetDocumentOnSAPreviewHandler : IRequestHandler<GetDocumentOnSAPreviewRequest, GetDocumentOnSAPreviewResponse>
{
    public async Task<GetDocumentOnSAPreviewResponse> Handle(GetDocumentOnSAPreviewRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);

        return new GetDocumentOnSAPreviewResponse
        {

        };
    }

    public GetDocumentOnSAPreviewHandler()
    {
        
    }
}