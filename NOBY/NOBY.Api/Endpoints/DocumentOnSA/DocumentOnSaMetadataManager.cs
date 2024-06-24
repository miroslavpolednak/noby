using DomainServices.CodebookService.Contracts.v1;
using FastEnumUtility;

namespace NOBY.Api.Endpoints.DocumentOnSA;

public static class DocumentOnSaMetadataManager
{
    private const int _individualCommunication = 605353;

    public static SharedTypesSigningEACodeMainItem GetEaCodeMainItem(DocIdentificator docIdentificator, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypeItems, List<EaCodesMainResponse.Types.EaCodesMainItem> eaCodeMainItems)
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

        return new SharedTypesSigningEACodeMainItem { Id = eaCodeMain.Id, DocumentType = eaCodeMain.Name, Category = eaCodeMain.Category };
    }

    public static SharedTypesSigningSignatureState GetSignatureState(DocumentOnSAInfo docSa, List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates) => docSa switch
    {
        // ready (připraveno) 1
        DocumentOnSAInfo doc when doc.IsValid && doc.DocumentOnSAId is null => GetSignatureState(1, signatureStates),
        // InTheProcess (v procesu) 2
        DocumentOnSAInfo doc when doc.IsValid && doc.DocumentOnSAId is not null && doc.IsSigned == false => GetSignatureState(2, signatureStates),
        // WaitingForScan (čeká na sken) 3
        DocumentOnSAInfo doc when doc.IsValid && doc.IsSigned && doc.SignatureTypeId == SignatureTypes.Paper.ToByte() && doc.EArchivIdsLinked.Count == 0 && doc.SalesArrangementTypeId != SalesArrangementTypes.Drawing.ToByte() &&
        (
         (doc.Source == Source.Noby)
         ||
         (doc.Source == Source.Workflow && doc.EaCodeMainId == _individualCommunication)
        )
        => GetSignatureState(3, signatureStates),
        // Signed (podepsáno) 4
        DocumentOnSAInfo doc when doc.IsValid && doc.IsSigned &&
        (
          (doc.SalesArrangementTypeId == SalesArrangementTypes.Drawing.ToByte())
          ||
          (doc.SalesArrangementTypeId != SalesArrangementTypes.Drawing.ToByte() && doc.Source == Source.Workflow && doc.EaCodeMainId != _individualCommunication)
          ||
          (doc.SalesArrangementTypeId != SalesArrangementTypes.Drawing.ToByte() && doc.Source == Source.Workflow && doc.EaCodeMainId == _individualCommunication && doc.SignatureTypeId == SignatureTypes.Paper.ToByte() && doc.EArchivIdsLinked.Count > 0)
          ||
          (doc.SalesArrangementTypeId != SalesArrangementTypes.Drawing.ToByte() && doc.Source == Source.Noby && doc.SignatureTypeId == SignatureTypes.Electronic.ToByte())
          ||
          (doc.SalesArrangementTypeId != SalesArrangementTypes.Drawing.ToByte() && doc.Source == Source.Noby && doc.SignatureTypeId == SignatureTypes.Paper.ToByte() && doc.EArchivIdsLinked.Count > 0)
        )
        => GetSignatureState(4, signatureStates),
        // Canceled (zrušeno) 5 
        _ => GetSignatureState(5, signatureStates)
    };

    private static SharedTypesSigningSignatureState GetSignatureState(int stateId, List<GenericCodebookResponse.Types.GenericCodebookItem> signatureStates)
    {
        var signatureState = signatureStates.Single(s => s.Id == stateId);
        return new SharedTypesSigningSignatureState { Id = signatureState.Id, Name = signatureState.Name };
    }
}
