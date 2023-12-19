using NOBY.Dto.Signing;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAStatus;

public class GetDocumentOnSAStatusResponse
{
    public int DocumentOnSAId { get; set; }
    public SignatureState SignatureState { get; set; } = null!;
}
