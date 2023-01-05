namespace NOBY.Api.Endpoints.DocumentArchive.UploadDocument;

public class UploadDocumentRequest:IRequest<Guid>
{
	public UploadDocumentRequest(IFormFile file)
	{
        File = file;
    }

    public IFormFile File { get; }
}
