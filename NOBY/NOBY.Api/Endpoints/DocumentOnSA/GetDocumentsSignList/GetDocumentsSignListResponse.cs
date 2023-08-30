using CIS.Foms.Enums;
using NOBY.Dto.Signing;

namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;

public class GetDocumentsSignListResponse
{
    public IReadOnlyCollection<DocumentData> Data { get; set; } = null!;
}



