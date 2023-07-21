using CIS.Core.Attributes;
using CIS.Foms.Enums;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Contracts;
using FastEnumUtility;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Mappers;

public interface IDocumentOnSaMapper
{
    IEnumerable<DocumentOnSAToSign> MapDocumentOnSaToSign(IEnumerable<DocumentOnSa> documentOnSas);

    DocumentOnSAToSign CreateDocumentOnSaToSign(DocumentTypesResponse.Types.DocumentTypeItem documentTypeItem, int salesArrangementId);

    IEnumerable<DocumentOnSAToSign> CreateDocumentOnSaToSign(IEnumerable<int> documentOnSaIds, int salesArrangementId);

    IEnumerable<DocumentOnSAToSign> CreateDocumentOnSaToSign(IEnumerable<Household> households);

}

[ScopedService, AsImplementedInterfacesService]
public class DocumentOnSaMapper : IDocumentOnSaMapper
{
    public IEnumerable<DocumentOnSAToSign> MapDocumentOnSaToSign(IEnumerable<DocumentOnSa> documentOnSas)
    {
        foreach (var documentOnSa in documentOnSas)
        {
            yield return new DocumentOnSAToSign
            {
                DocumentOnSAId = documentOnSa.DocumentOnSAId,
                DocumentTypeId = documentOnSa.DocumentTypeId,
                DocumentTemplateVersionId = documentOnSa.DocumentTemplateVersionId,
                DocumentTemplateVariantId = documentOnSa.DocumentTemplateVariantId,
                FormId = documentOnSa.FormId ?? string.Empty,
                EArchivId = documentOnSa.EArchivId ?? string.Empty,
                DmsxId = documentOnSa.DmsxId ?? string.Empty,
                SalesArrangementId = documentOnSa.SalesArrangementId,
                HouseholdId = documentOnSa.HouseholdId,
                IsValid = documentOnSa.IsValid,
                IsSigned = documentOnSa.IsSigned,
                IsArchived = documentOnSa.IsArchived,
                SignatureMethodCode = documentOnSa.SignatureMethodCode ?? string.Empty,
                SignatureDateTime = documentOnSa.SignatureDateTime is not null ? Timestamp.FromDateTime(DateTime.SpecifyKind(documentOnSa.SignatureDateTime.Value, DateTimeKind.Utc)) : null,
                SignatureConfirmedBy = documentOnSa.SignatureConfirmedBy,
                IsFinal = documentOnSa.IsFinal,
                SignatureTypeId = documentOnSa.SignatureTypeId,
                Source = (Source)documentOnSa.Source,
                CustomerOnSAId = documentOnSa.CustomerOnSAId1,
                IsPreviewSentToCustomer = documentOnSa.IsPreviewSentToCustomer
            };
        }
    }

    public DocumentOnSAToSign CreateDocumentOnSaToSign(DocumentTypesResponse.Types.DocumentTypeItem documentTypeItem, int salesArrangementId)
    {
        return new DocumentOnSAToSign
        {
            DocumentTypeId = documentTypeItem.Id,
            SalesArrangementId = salesArrangementId,
            IsValid = true,
            IsSigned = false,
            IsArchived = false
        };
    }

    public IEnumerable<DocumentOnSAToSign> CreateDocumentOnSaToSign(IEnumerable<int> documentOnSaIds, int salesArrangementId)
    {
        return documentOnSaIds.Select(documentOnSaId => new DocumentOnSAToSign
        {
            DocumentTypeId = DocumentTypes.DANRESID.ToByte(), //13
            SalesArrangementId = salesArrangementId,
            CustomerOnSAId = documentOnSaId,
            IsValid = true,
            IsSigned = false,
            IsArchived = false
        });
    }

    public IEnumerable<DocumentOnSAToSign> CreateDocumentOnSaToSign(IEnumerable<Household> households)
    {
        return households.Select(s => new DocumentOnSAToSign
        {
            DocumentTypeId = GetDocumentTypeId((HouseholdTypes)s.HouseholdTypeId),
            SalesArrangementId = s.SalesArrangementId,
            HouseholdId = s.HouseholdId,
            IsValid = true,
            IsSigned = false,
            IsArchived = false
        });
    }

    private static int GetDocumentTypeId(HouseholdTypes householdType) => householdType switch
    {
        // 1
        HouseholdTypes.Main => DocumentTypes.ZADOSTHU.ToByte(), // 4
        // 2
        HouseholdTypes.Codebtor => DocumentTypes.ZADOSTHD.ToByte(), // 5
        _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.HouseholdTypeIdNotExist, householdType.ToByte())
    };
}
