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
using SharedTypes.GrpcTypes;
using DomainServices.CustomerService.Clients;
using ExternalServices.ESignatures.Dto;
using CIS.Core.Security;
using DomainServices.UserService.Clients;
using DomainServices.CodebookService.Clients;
using System.Globalization;
using DomainServices.CaseService.Clients.v1;
using static ExternalServices.ESignatures.Dto.PrepareDocumentRequest;
using SharedTypes.Types;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.Infrastructure.gRPC;
using DomainServices.DocumentOnSAService.Api.Extensions;
using SharedTypes.Enums;
using FastEnumUtility;
using DomainServices.ProductService.Clients;
using SharedTypes.Extensions;
using Google.Protobuf.Collections;
using DomainServices.CustomerService.Contracts;
using DomainServices.ProductService.Contracts;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

[TransientService, SelfService]
public class StartSigningMapper
{
    private const string _signatureAnchorTemplate = "X_SIG_";
    private readonly TimeProvider _dateTime;
    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    private readonly ICustomerServiceClient _customerServiceClient;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ICodebookServiceClient _codebookServiceClient;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorServiceClient;
    private readonly IProductServiceClient _productServiceClient;
    private readonly IMediator _mediator;

    public StartSigningMapper(
        TimeProvider dateTime,
        IDocumentArchiveServiceClient documentArchiveServiceClient,
        ICustomerServiceClient customerServiceClient,
        ICurrentUserAccessor currentUser,
        IUserServiceClient userServiceClient,
        ICodebookServiceClient codebookServiceClient,
        ICaseServiceClient caseServiceClient,
        IDocumentGeneratorServiceClient documentGeneratorServiceClient,
        IProductServiceClient productServiceClient,
        IMediator mediator)
    {
        _dateTime = dateTime;
        _documentArchiveServiceClient = documentArchiveServiceClient;
        _customerServiceClient = customerServiceClient;
        _currentUser = currentUser;
        _userServiceClient = userServiceClient;
        _codebookServiceClient = codebookServiceClient;
        _caseServiceClient = caseServiceClient;
        _documentGeneratorServiceClient = documentGeneratorServiceClient;
        _productServiceClient = productServiceClient;
        _mediator = mediator;
    }

    public async Task<UploadDocumentRequest> MapUploadDocumentRequest(long referenceId, string filename, SalesArrangement salesArrangement, DocumentOnSa documentOnSa, CancellationToken cancellationToken)
    {
        var docGenRequest = GenerateDocumentRequestMapper.CreateGenerateDocumentRequest(salesArrangement, documentOnSa);
        var docGenResponse = await _documentGeneratorServiceClient.GenerateDocument(docGenRequest, cancellationToken);

        return new()
        {
            ReferenceId = referenceId,
            Filename = filename,
            CreationDate = _dateTime.GetLocalNow().DateTime,
            FileData = docGenResponse.Data.ToArrayUnsafe()
        };
    }

