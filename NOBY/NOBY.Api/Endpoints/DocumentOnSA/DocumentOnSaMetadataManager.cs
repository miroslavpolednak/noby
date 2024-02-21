using SharedTypes.Enums;
using DomainServices.CodebookService.Contracts.v1;
using FastEnumUtility;
using NOBY.Dto.Signing;

namespace NOBY.Api.Endpoints.DocumentOnSA;

public static class DocumentOnSaMetadataManager
{
    public static EACodeMainItem GetEaCodeMainItem(DocIdentificator docIdentificator, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypeItems, List<EaCodesMainResponse.Types.EaCodesMainItem> eaCodeMainItems)
    {
        EaCodesMainResponse.Types.EaCodesMainItem eaCodeMain;

        if (docIdentificator.DocumentTypeId is not null)
        {
            var docType = documentTypeItems.Single(d => d.Id == docIdentificator.DocumentTypeId);
            eaCodeMain = eaCodeMainItems.Single(e => e.Id == docType.EACodeMainId);
        }
        else if (docIdentificator.EACodeMainId is not null)
        {
            eaCodeMain = eaCodeMainItems.Single(e => e.Id == docIdentificator.EACodeMainId);
        }
        else
        {
            throw new NobyValidationException($"{nameof(docIdentificator.DocumentTypeId)} or {nameof(docIdentificator.EACodeMainId)} have to be fill in");
        }

        return new EACodeMainItem { Id = eaCodeMain.Id, DocumentType = eaCodeMain.Name, Category = eaCodeMain.Category };
    }

    public static SignatureState GetSignatureState(DocumentOnSAInfo docSa, List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates) => docSa switch
    {
        // ready (připraveno) 1
        DocumentOnSAInfo doc when doc.IsValid && doc.DocumentOnSAId is null => GetSignatureState(1, signatureStates),
        // InTheProcess (v procesu) 2
        DocumentOnSAInfo doc when doc.IsValid && doc.DocumentOnSAId is not null && doc.IsSigned == false => GetSignatureState(2, signatureStates),
        // WaitingForScan (čeká na sken) 3
        DocumentOnSAInfo doc when doc.IsValid && doc.IsSigned && doc.SignatureTypeId == SignatureTypes.Paper.ToByte() && doc.EArchivIdsLinked.Count == 0 && doc.SalesArrangementTypeId != SalesArrangementTypes.Drawing.ToByte() && doc.Source != Source.Workflow => GetSignatureState(3, signatureStates),
        // Signed (podepsáno) 4
        DocumentOnSAInfo doc when doc.IsValid && doc.IsSigned && (doc.EArchivIdsLinked.Count != 0 || doc.SalesArrangementTypeId == SalesArrangementTypes.Drawing.ToByte() || doc.Source == Source.Workflow || doc.SignatureTypeId == SignatureTypes.Paper.ToByte()) => GetSignatureState(4, signatureStates),
        // Canceled (zrušeno) 5 
        _ => GetSignatureState(5, signatureStates)
    };

    private static SignatureState GetSignatureState(int stateId, List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates)
    {
        var signatureState = signatureStates.Single(s => s.Id == stateId);
        return new SignatureState { Id = signatureState.Id, Name = signatureState.Name };
    }
}

public class DocIdentificator
{
    public int? DocumentTypeId { get; set; }

    public int? EACodeMainId { get; set; }
}

public class DocumentOnSAInfo
{
    public bool IsValid { get; set; }

    public int? DocumentOnSAId { get; set; }

    public bool IsSigned { get; set; }

    public Source Source { get; set; }

    public int? SalesArrangementTypeId { get; set; }

    public int SignatureTypeId { get; set; }

    public IReadOnlyCollection<string> EArchivIdsLinked { get; set; } = null!;
}
