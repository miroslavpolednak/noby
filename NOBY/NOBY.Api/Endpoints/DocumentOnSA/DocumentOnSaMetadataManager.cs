using DomainServices.CodebookService.Contracts.Endpoints.DocumentTypes;
using DomainServices.CodebookService.Contracts.Endpoints.EaCodesMain;
using FastEnumUtility;
using NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;

namespace NOBY.Api.Endpoints.DocumentOnSA;

public static class DocumentOnSaMetadataManager
{
    public static EACodeMainItemDto GetEaCodeMainItem(int DocumentTypeId, List<DocumentTypeItem> documentTypeItems, List<EaCodeMainItem> eaCodeMainItems)
    {
        var docType = documentTypeItems.Single(d => d.Id == DocumentTypeId);
        var eaCodeMain = eaCodeMainItems.Single(e => e.Id == docType.EACodeMainId);
        return new EACodeMainItemDto { Id = docType.EACodeMainId!.Value, DocumentType = eaCodeMain.Name, Category = eaCodeMain.Category };
    }

    public static SignatureStateDto GetSignatureState(DocumentOnSAInfo docSa) => docSa switch
    {
        DocumentOnSAInfo doc when doc.DocumentOnSAId is null => new SignatureStateDto { Id = (int)SignatureStateE.Ready, Name = SignatureStateE.Ready.FastToString() },
        DocumentOnSAInfo doc when doc.DocumentOnSAId is not null && doc.IsSigned == false => new SignatureStateDto { Id = (int)SignatureStateE.InTheProcess, Name = SignatureStateE.InTheProcess.FastToString() },
        DocumentOnSAInfo doc when doc.IsSigned && string.IsNullOrEmpty(doc.EArchivId) => new SignatureStateDto { Id = (int)SignatureStateE.WaitingForScan, Name = SignatureStateE.WaitingForScan.FastToString() },
        DocumentOnSAInfo doc when doc.IsSigned && !string.IsNullOrEmpty(doc.EArchivId) => new SignatureStateDto { Id = (int)SignatureStateE.Signed, Name = SignatureStateE.Signed.FastToString() },
        _ => new SignatureStateDto { Id = (int)SignatureStateE.Unknown, Name = SignatureStateE.Unknown.FastToString() }
    };
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