    public async Task<PrepareDocumentRequest> MapPrepareDocumentRequest(DocumentOnSa documentOnSa, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var currentUser = await _userServiceClient.GetUser(_currentUser.User!.Id, cancellationToken);
        var documentType = (await _codebookServiceClient.DocumentTypes(cancellationToken)).Single(s => s.Id == documentOnSa.DocumentTypeId);
        var caseObj = await _caseServiceClient.GetCaseDetail(salesArrangement.CaseId, cancellationToken);

        GetCustomersOnProductResponse? customersOnProductResponse = null;
        if (salesArrangement.SalesArrangementTypeId == (int)SalesArrangementTypes.CustomerChange)
        {
            customersOnProductResponse = await _productServiceClient.GetCustomersOnProduct(salesArrangement.CaseId, cancellationToken);
        }

        var request = new PrepareDocumentRequest
        {
            ExternalId = await GetExternalId(salesArrangement.CaseId, cancellationToken),
            AdditionalData = $"case_id:{salesArrangement.CaseId}",
            CurrentUserInfo = new()
            {
                Cpm = currentUser.UserInfo.Cpm,
                Icp = currentUser.UserInfo.Icp,
                FullName = $"{currentUser.UserInfo.FirstName} {currentUser.UserInfo.LastName}"
            },
            CreatorInfo = new()
            {
                Cpm = currentUser.UserInfo.Cpm,
                Icp = currentUser.UserInfo.Icp,
                FullName = $"{currentUser.UserInfo.FirstName} {currentUser.UserInfo.LastName}"
            },
            DocumentData = new()
            {
                DocumentTypeId = documentOnSa.DocumentTypeId!.Value,
                DocumentTemplateVersionId = documentOnSa.DocumentTemplateVersionId!.Value,
                FileName = $"{documentType.FileName}_{documentOnSa.DocumentOnSAId}_{_dateTime.GetLocalNow().ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}.pdf",
                FormId = documentOnSa.FormId,
                ContractNumber = caseObj.Data.ContractNumber
            },

            ClientData = new()
        };

        if (salesArrangement.SalesArrangementTypeId == (int)SalesArrangementTypes.CustomerChange) // 9
        {
            await MapClientDataFromKonstDb(request.ClientData, customersOnProductResponse!, cancellationToken);
        }
        else if (documentOnSa.CustomerOnSAId1 is not null)// Have CustomerOnSA
        {
            var signingIdentity = documentOnSa.SigningIdentities.Single(s => s.SigningIdentityJson.CustomerOnSAId == documentOnSa.CustomerOnSAId1).SigningIdentityJson;
            MapClientData(request.ClientData, signingIdentity);
        }
        else // SigningIdentity from SA it is according to order in collection
        {
            var signingIdentity = documentOnSa.SigningIdentities.First();
            MapClientData(request.ClientData, signingIdentity.SigningIdentityJson);
        }

        request.OtherClients = [];

        if (salesArrangement.SalesArrangementTypeId == (int)SalesArrangementTypes.CustomerChange) // 9
        {
            await MapOtherClientsFromKonstDb(request.OtherClients, customersOnProductResponse!, cancellationToken);
        }
        else if (documentOnSa.SigningIdentities.Count > 1)
        {
            MapOtherClientsFromSigningIdentities(documentOnSa, request.OtherClients);
        }

        return request;
    }

    private async Task MapOtherClientsFromKonstDb(List<OtherClient> otherClients, GetCustomersOnProductResponse customersResponse, CancellationToken cancellationToken)
    {
        var relationshipCustomerProductTypes = await _codebookServiceClient.RelationshipCustomerProductTypes(cancellationToken);

        var customersWithRelation = customersResponse.Customers.Select(c => new
        {
            customer = c,
            relation = relationshipCustomerProductTypes.Single(r => r.Id == c.RelationshipCustomerProductTypeId)
        });

        var counter = 1;
        foreach (var customerItem in customersWithRelation.Where(c => c.relation.RdmCode != "A")) //All customers without owner
        {
            var customer = await _customerServiceClient.GetCustomerDetail(customerItem.customer.CustomerIdentifiers.GetKbIdentity(), cancellationToken); //Kb indentita

            var otherClient = new OtherClient
            {
                Identities = customerItem.customer.CustomerIdentifiers.Select(s => new CustomerIdentity(s.IdentityId, (IdentitySchemes)s.IdentityScheme)),
                CodeIndex = ++counter,
                FullName = $"{customer.NaturalPerson.FirstName} {customer.NaturalPerson.LastName}",
                Phone = GetPhoneFromCustomerContacts(customer.Contacts),
                Email = GetEmailFromCustomerContacts(customer.Contacts)
            };

            otherClients.Add(otherClient);
        }
    }

    private static void MapOtherClientsFromSigningIdentities(DocumentOnSa documentOnSa, List<OtherClient> otherClients)
    {
        var counter = 1;
        foreach (var item in documentOnSa.SigningIdentities.Where(c => c.SigningIdentityJson.CustomerOnSAId != documentOnSa.CustomerOnSAId1))
        {
            var otherClient = new OtherClient
            {
                Identities = item.SigningIdentityJson.CustomerIdentifiers.Select(p => new CustomerIdentity(p.IdentityId, p.IdentityScheme)),
                CodeIndex = ++counter,
                FullName = $"{item.SigningIdentityJson.FirstName} {item.SigningIdentityJson.LastName}",
                Phone = string.Concat(item.SigningIdentityJson.MobilePhone?.PhoneIDC, item.SigningIdentityJson.MobilePhone?.PhoneNumber),
                Email = item.SigningIdentityJson.EmailAddress
            };

            otherClients.Add(otherClient);
        };
    }

