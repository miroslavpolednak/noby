﻿using SharedTypes.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using FastEnumUtility;
using NOBY.Api.Endpoints.Customer;
using NOBY.Api.Endpoints.Customer.GetCustomerDetailWithChanges;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using NOBY.Api.Endpoints.SalesArrangement.SharedDto;
using _DocOnSA = DomainServices.DocumentOnSAService.Contracts;
using ValidateSalesArrangementRequest = NOBY.Api.Endpoints.SalesArrangement.ValidateSalesArrangement.ValidateSalesArrangementRequest;
using NOBY.Api.Extensions;
using NOBY.Services.SalesArrangementAuthorization;

namespace NOBY.Api.Endpoints.DocumentOnSA.StartSigning;

internal sealed class StartSigningHandler : IRequestHandler<StartSigningRequest, StartSigningResponse>
{
    private const string _signatureAnchorOne = "X_SIG_1";
    private const string _signatureAnchorTwo = "X_SIG_2";

    private readonly IDocumentOnSAServiceClient _client;
    private readonly ICodebookServiceClient _codebookServiceClient;
    private readonly ICustomerOnSAServiceClient _customerOnSAServiceClient;
    private readonly CustomerWithChangedDataService _changedDataService;
    private readonly IMediator _mediator;
    private readonly ISalesArrangementServiceClient _salesArrangementServiceClient;
    private readonly IHouseholdServiceClient _householdClient;

    public StartSigningHandler(
        IDocumentOnSAServiceClient client,
        ICodebookServiceClient codebookServiceClient,
        ICustomerOnSAServiceClient customerOnSAServiceClient,
        CustomerWithChangedDataService changedDataService,
        IMediator mediator,
        ISalesArrangementServiceClient salesArrangementServiceClient,
        IHouseholdServiceClient householdClient)
    {
        _client = client;
        _codebookServiceClient = codebookServiceClient;
        _customerOnSAServiceClient = customerOnSAServiceClient;
        _changedDataService = changedDataService;
        _mediator = mediator;
        _salesArrangementServiceClient = salesArrangementServiceClient;
        _householdClient = householdClient;
    }

    public async Task<StartSigningResponse> Handle(StartSigningRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementServiceClient.GetSalesArrangement(request.SalesArrangementId!.Value, cancellationToken);

        // nesmi se jedna o refinancovani
        if (ISalesArrangementAuthorizationService.RefinancingSATypes.Contains(salesArrangement.SalesArrangementTypeId))
        {
            throw new NobyValidationException(90032);
        }

        if (salesArrangement.SalesArrangementTypeId == SalesArrangementTypes.Mortgage.ToByte() // 1
            || salesArrangement.SalesArrangementTypeId == SalesArrangementTypes.Drawing.ToByte() // 6
            || salesArrangement.SalesArrangementTypeId == SalesArrangementTypes.CustomerChange3602A.ToByte() // 10 
            || salesArrangement.SalesArrangementTypeId == SalesArrangementTypes.CustomerChange3602B.ToByte() // 11
            || salesArrangement.SalesArrangementTypeId == SalesArrangementTypes.CustomerChange3602C.ToByte() // 12
            )
        {
            // CheckForm
            await ValidateSalesArrangement(request.SalesArrangementId!.Value, cancellationToken);
        }

        int? customerOnSAId1 = null;
        int? customerOnSAId2 = null;

        // CRS request
        if (request.DocumentTypeId == DocumentTypes.DANRESID.ToByte()) // 13
        {
            customerOnSAId1 = request.CustomerOnSAId;
        }
        else
        {
            var houseHolds = await _householdClient.GetHouseholdList(request.SalesArrangementId!.Value, cancellationToken);
            //Product request (4, 5)
            if (request.DocumentTypeId == DocumentTypes.ZADOSTHU.ToByte() || request.DocumentTypeId == DocumentTypes.ZADOSTHD.ToByte())
            {
                var householdTypeId = GetHouseholdTypeId((DocumentTypes)request.DocumentTypeId);
                var houseHold = houseHolds.SingleOrDefault(r => r.HouseholdTypeId == householdTypeId)
                    ?? throw new NobyValidationException($"Household doesn't exist for specified SalesArrangementId {request.SalesArrangementId}");
                customerOnSAId1 = houseHold.CustomerOnSAId1;
                customerOnSAId2 = houseHold.CustomerOnSAId2;
            }
            //ServiceRequest with household (11 ,12, 16)
            else if (request.DocumentTypeId == DocumentTypes.ZUSTAVSI.ToByte() // 11
                            || request.DocumentTypeId == DocumentTypes.PRISTOUP.ToByte() // 12
                            || request.DocumentTypeId == DocumentTypes.ZADOSTHD_SERVICE.ToByte()) // 16
            {
                var houseHold = houseHolds.SingleOrDefault(r => r.HouseholdTypeId == HouseholdTypes.Codebtor.ToByte())
                 ?? throw new NobyValidationException($"Household doesn't exist for specified SalesArrangementId {request.SalesArrangementId}");
                customerOnSAId1 = houseHold.CustomerOnSAId1;
                customerOnSAId2 = houseHold.CustomerOnSAId2;
            }
            else // Service request without households
            {
                if (houseHolds.Any())
                    throw new NobyValidationException($"Households should by empty for service request without household (DocumentTypeId: {request.DocumentTypeId})");
            }
        }

        var result = await _client.StartSigning(new()
        {
            DocumentTypeId = request.DocumentTypeId,
            SalesArrangementId = request.SalesArrangementId,
            SignatureTypeId = request.SignatureTypeId,
            CustomerOnSAId1 = customerOnSAId1,
            CustomerOnSAId2 = customerOnSAId2,
            CustomerOnSAId1SigningIdentity = customerOnSAId1 is not null ? await MapCustomerOnSAIdentity(customerOnSAId1.Value, _signatureAnchorOne, cancellationToken) : null,
            CustomerOnSAId2SigningIdentity = customerOnSAId2 is not null ? await MapCustomerOnSAIdentity(customerOnSAId2.Value, _signatureAnchorTwo, cancellationToken) : null
        }, cancellationToken);

        return await MapToResponse(result, salesArrangement.SalesArrangementTypeId, cancellationToken);
    }

