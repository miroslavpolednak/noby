using CIS.Core.Attributes;
using SharedTypes.Enums;
using SharedTypes.GrpcTypes;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Clients;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Api.Extensions;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using FastEnumUtility;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Mappers;

public interface IDocumentOnSaMapper
{
    IEnumerable<DocumentOnSAToSign> MapDocumentOnSaToSign(IEnumerable<DocumentOnSa> documentOnSas);

    DocumentOnSAToSign CreateDocumentOnSaToSign(DocumentTypesResponse.Types.DocumentTypeItem documentTypeItem, int salesArrangementId);

    /// <summary>
    /// For CRS
    /// </summary>
    Task<IReadOnlyCollection<DocumentOnSAToSign>> CreateDocumentOnSaToSign(IEnumerable<int> customerOnSaIds, int salesArrangementId, CancellationToken cancellationToken);

    IEnumerable<DocumentOnSAToSign> CreateDocumentOnSaToSign(IEnumerable<Household> households);

}

[ScopedService, AsImplementedInterfacesService]
public class DocumentOnSaMapper : IDocumentOnSaMapper
{
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICustomerChangeDataMerger _customerChangeDataMerger;

    public DocumentOnSaMapper(
        ICustomerOnSAServiceClient customerOnSAService,
        ICustomerServiceClient customerService,
        ICustomerChangeDataMerger customerChangeDataMerger
        )
    {
        _customerOnSAService = customerOnSAService;
        _customerService = customerService;
        _customerChangeDataMerger = customerChangeDataMerger;
    }

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
                SignatureDateTime = documentOnSa.SignatureDateTime is not null ? Timestamp.FromDateTime(DateTime.SpecifyKind(documentOnSa.SignatureDateTime.Value, DateTimeKind.Local)) : null,
                SignatureConfirmedBy = documentOnSa.SignatureConfirmedBy,
                IsFinal = documentOnSa.IsFinal,
                SignatureTypeId = documentOnSa.SignatureTypeId,
                Source = documentOnSa.Source.MapToContractEnum(),
                // Only for CRS DocumentTypeId == 13, should by only one in collection
                CustomerOnSA = documentOnSa.DocumentTypeId == DocumentTypes.DANRESID.ToByte() ? new()
                {
                    CustomerOnSAId = documentOnSa.SigningIdentities.FirstOrDefault()?.SigningIdentityJson?.CustomerOnSAId,
                    FirstName = documentOnSa.SigningIdentities.FirstOrDefault()?.SigningIdentityJson?.FirstName,
                    LastName = documentOnSa.SigningIdentities.FirstOrDefault()?.SigningIdentityJson?.LastName
                } : new(),
                IsPreviewSentToCustomer = documentOnSa.IsPreviewSentToCustomer,
                TaskId = documentOnSa.TaskId,
                CaseId = documentOnSa.CaseId,
                ExternalId = documentOnSa.ExternalId,
                EArchivIdsLinked = { documentOnSa.EArchivIdsLinkeds.Select(s => s.EArchivId) },
                EACodeMainId = documentOnSa.EACodeMainId,
                IsCustomerPreviewSendingAllowed = documentOnSa.IsCustomerPreviewSendingAllowed
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

    public async Task<IReadOnlyCollection<DocumentOnSAToSign>> CreateDocumentOnSaToSign(IEnumerable<int> customerOnSaIds, int salesArrangementId, CancellationToken cancellationToken)
    {
        var documentsToSignList = new List<DocumentOnSAToSign>();
        foreach (var customerOnSaId in customerOnSaIds)
        {
            // Merge customer with customerOnSa
            var customerOnSa = await _customerOnSAService.GetCustomer(customerOnSaId, cancellationToken);
            var customerDetail = await _customerService.GetCustomerDetail(customerOnSa.CustomerIdentifiers.First(r => r.IdentityScheme == Identity.Types.IdentitySchemes.Kb), cancellationToken);
            _customerChangeDataMerger.MergeClientData(customerDetail, customerOnSa);
            documentsToSignList.Add(new()
            {
                DocumentTypeId = DocumentTypes.DANRESID.ToByte(), //13
                SalesArrangementId = salesArrangementId,
                CustomerOnSA = new()
                {
                    CustomerOnSAId = customerOnSaId,
                    FirstName = customerDetail.NaturalPerson?.FirstName,
                    LastName = customerDetail.NaturalPerson?.LastName,
                },
                IsValid = true,
                IsSigned = false,
                IsArchived = false
            });
        }

        return documentsToSignList;
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
