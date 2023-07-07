using CIS.Core.Attributes;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CaseService.Contracts;
using DomainServices.DocumentOnSAService.Contracts;
using __Entity = DomainServices.DocumentOnSAService.Api.Database.Entities;
using __DbEnum = DomainServices.DocumentOnSAService.Api.Database.Enums;
using __Household = DomainServices.HouseholdService.Contracts;
using DomainServices.DocumentArchiveService.Clients;
using System.Text.Json;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.SalesArrangementService.Contracts;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Clients;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

[TransientService, SelfService]
public class StartSigningMapper
{
    private const string _signatureAnchorTemplate = "X_SIG_";

    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    private readonly ICustomerServiceClient _customerServiceClient;

    public StartSigningMapper(
        IDocumentArchiveServiceClient documentArchiveServiceClient,
        ICustomerServiceClient customerServiceClient)
    {
        _documentArchiveServiceClient = documentArchiveServiceClient;
        _customerServiceClient = customerServiceClient;
    }

    public async Task<__Entity.DocumentOnSa> WorkflowMapToEntity(StartSigningRequest request, GetTaskDetailResponse taskDetail, CancellationToken cancellationToken)
    {
        var signing = taskDetail.TaskDetail.AmendmentsCase switch
        {
            TaskDetailItem.AmendmentsOneofCase.Signing => taskDetail.TaskDetail.Signing,
            _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.AmendmentHasToBeOfTypeSigning)
        };