    private async Task<string> GetExternalId(long caseId, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseServiceClient.GetCaseDetail(caseId, cancellationToken);

        var identityOnCase = (caseDetail.Customer?.Identity)
            ?? throw new NotSupportedException("Customer identity on Case cannot be null");

        if (identityOnCase?.IdentityScheme == Identity.Types.IdentitySchemes.Mp)
        {
            return identityOnCase.IdentityId.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            var kbIdentityId = identityOnCase!.IdentityId;
            var customersResponse = await _productServiceClient.GetCustomersOnProduct(caseId, cancellationToken);
            var customer = customersResponse.Customers.Single(c => c.CustomerIdentifiers.Any(i => i.IdentityId == kbIdentityId));
            var mpIndentity = customer.CustomerIdentifiers.GetKbIdentity();
            return mpIndentity.IdentityId.ToString(CultureInfo.InvariantCulture);
        }
    }

    private async Task MapClientDataFromKonstDb(ClientInfo clientData, GetCustomersOnProductResponse customersResponse, CancellationToken cancellationToken)
    {
        var relationshipCustomerProductTypes = await _codebookServiceClient.RelationshipCustomerProductTypes(cancellationToken);

        var customersWithRelation = customersResponse.Customers.Select(c => new
        {
            customer = c,
            relation = relationshipCustomerProductTypes.Single(r => r.Id == c.RelationshipCustomerProductTypeId)
        });

        var customerOwner = customersWithRelation.Single(c => c.relation.RdmCode == "A"); // A mean Owner (Hlavní dlužník)
        var customer = await _customerServiceClient.GetCustomerDetail(customerOwner.customer.CustomerIdentifiers.GetKbIdentity(), cancellationToken); //Kb indentita

        clientData.FullName = $"{customer.NaturalPerson.FirstName} {customer.NaturalPerson.LastName}";
        clientData.BirthNumber = customer.NaturalPerson.BirthNumber;
        clientData.Phone = GetPhoneFromCustomerContacts(customer.Contacts);
        clientData.Email = GetEmailFromCustomerContacts(customer.Contacts);
        clientData.Identities = customerOwner.customer.CustomerIdentifiers.Select(s => new CustomerIdentity(s.IdentityId, (IdentitySchemes)s.IdentityScheme));
    }

    private static string? GetEmailFromCustomerContacts(RepeatedField<Contact> contacts)
    {
        var emailContact = contacts.FirstOrDefault(c => c.DataCase == Contact.DataOneofCase.Email);
        return emailContact?.Email.EmailAddress;
    }

    private static string? GetPhoneFromCustomerContacts(RepeatedField<Contact> contacts)
    {
        var phoneContact = contacts.FirstOrDefault(c => c.DataCase == Contact.DataOneofCase.Mobile);
        return phoneContact is not null ? string.Concat(phoneContact.Mobile.PhoneIDC, phoneContact.Mobile.PhoneNumber) : null;
    }

    private static void MapClientData(ClientInfo clientData, SigningIdentityJson signingIdentity)
    {
        clientData.FullName = $"{signingIdentity.FirstName} {signingIdentity.LastName}";
        clientData.BirthNumber = signingIdentity.BirthNumber;
        clientData.Phone = string.Concat(signingIdentity.MobilePhone?.PhoneIDC, signingIdentity.MobilePhone?.PhoneNumber);
        clientData.Email = signingIdentity.EmailAddress;
        clientData.Identities = signingIdentity.CustomerIdentifiers.Select(s => new CustomerIdentity(s.IdentityId, s.IdentityScheme));
    }

    public async Task<DocumentOnSa> WorkflowMapToEntity(StartSigningRequest request, GetTaskDetailResponse taskDetail, CancellationToken cancellationToken)
    {
        var signing = taskDetail.TaskDetail.AmendmentsCase switch
        {
            TaskDetailItem.AmendmentsOneofCase.Signing => taskDetail.TaskDetail.Signing,
            _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.AmendmentHasToBeOfTypeSigning)
        };

