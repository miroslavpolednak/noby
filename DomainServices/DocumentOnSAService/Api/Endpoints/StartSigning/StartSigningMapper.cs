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
using ExternalServices.ESignatures.Dto;
using CIS.Core.Security;
using DomainServices.UserService.Clients;
using CIS.Core;
using DomainServices.CodebookService.Clients;
using System.Globalization;
using DomainServices.CaseService.Clients;
using static ExternalServices.ESignatures.Dto.PrepareDocumentRequest;
using CIS.Foms.Types;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.Infrastructure.gRPC;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

[TransientService, SelfService]
public class StartSigningMapper
{
    private const string _signatureAnchorTemplate = "X_SIG_";
    private readonly IDateTime _dateTime;
    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    private readonly ICustomerServiceClient _customerServiceClient;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ICodebookServiceClient _codebookServiceClient;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorServiceClient;

    public StartSigningMapper(
        IDateTime dateTime,
        IDocumentArchiveServiceClient documentArchiveServiceClient,
        ICustomerServiceClient customerServiceClient,
        ICurrentUserAccessor currentUser,
        IUserServiceClient userServiceClient,
        ICodebookServiceClient codebookServiceClient,
        ICaseServiceClient caseServiceClient,
        IDocumentGeneratorServiceClient documentGeneratorServiceClient)
    {
        _dateTime = dateTime;
        _documentArchiveServiceClient = documentArchiveServiceClient;
        _customerServiceClient = customerServiceClient;
        _currentUser = currentUser;
        _userServiceClient = userServiceClient;
        _codebookServiceClient = codebookServiceClient;
        _caseServiceClient = caseServiceClient;
        _documentGeneratorServiceClient = documentGeneratorServiceClient;
    }

    public async Task<UploadDocumentRequest> MapUploadDocumentRequest(long referenceId, string filename, DocumentOnSa documentOnSa, CancellationToken cancellationToken)
    {
        var docGenRequest = GenerateDocumentRequestMapper.CreateGenerateDocumentRequest(documentOnSa);
        var docGenResponse = await _documentGeneratorServiceClient.GenerateDocument(docGenRequest, cancellationToken);

        return new()
        {
            ReferenceId = referenceId,
            Filename = filename,
            CreationDate = _dateTime.Now,
            FileData = docGenResponse.Data.ToArrayUnsafe()
        };
    }

    public async Task<PrepareDocumentRequest> MapPrepareDocumentRequest(DocumentOnSa documentOnSa, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var currentUser = await _userServiceClient.GetUser(_currentUser.User!.Id, cancellationToken);
        var saUser = await _userServiceClient.GetUser(salesArrangement.Created.UserId!.Value, cancellationToken);
        var documentType = (await _codebookServiceClient.DocumentTypes(cancellationToken)).Single(s => s.Id == documentOnSa.DocumentTypeId);
        var caseObj = await _caseServiceClient.GetCaseDetail(salesArrangement.CaseId, cancellationToken);

        var request = new PrepareDocumentRequest
        {
            CurrentUserInfo = new()
            {
                Cpm = currentUser.UserInfo.Cpm,
                Icp = currentUser.UserInfo.Icp,
                FullName = $"{currentUser.UserInfo.FirstName} {currentUser.UserInfo.LastName}"
            },
            CreatorInfo = new()
            {
                Cpm = saUser.UserInfo.Cpm,
                Icp = saUser.UserInfo.Icp,
                FullName = $"{saUser.UserInfo.FirstName} {saUser.UserInfo.LastName}"
            },
            DocumentData = new()
            {
                DocumentTypeId = documentOnSa.DocumentTypeId!.Value,
                DocumentTemplateVersionId = documentOnSa.DocumentTemplateVersionId!.Value,
                FileName = $"{documentType.FileName}_{salesArrangement.CaseId}_{_dateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf",
                FormId = documentOnSa.FormId,
                ContractNumber = caseObj.Data.ContractNumber
            },

            ClientData = new()
        };
        // Have CustomerOnSA 
        if (documentOnSa.CustomerOnSAId1 is not null)
        {
            var signingIdentity = documentOnSa.SigningIdentities.Single(s => s.SigningIdentityJson.CustomerOnSAId == documentOnSa.CustomerOnSAId1).SigningIdentityJson;
            MapClientData(request.ClientData, signingIdentity);
        }
        else // SigningIdentity from SA it is according to order in collection
        {
            var signingIdentity = documentOnSa.SigningIdentities.First();
            MapClientData(request.ClientData, signingIdentity.SigningIdentityJson);
        }

        request.OtherClients = new();
        if (documentOnSa.SigningIdentities.Count > 1)
        {
            var counter = 1;
            foreach (var item in documentOnSa.SigningIdentities.Skip(1))
            {
                var otherClient = new OtherClient
                {
                    Identities = item.SigningIdentityJson.CustomerIdentifiers.Select(p => new CustomerIdentity(p.IdentityId, p.IdentityScheme)),
                    CodeIndex = ++counter,
                    FullName = $"{item.SigningIdentityJson.FirstName} {item.SigningIdentityJson.LastName}",
                    Phone = item.SigningIdentityJson.MobilePhone?.PhoneNumber,
                    Email = item.SigningIdentityJson.EmailAddress
                };
                request.OtherClients.Add(otherClient);
            };
        }

        return request;
    }

    private static void MapClientData(ClientInfo clientData, SigningIdentityJson signingIdentity)
    {
        clientData.FullName = $"{signingIdentity.FirstName} {signingIdentity.LastName}";
        clientData.BirthNumber = signingIdentity.BirthNumber;
        clientData.Phone = signingIdentity.MobilePhone?.PhoneNumber;
        clientData.Email = signingIdentity.EmailAddress;
        clientData.Identities = signingIdentity.CustomerIdentifiers.Select(s => new CustomerIdentity(s.IdentityId, s.IdentityScheme));
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
        entity.DocumentTypeId = request.DocumentTypeId!.Value; 
        entity.DocumentTemplateVersionId = documentDataResponse.DocumentTemplateVersionId;
        entity.DocumentTemplateVariantId = documentDataResponse.DocumentTemplateVariantId;
        entity.FormId = formId;
        entity.EArchivId = await _documentArchiveServiceClient.GenerateDocumentId(new(), cancellationToken);
        entity.SalesArrangementId = request.SalesArrangementId!.Value;
        entity.Data = JsonSerializer.Serialize(documentDataResponse.DocumentData);
        entity.Source = __DbEnum.Source.Noby;
        entity.SignatureTypeId = request.SignatureTypeId;
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
            entitySigningIdentity.SigningIdentityJson.BirthNumber = customer.NaturalPerson.BirthNumber;
            
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
        entity.SigningIdentityJson.BirthNumber = signingIdentity.BirthNumber;
        entity.SigningIdentityJson.MobilePhone = new __Entity.MobilePhone
        {
            PhoneNumber = signingIdentity.MobilePhone?.PhoneNumber,
            PhoneIDC = signingIdentity.MobilePhone?.PhoneIDC,
        };
        entity.SigningIdentityJson.EmailAddress = signingIdentity.EmailAddress;
        return entity;
    }
}