        var entity = new __Entity.DocumentOnSa();
        entity.FormId = signing.FormId;
        entity.ExternalId = signing.DocumentForSigning;
        entity.Source = __DbEnum.Source.Workflow;
        entity.EArchivId = await _documentArchiveServiceClient.GenerateDocumentId(new(), cancellationToken);
        entity.SalesArrangementId = request.SalesArrangementId!.Value;
        entity.CaseId = request.CaseId;
        entity.TaskId = request.TaskId;
        entity.SignatureTypeId = taskDetail.TaskObject.SignatureType switch
        {
            "paper" => 1,
            "digital" => 3,
            _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.UnsupportedSbSignatureType, request.TaskId)
        };
        entity.IsValid = true;
        entity.IsSigned = false;
        entity.IsArchived = false;

        return entity;
    }

    public async Task<DocumentOnSa> ServiceRequestMapToEntity(StartSigningRequest request, string formId, GetDocumentDataResponse documentDataResponse, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var entity = new DocumentOnSa();
        entity.DocumentTypeId = request.DocumentTypeId!.Value; // resolved in handler
        entity.DocumentTemplateVersionId = documentDataResponse.DocumentTemplateVersionId;
        entity.DocumentTemplateVariantId = documentDataResponse.DocumentTemplateVariantId;
        entity.FormId = formId;
        entity.EArchivId = await _documentArchiveServiceClient.GenerateDocumentId(new(), cancellationToken);
        entity.SalesArrangementId = request.SalesArrangementId!.Value;
        entity.Data = JsonSerializer.Serialize(documentDataResponse.DocumentData);
        entity.Source = __DbEnum.Source.Noby;
        entity.SignatureTypeId = request.SignatureTypeId;
        //entity.ExternalId = ToDo startSigning uloží ExternalId při elektronickém podpisu
        if (request.CustomerOnSAId1 is null && request.CustomerOnSAId2 is null)// Service request without household
        {
            // For service request without household (CustomerOnSA), we have to get SigningIdentities from SalesArrangement object.Parameters.Applicant 
            await MapSigningIdentitiesFromSalesArrangement(entity, salesArrangement, cancellationToken);
        }
        else // Service request with household
        {
            entity.CustomerOnSAId1 = request.CustomerOnSAId1;
            entity.CustomerOnSAId2 = request.CustomerOnSAId2;
            MapSigningIdentities(request, entity);
        }

        entity.CaseId = request.CaseId;
        entity.IsValid = true;
        entity.IsSigned = false;
        entity.IsArchived = false;
        return entity;
    }

    public async Task<__Entity.DocumentOnSa> ProductRequestMapToEntity(StartSigningRequest request, __Household.Household houseHold, string formId, GetDocumentDataResponse documentDataResponse, CancellationToken cancellationToken)
    {
        var entity = new DocumentOnSa();
        entity.DocumentTypeId = request.DocumentTypeId!.Value;
        entity.DocumentTemplateVersionId = documentDataResponse.DocumentTemplateVersionId;
        entity.DocumentTemplateVariantId = documentDataResponse.DocumentTemplateVariantId;
        entity.FormId = formId;
        entity.EArchivId = await _documentArchiveServiceClient.GenerateDocumentId(new(), cancellationToken);
        entity.SalesArrangementId = request.SalesArrangementId!.Value;
        entity.HouseholdId = houseHold.HouseholdId;
        entity.Data = JsonSerializer.Serialize(documentDataResponse.DocumentData);
        entity.Source = __DbEnum.Source.Noby;
        entity.SignatureTypeId = request.SignatureTypeId;
        entity.CustomerOnSAId1 = request.CustomerOnSAId1;
        entity.CustomerOnSAId2 = request.CustomerOnSAId2;
        //entity.ExternalId = ToDo startSigning uloží ExternalId při elektronickém podpisu
        MapSigningIdentities(request, entity);
        entity.CaseId = request.CaseId;
        entity.IsValid = true;
        entity.IsSigned = false;
        entity.IsArchived = false;
        return entity;
    }

    public async Task<DocumentOnSa> CrsMapToEntity(StartSigningRequest request, string formId, GetDocumentDataResponse documentDataResponse, CancellationToken cancellationToken)
    {
        var entity = new DocumentOnSa();
        entity.DocumentTypeId = request.DocumentTypeId!.Value;
        entity.DocumentTemplateVersionId = documentDataResponse.DocumentTemplateVersionId;
        entity.DocumentTemplateVariantId = documentDataResponse.DocumentTemplateVariantId;
        entity.FormId = formId;
        entity.EArchivId = await _documentArchiveServiceClient.GenerateDocumentId(new(), cancellationToken);
        entity.SalesArrangementId = request.SalesArrangementId!.Value;
        entity.Source = __DbEnum.Source.Noby;
        entity.CustomerOnSAId1 = request.CustomerOnSAId1;
        MapSigningIdentities(request, entity);
        entity.CaseId = request.CaseId;
        entity.SignatureTypeId = request.SignatureTypeId;
        entity.Data = JsonSerializer.Serialize(documentDataResponse.DocumentData);
        entity.IsValid = true;
        entity.IsSigned = false;
        entity.IsArchived = false;
        return entity;
    }

    public StartSigningResponse MapToResponse(DocumentOnSa documentOnSaEntity)
    {
        return new StartSigningResponse
        {
            DocumentOnSa = new DocumentOnSA
            {
                DocumentOnSAId = documentOnSaEntity.DocumentOnSAId,
                DocumentTypeId = documentOnSaEntity.DocumentTypeId,
                FormId = documentOnSaEntity.FormId ?? string.Empty,
                HouseholdId = documentOnSaEntity.HouseholdId,
                IsValid = documentOnSaEntity.IsValid,
                IsSigned = documentOnSaEntity.IsSigned,
                IsArchived = documentOnSaEntity.IsArchived,
                SignatureMethodCode = documentOnSaEntity.SignatureMethodCode ?? string.Empty,
                EArchivId = documentOnSaEntity.EArchivId,
                SignatureTypeId = documentOnSaEntity.SignatureTypeId,
            }
        };
    }

    private async Task MapSigningIdentitiesFromSalesArrangement(DocumentOnSa entity, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var identities = GetIdentities(salesArrangement);

        int index = 0;
        foreach (var identity in identities)
        {
            var customer = await _customerServiceClient.GetCustomerDetail(identity, cancellationToken);

            var entitySigningIdentity = new __Entity.SigningIdentity();
            entitySigningIdentity.SigningIdentityJson = new();
            entitySigningIdentity.SigningIdentityJson.CustomerIdentifiers.Add(new CustomerIdentifier
            {
                IdentityId = identity.IdentityId,
                IdentityScheme = (CIS.Foms.Enums.IdentitySchemes)(int)identity.IdentityScheme
            });

            entitySigningIdentity.SigningIdentityJson.SignatureDataCode = $"{_signatureAnchorTemplate}{++index}";
            entitySigningIdentity.SigningIdentityJson.FirstName = customer.NaturalPerson.FirstName;
            entitySigningIdentity.SigningIdentityJson.LastName = customer.NaturalPerson.LastName;

            foreach (var contact in customer.Contacts)
            {
                switch (contact.DataCase)
                {
                    case CustomerService.Contracts.Contact.DataOneofCase.Mobile:
                        entitySigningIdentity.SigningIdentityJson.MobilePhone = new __Entity.MobilePhone
                        {
                            PhoneNumber = contact.Mobile.PhoneNumber,
                            PhoneIDC = contact.Mobile.PhoneIDC,
                        };
                        break;
                    case CustomerService.Contracts.Contact.DataOneofCase.Email:
                        entitySigningIdentity.SigningIdentityJson.EmailAddress = contact.Email.EmailAddress;
                        break;
                }
            }

            entity.SigningIdentities.Add(entitySigningIdentity);
        }
    }

    private static List<Identity> GetIdentities(SalesArrangement salesArrangement)
    {
        var identities = new List<Identity>();
        switch (salesArrangement.ParametersCase)
        {
            case SalesArrangement.ParametersOneofCase.Drawing:
                identities.Add(salesArrangement.Drawing.Applicant);
                break;
            case SalesArrangement.ParametersOneofCase.GeneralChange:
                identities.Add(salesArrangement.GeneralChange.Applicant);
                break;
            case SalesArrangement.ParametersOneofCase.HUBN:
                identities.Add(salesArrangement.HUBN.Applicant);
                break;
            case SalesArrangement.ParametersOneofCase.CustomerChange:
                identities.AddRange(salesArrangement.CustomerChange.Applicants.Select(s => s.Identity));
                break;
        }

        return identities;
    }

    private static void MapSigningIdentities(StartSigningRequest request, DocumentOnSa entity)
    {
        if (request.CustomerOnSAId1 is not null)
            entity.SigningIdentities.Add(MapSigningIdentity(request.CustomerOnSAId1SigningIdentity));
        if (request.CustomerOnSAId2 is not null)
            entity.SigningIdentities.Add(MapSigningIdentity(request.CustomerOnSAId2SigningIdentity));
    }

    private static __Entity.SigningIdentity MapSigningIdentity(Contracts.SigningIdentity signingIdentity)
    {
        var entity = new __Entity.SigningIdentity();
        entity.SigningIdentityJson = new();
        entity.SigningIdentityJson.CustomerIdentifiers.AddRange(signingIdentity.CustomerIdentifiers.Select(s => new CustomerIdentifier
        {
            IdentityId = s.IdentityId,
            IdentityScheme = (CIS.Foms.Enums.IdentitySchemes)(int)s.IdentityScheme
        }));

        entity.SigningIdentityJson.CustomerOnSAId = signingIdentity.CustomerOnSAId;
        entity.SigningIdentityJson.SignatureDataCode = signingIdentity.SignatureDataCode;
        entity.SigningIdentityJson.FirstName = signingIdentity.FirstName;
        entity.SigningIdentityJson.LastName = signingIdentity.LastName;
        entity.SigningIdentityJson.MobilePhone = new __Entity.MobilePhone
        {
            PhoneNumber = signingIdentity.MobilePhone?.PhoneNumber,
            PhoneIDC = signingIdentity.MobilePhone?.PhoneIDC,
        };
        entity.SigningIdentityJson.EmailAddress = signingIdentity.EmailAddress;
        return entity;
    }
}