    /// <summary>
    /// Checkform 
    /// </summary>
    private async Task ValidateSalesArrangement(int salesArrangementId, CancellationToken cancellationToken)
    {
        var validationResult = await _mediator.Send(new ValidateSalesArrangementRequest(salesArrangementId), cancellationToken);

        if (validationResult.Categories?.Any(HasCategoryAnyError) ?? false)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var errors = validationResult.Categories.Select(s => new CisExceptionItem(90009, JsonSerializer.Serialize(s, options)));
            throw new CisValidationException(errors);
        }
    }

    private static bool HasCategoryAnyError(ValidateCategory category) => category.ValidationMessages.Any(HasMessageError);

    private static bool HasMessageError(ValidateMessage message) => message.Severity == MessageSeverity.Error;

    private async Task<_DocOnSA.SigningIdentity> MapCustomerOnSAIdentity(int customerOnSAId, string signatureAnchor, CancellationToken cancellationToken)
    {
        var customerOnSa = await _customerOnSAServiceClient.GetCustomer(customerOnSAId, cancellationToken);
        var detailWithChangedData = await _changedDataService.GetCustomerWithChangedData<GetCustomerDetailWithChangesResponse>(customerOnSa, cancellationToken);

        var signingIdentity = new _DocOnSA.SigningIdentity();
        
        // Product, CRS and Service with household mapping
        signingIdentity.CustomerIdentifiers.AddRange(customerOnSa.CustomerIdentifiers.Select(s => new SharedTypes.GrpcTypes.Identity
        {
            IdentityId = s.IdentityId,
            IdentityScheme = s.IdentityScheme
        }));

        signingIdentity.CustomerOnSAId = customerOnSAId;
        signingIdentity.SignatureDataCode = signatureAnchor;
        signingIdentity.FirstName = detailWithChangedData.NaturalPerson?.FirstName;
        signingIdentity.LastName = detailWithChangedData.NaturalPerson?.LastName;
        signingIdentity.MobilePhone = new _DocOnSA.MobilePhone
        {
            PhoneNumber = detailWithChangedData.MobilePhone?.PhoneNumber,
            PhoneIDC = detailWithChangedData.MobilePhone?.PhoneIDC
        };
        signingIdentity.EmailAddress = detailWithChangedData.EmailAddress?.EmailAddress;
        signingIdentity.BirthNumber = detailWithChangedData.NaturalPerson?.BirthNumber;
        return signingIdentity;
    }

    private async Task<StartSigningResponse> MapToResponse(_DocOnSA.StartSigningResponse result, int salesArrangementTypeId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(result.DocumentOnSa, nameof(result.DocumentOnSa));

        var documentTypes = await _codebookServiceClient.DocumentTypes(cancellationToken);
        var eACodeMains = await _codebookServiceClient.EaCodesMain(cancellationToken);
        var signatureStates = await _codebookServiceClient.SignatureStatesNoby(cancellationToken);

        return new StartSigningResponse
        {
            DocumentOnSAId = result.DocumentOnSa.DocumentOnSAId,
            DocumentTypeId = result.DocumentOnSa.DocumentTypeId,
            FormId = result.DocumentOnSa.FormId,
            IsSigned = result.DocumentOnSa.IsSigned,
            SignatureTypeId = result.DocumentOnSa.SignatureTypeId,
            SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
            {
                IsValid = result.DocumentOnSa.IsValid,
                DocumentOnSAId = result.DocumentOnSa.DocumentOnSAId,
                IsSigned = result.DocumentOnSa.IsSigned,
                Source = result.DocumentOnSa.Source.MapToCisEnum(),
                SalesArrangementTypeId = salesArrangementTypeId,
                EArchivIdsLinked = Array.Empty<string>(),
                SignatureTypeId = result.DocumentOnSa.SignatureTypeId ?? 0
            },
              signatureStates),
            EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(
                new() { DocumentTypeId = result.DocumentOnSa.DocumentTypeId, EACodeMainId = result.DocumentOnSa.EACodeMainId }, documentTypes, eACodeMains)
        };
    }

    private static int GetHouseholdTypeId(DocumentTypes documentType) => documentType switch
    {
        // 4 
        DocumentTypes.ZADOSTHU => HouseholdTypes.Main.ToByte(),// 1,
        // 5 
        DocumentTypes.ZADOSTHD => HouseholdTypes.Codebtor.ToByte(),// 2
        _ => throw new NobyValidationException($"DocumentTypeId {documentType} not supported for ProductRequest")
    };

}
