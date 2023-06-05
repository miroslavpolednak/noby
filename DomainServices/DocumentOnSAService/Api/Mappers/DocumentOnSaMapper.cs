using CIS.Core.Attributes;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Mappers;

public interface IDocumentOnSaMapper
{
    IEnumerable<DocumentOnSAToSign> MapDocumentOnSaToSign(IEnumerable<DocumentOnSa> documentOnSas);
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
                IsFinal = documentOnSa.IsFinal
            };
        }
    }
}
