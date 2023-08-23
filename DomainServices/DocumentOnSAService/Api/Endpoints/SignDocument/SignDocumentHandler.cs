using CIS.Core;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.Infrastructure.Audit;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.DocumentOnSAService.Api.Common;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.Eas.V1;
using ExternalServices.Sulm.V1;
using FastEnumUtility;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using static DomainServices.HouseholdService.Contracts.GetCustomerChangeMetadataResponse.Types;
using _CustomerService = DomainServices.CustomerService.Contracts;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SignDocument;

public sealed class SignDocumentHandler : IRequestHandler<SignDocumentRequest, Empty>
{
    public List<CustomerOnSA> CustomersOnSABuffer { get; set; } = new();

    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IEasClient _easClient;
    private readonly ISulmClientHelper _sulmClientHelper;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IProductServiceClient _productService;
    private readonly IAuditLogger _auditLogger;
    private readonly ISalesArrangementStateManager _salesArrangementStateManager;
    private readonly ICustomerChangeDataMerger _customerChangeDataMerger;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICaseServiceClient _caseService;

    public SignDocumentHandler(
        DocumentOnSAServiceDbContext dbContext,
        IDateTime dateTime,
        ICurrentUserAccessor currentUser,
        ISalesArrangementServiceClient salesArrangementService,
        IEasClient easClient,
        ISulmClientHelper sulmClientHelper,
        IHouseholdServiceClient householdService,
        ICustomerOnSAServiceClient customerOnSaService,
        IProductServiceClient productService,
        IAuditLogger auditLogger,
        ISalesArrangementStateManager salesArrangementStateManager,
        ICustomerChangeDataMerger customerChangeDataMerger,
        ICustomerServiceClient customerService,
        ICodebookServiceClient codebookService,
        ICaseServiceClient caseService)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
        _currentUser = currentUser;
        _salesArrangementService = salesArrangementService;
        _easClient = easClient;
        _sulmClientHelper = sulmClientHelper;
        _householdService = householdService;
        _customerOnSAService = customerOnSaService;
        _productService = productService;
        _auditLogger = auditLogger;
        _salesArrangementStateManager = salesArrangementStateManager;
        _customerChangeDataMerger = customerChangeDataMerger;
        _customerService = customerService;
        _codebookService = codebookService;
        _caseService = caseService;
    }

    public async Task<Empty> Handle(SignDocumentRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FirstOrDefaultAsync(r => r.DocumentOnSAId == request.DocumentOnSAId!.Value, cancellationToken);

        if (documentOnSa is null)
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId!.Value);

        if (documentOnSa.IsSigned)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.AlreadySignedDocumentOnSA, request.DocumentOnSAId!.Value);

        var salesArrangement = await _salesArrangementService.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);

        if (salesArrangement.State != SalesArrangementStates.InSigning.ToByte())
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SigningInvalidSalesArrangementState);

        if (documentOnSa.IsValid == false || documentOnSa.IsSigned || documentOnSa.IsFinal)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UnableToStartSigningOrSignInvalidDocument);

        var signatureDate = _dateTime.Now;

        UpdateDocumentOnSa(documentOnSa, signatureDate);

        await AddSignatureIfNotSetYet(documentOnSa, salesArrangement, signatureDate, cancellationToken);

        // Update Mortgage.FirstSignatureDate
        if (documentOnSa.DocumentTypeId == DocumentTypes.ZADOSTHU.ToByte()) // 4
            await UpdateFirstSignatureDate(signatureDate, salesArrangement, cancellationToken);

        var houseHold = documentOnSa.HouseholdId.HasValue
            ? await _householdService.GetHousehold(documentOnSa.HouseholdId!.Value, cancellationToken)
            : null;

        // SUML call
        if (documentOnSa.DocumentTypeId == DocumentTypes.ZADOSTHU.ToByte()) // 4 
            await SumlCall(salesArrangement, houseHold!, cancellationToken);

        if (houseHold is null)
        {
            // CRS
            if (documentOnSa.DocumentTypeId == DocumentTypes.DANRESID.ToByte()) // 13
            {
                await ProcessCrsDocumentOnSa(documentOnSa, cancellationToken);
            }
        }
        else
        {
            await SetFlowSwitch(houseHold!, cancellationToken);
            await ProcessDocumentOnSaWithHousehold(salesArrangement, houseHold, documentOnSa, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        _auditLogger.LogWithCurrentUser(
            AuditEventTypes.Noby007,
            "Dokument byl označen za podepsaný",
            products: new List<AuditLoggerHeaderItem>
            {
                new("case", salesArrangement.CaseId),
                new("salesArrangement", salesArrangement.SalesArrangementId),
                new("form", documentOnSa.FormId)
            }
        );

        // SA state
        if (salesArrangement.State == SalesArrangementStates.InSigning.ToByte())
        {
            await _salesArrangementStateManager.SetSalesArrangementStateAccordingDocumentsOnSa(salesArrangement.SalesArrangementId, cancellationToken);
        }
        else
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.SigningInvalidSalesArrangementState);
        }

        return new Empty();
    }

    private async Task ProcessCrsDocumentOnSa(DocumentOnSa documentOnSa, CancellationToken cancellationToken)
    {
        var customerOnSa = CustomersOnSABuffer.FirstOrDefault(c => c.CustomerOnSAId == documentOnSa.CustomerOnSAId1!.Value)
                                    ?? await _customerOnSAService.GetCustomer(documentOnSa.CustomerOnSAId1!.Value, cancellationToken);

        var customerChangeMetadata = await GetCustomerOnSaMetadata(documentOnSa, customerOnSa, cancellationToken);

        if (customerChangeMetadata.CustomerChangeMetadata.WasCRSChanged)
        {
            var customerDetail = await _customerService.GetCustomerDetail(customerOnSa.CustomerIdentifiers.First(r => r.IdentityScheme == Identity.Types.IdentitySchemes.Kb), cancellationToken);
            _customerChangeDataMerger.MergeTaxResidence(customerDetail?.NaturalPerson!, customerOnSa);
            var updateCustomerRequest = MapUpdateCustomerRequest((int)CIS.Foms.Enums.Mandants.Kb, customerDetail!);
            await _customerService.UpdateCustomer(updateCustomerRequest, cancellationToken);
            // Throw away locally stored CRS data (keep client changes) 
            var jsonCustomerChangeDataWithoutCrs = _customerChangeDataMerger.TrowAwayLocallyStoredCrsData(customerOnSa);
            await _customerOnSAService.UpdateCustomerDetail(MapUpdateCustomerOnSaRequest(customerOnSa, jsonCustomerChangeDataWithoutCrs), cancellationToken);
        }
    }

    private async Task ProcessDocumentOnSaWithHousehold(SalesArrangement salesArrangement, Household household, DocumentOnSa documentOnSa, CancellationToken cancellationToken)
    {
        var mandantId = await GetMandantId(salesArrangement, cancellationToken);

        if (mandantId != (int)CIS.Foms.Enums.Mandants.Kb)
        {
            throw new CisValidationException(90002, $"Mp products not supported (mandant {mandantId})");
        }

        var customersOnSa = await GetCustomersOnSa(household, cancellationToken);
        foreach (var customerOnSa in customersOnSa)
        {
            var customerChangeMetadata = await GetCustomerOnSaMetadata(documentOnSa, customerOnSa, cancellationToken);
            if (customerChangeMetadata.CustomerChangeMetadata.WereClientDataChanged)
            {
                var customerDetail = await _customerService.GetCustomerDetail(customerOnSa.CustomerIdentifiers.First(r => r.IdentityScheme == Identity.Types.IdentitySchemes.Kb), cancellationToken);
                _customerChangeDataMerger.MergeClientData(customerDetail, customerOnSa);
                var updateCustomerRequest = MapUpdateCustomerRequest(mandantId.Value, customerDetail);
                await _customerService.UpdateCustomer(updateCustomerRequest, cancellationToken);
                //Throw away locally stored Client data (keep CRS changes) 
                var jsonCustomerChangeDataWithCrs = _customerChangeDataMerger.TrowAwayLocallyStoredClientData(customerOnSa);
                await _customerOnSAService.UpdateCustomerDetail(MapUpdateCustomerOnSaRequest(customerOnSa, jsonCustomerChangeDataWithCrs), cancellationToken);
            }
        }
    }

    private async Task<int?> GetMandantId(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(salesArrangement.CaseId, cancellationToken);
        var productTypes = await _codebookService.ProductTypes(cancellationToken);
        return productTypes.Single(r => r.Id == caseDetail.Data.ProductTypeId).MandantId;
    }

    private async Task<List<CustomerOnSA>> GetCustomersOnSa(Household household, CancellationToken cancellationToken)
    {
        var customers = new List<CustomerOnSA>();
        if (household.CustomerOnSAId1 is not null)
        {
            customers.Add(CustomersOnSABuffer.FirstOrDefault(c => c.CustomerOnSAId == household.CustomerOnSAId1.Value)
                                    ?? await _customerOnSAService.GetCustomer(household.CustomerOnSAId1.Value, cancellationToken));
        }
        if (household.CustomerOnSAId2 is not null)
        {
            customers.Add(CustomersOnSABuffer.FirstOrDefault(c => c.CustomerOnSAId == household.CustomerOnSAId2.Value)
                                    ?? await _customerOnSAService.GetCustomer(household.CustomerOnSAId2.Value, cancellationToken));
        }

        return customers;
    }

    private static UpdateCustomerDetailRequest MapUpdateCustomerOnSaRequest(CustomerOnSA customerOnSa, string? customerChangeDataJson)
    {
        return new UpdateCustomerDetailRequest
        {
            CustomerOnSAId = customerOnSa.CustomerOnSAId,
            CustomerAdditionalData = customerOnSa.CustomerAdditionalData,
            CustomerChangeData = customerChangeDataJson
        };
    }

    private static _CustomerService.UpdateCustomerRequest MapUpdateCustomerRequest(int mandantId, CustomerDetailResponse customerDetail)
    {
        return new _CustomerService.UpdateCustomerRequest
        {
            Addresses = { customerDetail.Addresses },
            Contacts = { customerDetail.Contacts },
            CustomerIdentification = customerDetail.CustomerIdentification,
            IdentificationDocument = customerDetail.IdentificationDocument,
            Identities = { customerDetail.Identities },
            Mandant = (CIS.Infrastructure.gRPC.CisTypes.Mandants)mandantId,
            NaturalPerson = customerDetail.NaturalPerson
        };
    }

    private async Task<GetCustomerChangeMetadataResponseItem> GetCustomerOnSaMetadata(DocumentOnSa documentOnSa, CustomerOnSA customerOnSa, CancellationToken cancellationToken)
    {
        var customersChangeMetadata = await _customerOnSAService.GetCustomerChangeMetadata(documentOnSa.SalesArrangementId, cancellationToken);
        return customersChangeMetadata!.Single(r => r.CustomerOnSAId == customerOnSa.CustomerOnSAId);
    }

    private async Task SetFlowSwitch(Household houseHold, CancellationToken cancellationToken)
    {
        var flowSwitchId = houseHold.HouseholdTypeId switch
        {
            (int)HouseholdTypes.Main => FlowSwitches.Was3601MainChangedAfterSigning,
            (int)HouseholdTypes.Codebtor => FlowSwitches.Was3602CodebtorChangedAfterSigning,
            _ => throw new CisValidationException("Unsupported HouseholdType")
        };

        await _salesArrangementService.SetFlowSwitches(houseHold.SalesArrangementId, new()
            {
                new() { FlowSwitchId = (int)flowSwitchId, Value = false }
            }, cancellationToken);
    }

    private async Task UpdateFirstSignatureDate(DateTime signatureDate, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        //SalesArrangement parameters
        salesArrangement.Mortgage.FirstSignatureDate = signatureDate;
        await _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Mortgage = salesArrangement.Mortgage
        },
            cancellationToken);

        //KonsDb 
        var mortgageResponse = await _productService.GetMortgage(salesArrangement.CaseId, cancellationToken);
        mortgageResponse.Mortgage.FirstSignatureDate = signatureDate;

        await _productService.UpdateMortgage(new UpdateMortgageRequest { ProductId = salesArrangement.CaseId, Mortgage = mortgageResponse.Mortgage }, cancellationToken);
    }

    private async Task SumlCall(SalesArrangement salesArrangement, Household houseHold, CancellationToken cancellationToken)
    {
        if (salesArrangement.IsProductSalesArrangement())
        {
            await SumlCallForSpecifiedCustomer(houseHold.CustomerOnSAId1!.Value, cancellationToken);

            if (houseHold.CustomerOnSAId2 is not null)
            {
                await SumlCallForSpecifiedCustomer(houseHold.CustomerOnSAId2.Value, cancellationToken);
            }
        }
    }

    private async Task SumlCallForSpecifiedCustomer(int customerOnSaId, CancellationToken cancellationToken)
    {
        var customerOnSa = await _customerOnSAService.GetCustomer(customerOnSaId, cancellationToken);
        CustomersOnSABuffer.Add(customerOnSa);

        var kbIdentity = customerOnSa.CustomerIdentifiers.First(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
        await _sulmClientHelper.StartUse(kbIdentity.IdentityId, ISulmClient.PurposeMLAP, cancellationToken);
    }

    private async Task AddSignatureIfNotSetYet(DocumentOnSa documentOnSa, SalesArrangement salesArrangement, DateTime signatureDate, CancellationToken cancellationToken)
    {
        if (documentOnSa.DocumentTypeId == DocumentTypes.ZADOSTHU.ToByte()
            && await _dbContext.DocumentOnSa.Where(d => d.SalesArrangementId == documentOnSa.SalesArrangementId).AllAsync(r => r.IsSigned == false, cancellationToken))
        {
            var result = await _easClient.AddFirstSignatureDate((int)salesArrangement.CaseId, signatureDate, cancellationToken);

            if (result is not null && result.CommonValue != 0)
            {
                throw new CisValidationException(ErrorCodeMapper.EasAddFirstSignatureDateReturnedErrorState, $"Eas AddFirstSignatureDate returned error state: {result.CommonValue} with message: {result.CommonText}");
            }
        }
    }

    private void UpdateDocumentOnSa(DocumentOnSa documentOnSa, DateTime signatureDate)
    {
        documentOnSa.IsSigned = true;
        documentOnSa.SignatureDateTime = signatureDate;
        documentOnSa.SignatureConfirmedBy = _currentUser.User?.Id;
    }
}
