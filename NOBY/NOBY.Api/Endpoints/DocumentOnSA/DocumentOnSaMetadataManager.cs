using CIS.Foms.Enums;
using DomainServices.CodebookService.Contracts.v1;
using FastEnumUtility;
using NOBY.Dto.Signing;

namespace NOBY.Api.Endpoints.DocumentOnSA;

public static class DocumentOnSaMetadataManager
{
    public static EACodeMainItem GetEaCodeMainItem(int documentTypeId, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypeItems, List<EaCodesMainResponse.Types.EaCodesMainItem> eaCodeMainItems)
    {
        var docType = documentTypeItems.Single(d => d.Id == documentTypeId);
        var eaCodeMain = eaCodeMainItems.Single(e => e.Id == docType.EACodeMainId);
        return new EACodeMainItem { Id = docType.EACodeMainId!.Value, DocumentType = eaCodeMain.Name, Category = eaCodeMain.Category };
    }

    public static SignatureState GetSignatureState(DocumentOnSAInfo docSa, List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates) => docSa switch
    {
        // ready (připraveno) 1
        DocumentOnSAInfo doc when doc.IsValid && doc.DocumentOnSAId is null => GetSignatureState(1, signatureStates),
        // InTheProcess (v procesu) 2
        DocumentOnSAInfo doc when doc.IsValid && doc.DocumentOnSAId is not null && doc.IsSigned == false => GetSignatureState(2, signatureStates),
        // WaitingForScan (čeká na sken) 3
        DocumentOnSAInfo doc when doc.IsValid && doc.IsSigned && !doc.EArchivIdsLinked.Any() && doc.SalesArrangementTypeId != SalesArrangementTypes.Drawing.ToByte() && doc.Source != Source.Workflow => GetSignatureState(3, signatureStates),
        // Signed (podepsáno) 4
        DocumentOnSAInfo doc when doc.IsValid && doc.IsSigned && (doc.EArchivIdsLinked.Any() || doc.SalesArrangementTypeId == SalesArrangementTypes.Drawing.ToByte() || doc.Source == Source.Workflow) => GetSignatureState(4, signatureStates),
        // Canceled (zrušeno) 5 
        _ => GetSignatureState(5, signatureStates)
    };

    private static SignatureState GetSignatureState(int stateId, List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates)
    {
        var signatureState = signatureStates.Single(s => s.Id == stateId);
        return new SignatureState { Id = signatureState.Id, Name = signatureState.Name };
    }
}

public class DocumentOnSAInfo
{
    public bool IsValid { get; set; }

    public int? DocumentOnSAId { get; set; }

    public bool IsSigned { get; set; }

    public Source Source { get; set; }

    public int? SalesArrangementTypeId { get; set; }

    public IReadOnlyCollection<string> EArchivIdsLinked { get; set; } = null!;
}
