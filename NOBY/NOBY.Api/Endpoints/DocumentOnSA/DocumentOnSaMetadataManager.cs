using DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Api.Endpoints.DocumentOnSA;

public static class DocumentOnSaMetadataManager
{
    public static EACodeMainItemDto GetEaCodeMainItem(int DocumentTypeId, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypeItems, List<EaCodesMainResponse.Types.EaCodesMainItem> eaCodeMainItems)
    {
        var docType = documentTypeItems.Single(d => d.Id == DocumentTypeId);
        var eaCodeMain = eaCodeMainItems.Single(e => e.Id == docType.EACodeMainId);
        return new EACodeMainItemDto { Id = docType.EACodeMainId!.Value, DocumentType = eaCodeMain.Name, Category = eaCodeMain.Category };
    }

    public static SignatureStateDto GetSignatureState(DocumentOnSAInfo docSa, List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates) => docSa switch
    {
        // ready (připraveno) 1
        DocumentOnSAInfo doc when doc.DocumentOnSAId is null => GetSignatureState(1, signatureStates),
        // InTheProcess (v procesu) 2
        DocumentOnSAInfo doc when doc.DocumentOnSAId is not null && doc.IsSigned == false => GetSignatureState(2, signatureStates),
        // WaitingForScan (čeká na sken) 3
        DocumentOnSAInfo doc when doc.IsSigned && string.IsNullOrEmpty(doc.EArchivId) => GetSignatureState(3, signatureStates),
        // Signed (podepsáno) 4
        DocumentOnSAInfo doc when doc.IsSigned && !string.IsNullOrEmpty(doc.EArchivId) => GetSignatureState(4, signatureStates),
        _ => new SignatureStateDto { Id = 0, Name = "Unknown" }
    };

    private static SignatureStateDto GetSignatureState(int stateId, List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates)
    {
        var signatureState = signatureStates.Single(s => s.Id == stateId);
        return new SignatureStateDto { Id = signatureState.Id, Name = signatureState.Name };
    }
}

public class EACodeMainItemDto
{
    public int Id { get; set; }

    public string DocumentType { get; set; } = null!;

    public string Category { get; set; } = null!;
}

public class SignatureStateDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}

public class DocumentOnSAInfo
{
    public int? DocumentOnSAId { get; set; }

    public bool IsSigned { get; set; }

    public string? EArchivId { get; set; }
}