        var entity = new DocumentOnSa();
        entity.FormId = signing.FormId;
        entity.ExternalIdSb = signing.DocumentForSigning;
        if (request.SignatureTypeId == SignatureTypes.Electronic.ToByte())
        {
            entity.ExternalIdESignatures = await GetExternalIdESignaturesFromSbQueue(signing, cancellationToken);
        }
        entity.Source = __DbEnum.Source.Workflow;
        entity.EArchivId = await _documentArchiveServiceClient.GenerateDocumentId(new(), cancellationToken);
        entity.SalesArrangementId = request.SalesArrangementId!.Value;
        entity.CaseId = request.CaseId;
        entity.TaskId = request.TaskId;
        entity.TaskIdSb = request.TaskIdSb;
        entity.SignatureTypeId = taskDetail.TaskObject.SignatureTypeId;
        entity.IsValid = true;
        entity.IsSigned = false;
        entity.IsArchived = false;
        entity.IsCustomerPreviewSendingAllowed = true;
        entity.EACodeMainId = signing.EACodeMain;
        return entity;
    }

    public async Task<DocumentOnSa> ServiceRequestMapToEntity(StartSigningRequest request, __Household.Household? houseHold, string formId, GetDocumentDataResponse documentDataResponse, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var entity = new DocumentOnSa();
        entity.DocumentTypeId = request.DocumentTypeId!.Value;
        entity.DocumentTemplateVersionId = documentDataResponse.DocumentTemplateVersionId;
        entity.DocumentTemplateVariantId = documentDataResponse.DocumentTemplateVariantId;
        entity.FormId = formId;
        entity.EArchivId = await _documentArchiveServiceClient.GenerateDocumentId(new(), cancellationToken);
        entity.SalesArrangementId = request.SalesArrangementId!.Value;
        entity.HouseholdId = houseHold?.HouseholdId;
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

    public async Task<DocumentOnSa> ProductRequestMapToEntity(StartSigningRequest request, __Household.Household houseHold, string formId, GetDocumentDataResponse documentDataResponse, CancellationToken cancellationToken)
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
                EArchivId = documentOnSaEntity.EArchivId,
                SignatureTypeId = documentOnSaEntity.SignatureTypeId,
                Source = documentOnSaEntity.Source.MapToContractEnum(),
                SalesArrangementId = documentOnSaEntity.SalesArrangementId,
                EACodeMainId = documentOnSaEntity.EACodeMainId
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
                IdentityScheme = (SharedTypes.Enums.IdentitySchemes)(int)identity.IdentityScheme
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
                identities.Add(salesArrangement.Drawing.Applicant.GetIdentity(Identity.Types.IdentitySchemes.Kb));
                break;
            case SalesArrangement.ParametersOneofCase.GeneralChange:
                identities.Add(salesArrangement.GeneralChange.Applicant.GetIdentity(Identity.Types.IdentitySchemes.Kb));
                break;
            case SalesArrangement.ParametersOneofCase.HUBN:
                identities.Add(salesArrangement.HUBN.Applicant.GetIdentity(Identity.Types.IdentitySchemes.Kb));
                break;
            case SalesArrangement.ParametersOneofCase.CustomerChange:
                identities.AddRange(salesArrangement.CustomerChange.Applicants.Select(s => s.Identity.GetIdentity(Identity.Types.IdentitySchemes.Kb)));
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
            IdentityScheme = (SharedTypes.Enums.IdentitySchemes)(int)s.IdentityScheme
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

    private async Task<string?> GetExternalIdESignaturesFromSbQueue(AmendmentSigning signing, CancellationToken cancellationToken)
    {
        if (signing.DocumentForSigningType.Equals("D", StringComparison.OrdinalIgnoreCase))
        {
            var docRequest = new GetElectronicDocumentFromQueueRequest
            {
                MainDocument = new()
                {
                    DocumentId = signing.DocumentForSigning
                },
                GetMetadataOnly = true
            };

            var docResponse = await _mediator.Send(docRequest, cancellationToken);
            return docResponse.ExternalIdESignatures;
        }
        else
        {
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.UnsupportedDocumentForSigningType, signing.DocumentForSigningType);
        }
    }
}


