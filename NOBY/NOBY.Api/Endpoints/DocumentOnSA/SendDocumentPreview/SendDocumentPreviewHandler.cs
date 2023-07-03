namespace NOBY.Api.Endpoints.DocumentOnSA.SendDocumentPreview;

public class SendDocumentPreviewHandler : IRequestHandler<SendDocumentPreviewRequest>
{
    public async Task Handle(SendDocumentPreviewRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
    }

    public SendDocumentPreviewHandler()
    {
        
    }
}