using CIS.Core;
using CIS.Core.Security;
using SharedTypes.Enums;
using SharedAudit;
using SharedTypes.GrpcTypes;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
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
using DomainServices.UserService.Clients;
using DomainServices.UserService.Contracts;
using ExternalServices.Eas.V1;
using ExternalServices.Sulm.V1;
using FastEnumUtility;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using static DomainServices.HouseholdService.Contracts.GetCustomerChangeMetadataResponse.Types;
using _CustomerService = DomainServices.CustomerService.Contracts;
using Source = DomainServices.DocumentOnSAService.Api.Database.Enums.Source;
using DomainServices.DocumentOnSAService.Api.Extensions;
using DomainServices.HouseholdService.Contracts.Model;
using SharedTypes.Extensions;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SignDocument;

public sealed class SignDocumentHandler : IRequestHandler<SignDocumentRequest, Empty>
{
    public List<CustomerOnSA> CustomersOnSABuffer { get; set; } = [];

    /// <summary>
    /// Unwritten data to KB CURE
    /// </summary>
    private const int _unwrittenDataEaCodeMainId = 616538;

    private const string _defaultContractNumber = "HF00111111125";

    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly TimeProvider _dateTime;
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
    private readonly ILogger<SignDocumentHandler> _logger;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly IUserServiceClient _userService;
    private readonly ICommonSigningMethods _commonSigningMethods;

    public SignDocumentHandler(
        DocumentOnSAServiceDbContext dbContext,
        TimeProvider dateTime,
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
        ICaseServiceClient caseService,
        ILogger<SignDocumentHandler> logger,
        IDocumentArchiveServiceClient documentArchiveService,
        IUserServiceClient userService,
        ICommonSigningMethods commonSigningMethods)
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
        _logger = logger;
        _documentArchiveService = documentArchiveService;
        _userService = userService;
        _commonSigningMethods = commonSigningMethods;
    }

    public async Task<Empty> Handle(SignDocumentRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FirstOrDefaultAsync(r => r.DocumentOnSAId == request.DocumentOnSAId!.Value, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId!.Value);

        if (!documentOnSa.IsValid || documentOnSa.IsSigned || documentOnSa.IsFinal)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UnableToStartSigningOrSignInvalidDocument);

        if (documentOnSa.SignatureTypeId != request.SignatureTypeId)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SignatureTypeIdHasToBeSame);

        var salesArrangement = await _salesArrangementService.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);

        if (documentOnSa.Source != Source.Workflow && salesArrangement.State != SalesArrangementStates.InSigning.ToByte())
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SigningInvalidSalesArrangementState);

        var signatureDate = _dateTime.GetLocalNow().DateTime;

        UpdateDocumentOnSa(documentOnSa, signatureDate);

        var houseHold = documentOnSa.HouseholdId.HasValue
                    ? await _householdService.GetHousehold(documentOnSa.HouseholdId!.Value, cancellationToken)
                    : null;

        // SUML call for all those document types, household should be not null 
        if (IsDocumentTypeWithHousehold(documentOnSa.DocumentTypeId.GetValueOrDefault()))
        {
            await SumlCall(houseHold!, cancellationToken);
        }

        var wasUpdateOfCustomersSuccessful = true;

        if (houseHold is null)
        {
            // CRS
            if (documentOnSa.DocumentTypeId.GetValueOrDefault() == DocumentTypes.DANRESID.ToByte()) // 13
            {
                wasUpdateOfCustomersSuccessful = await ProcessCrsDocumentOnSa(documentOnSa, salesArrangement, cancellationToken);
            }
        }
        else
        {
            await SetFlowSwitch(houseHold!, cancellationToken);
            wasUpdateOfCustomersSuccessful = await ProcessDocumentOnSaWithHousehold(salesArrangement, houseHold, documentOnSa, cancellationToken);
        }

        await AddFirstSignatureDateIfNotSetYet(documentOnSa, salesArrangement, signatureDate, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        LogAuditMessage(documentOnSa, salesArrangement);

        if (documentOnSa.Source == Source.Workflow)
            return new Empty();

        // SA state
        await _salesArrangementStateManager.SetSalesArrangementStateAccordingDocumentsOnSa(salesArrangement.SalesArrangementId, cancellationToken);

        if (!wasUpdateOfCustomersSuccessful)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UnsuccessfulCustomerDataUpdateToCM);

        return new Empty();
    }

    private static bool IsDocumentTypeWithHousehold(int documentTypeId) =>
                   documentTypeId == DocumentTypes.ZADOSTHU.ToByte() || // 4
                   documentTypeId == DocumentTypes.ZADOSTHD.ToByte() || // 5
                   documentTypeId == DocumentTypes.ZUSTAVSI.ToByte() || // 11
                   documentTypeId == DocumentTypes.PRISTOUP.ToByte() || // 12
                   documentTypeId == DocumentTypes.ZADOSTHD_SERVICE.ToByte();// 16

    private void LogAuditMessage(DocumentOnSa documentOnSa, SalesArrangement salesArrangement)
    {
        _auditLogger.Log(
            AuditEventTypes.Noby007,
            "Dokument byl označen za podepsaný",
            products: new List<AuditLoggerHeaderItem>
            {
                new(AuditConstants.ProductNamesCase, salesArrangement.CaseId),
                new(AuditConstants.ProductNamesSalesArrangement, salesArrangement.SalesArrangementId),
                new(AuditConstants.ProductNamesForm, documentOnSa.FormId)
            }
        );
    }

    private async Task<bool> ProcessCrsDocumentOnSa(DocumentOnSa documentOnSa, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var customerOnSa = CustomersOnSABuffer.Find(c => c.CustomerOnSAId == documentOnSa.CustomerOnSAId1!.Value)
                                    ?? await _customerOnSAService.GetCustomer(documentOnSa.CustomerOnSAId1!.Value, cancellationToken);

        var customerChangeMetadata = await GetCustomerOnSaMetadata(documentOnSa, customerOnSa, cancellationToken);

        if (customerChangeMetadata.CustomerChangeMetadata.WasCRSChanged)
        {
            try
            {
                var customerDetail = await _customerService.GetCustomerDetail(customerOnSa.CustomerIdentifiers.GetKbIdentity(), cancellationToken);
                _customerChangeDataMerger.MergeTaxResidence(customerDetail.NaturalPerson!, customerOnSa);
                var updateCustomerRequest = MapUpdateCustomerRequest((int)SharedTypes.Enums.Mandants.Kb, customerDetail);
                await _customerService.UpdateCustomer(updateCustomerRequest, cancellationToken);

                // Throw away locally stored CRS data (keep client changes) 
                var customerChangeData = customerOnSa.GetCustomerChangeDataObject();
                customerChangeData!.NaturalPerson!.TaxResidences = null;
                customerOnSa.CustomerChangeMetadata.WasCRSChanged = false;

                await _customerOnSAService.UpdateCustomerDetail(MapUpdateCustomerOnSaRequest(customerOnSa, customerOnSa.CustomerChangeMetadata, customerChangeData), cancellationToken);
            }
            catch (Exception exp) when (!string.IsNullOrWhiteSpace(customerOnSa.CustomerChangeData))
            {
                _logger.UpdateCustomerFailed(customerOnSa.CustomerOnSAId, exp);
                await CreateWfTask(customerOnSa, salesArrangement, exp.Message, cancellationToken);
                return false;
            }
        }

        return true;
    }

    private async Task<bool> ProcessDocumentOnSaWithHousehold(SalesArrangement salesArrangement, Household household, DocumentOnSa documentOnSa, CancellationToken cancellationToken)
    {
        var mandantId = await GetMandantId(salesArrangement, cancellationToken);

        if (mandantId != (int)SharedTypes.Enums.Mandants.Kb)
        {
            throw new CisValidationException(90002, $"Mp products not supported (mandant {mandantId})");
        }

        var errorsOfCustomerUpdate = new List<bool>();
        var customersOnSa = await GetCustomersOnSa(household, cancellationToken);
        foreach (var customerOnSa in customersOnSa)
        {
            var customerChangeMetadata = await GetCustomerOnSaMetadata(documentOnSa, customerOnSa, cancellationToken);
            if (customerChangeMetadata.CustomerChangeMetadata.WereClientDataChanged)
            {
                try
                {
                    var customerDetail = await _customerService.GetCustomerDetail(customerOnSa.CustomerIdentifiers.GetKbIdentity(), cancellationToken);
                    _customerChangeDataMerger.MergeClientData(customerDetail, customerOnSa);

                    //Do not update TaxResidence (event do not send TaxResidence already set in CM)
                    customerDetail.NaturalPerson.TaxResidence = null;

                    var updateCustomerRequest = MapUpdateCustomerRequest(mandantId.Value, customerDetail);
                    await _customerService.UpdateCustomer(updateCustomerRequest, cancellationToken);
                    //Throw away locally stored Client data (keep CRS changes) 
                    var customerChangeData = customerOnSa.GetCustomerChangeDataObject()?.NaturalPerson?.TaxResidences is null ? null : new CustomerChangeData
                    {
                        NaturalPerson = new NaturalPersonDelta
                        {
                            TaxResidences = customerOnSa.GetCustomerChangeDataObject()!.NaturalPerson!.TaxResidences
                        }
                    };

                    customerOnSa.CustomerChangeMetadata.WereClientDataChanged = false;
                    await _customerOnSAService.UpdateCustomerDetail(MapUpdateCustomerOnSaRequest(customerOnSa, customerOnSa.CustomerChangeMetadata, customerChangeData), cancellationToken);
                }
                catch (Exception exp) when (!string.IsNullOrWhiteSpace(customerOnSa.CustomerChangeData))
                {
                    _logger.UpdateCustomerFailed(customerOnSa.CustomerOnSAId, exp);
                    await CreateWfTask(customerOnSa, salesArrangement, exp.Message, cancellationToken);
                    errorsOfCustomerUpdate.Add(true);
                }
            }
        }

        return errorsOfCustomerUpdate.Count == 0;
    }

    private async Task CreateWfTask(CustomerOnSA customerOnSa, SalesArrangement salesArrangement, string message, CancellationToken cancellationToken)
    {
        dynamic parsedJson = JsonConvert.DeserializeObject(customerOnSa.CustomerChangeData)!;
        var formattedCustomerChangedData = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

        var customerChangeDataAsByteArray = Encoding.UTF8.GetBytes(formattedCustomerChangedData + Environment.NewLine + Environment.NewLine + message);

        var documentId = await _documentArchiveService.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken);
        var eaCodeMain = (await _codebookService.EaCodesMain(cancellationToken)).Single(e => e.Id == _unwrittenDataEaCodeMainId);
        var contractNumber = await GetContractNumber(salesArrangement.CaseId, cancellationToken);
        var user = await _userService.GetUser(_currentUser.User!.Id, cancellationToken);
        var authorUserLogin = GetAuthorUserLoginForDocumentUpload(user);

        var uploadDocRequest = new UploadDocumentRequest
        {
            BinaryData = ByteString.CopyFrom(customerChangeDataAsByteArray),
            Metadata = new()
            {
                CaseId = salesArrangement.CaseId,
                DocumentId = documentId,
                EaCodeMainId = _unwrittenDataEaCodeMainId,
                Filename = $"{eaCodeMain.Name}.txt",
                AuthorUserLogin = authorUserLogin,
                CreatedOn = _dateTime.GetLocalNow().DateTime,
                Description = string.Empty,
                FormId = string.Empty,
                ContractNumber = contractNumber
            }
        };

        await _documentArchiveService.UploadDocument(uploadDocRequest, cancellationToken);

        // ProcessTypeId == 1 (Hlavní úvěrový proces)
        var process = (await _caseService.GetProcessList(salesArrangement.CaseId, cancellationToken)).Single(r => r.ProcessTypeId == 1);

        var createTaskRequest = new CreateTaskRequest
        {
            CaseId = salesArrangement.CaseId,
            ProcessId = process.ProcessId,
            TaskTypeId = 3, // Konzultace
            TaskSubtypeId = 0, // Dotaz (Obecný)
            TaskRequest = "Při propisu dat do CURE (CustomerManagementu) nastala chyba, blíže viz příloha. Upravte prosím, nebo doplňte údaje na základě přílohy přímo v aplikaci CURE.",
            TaskDocumentsId = { documentId }
        };

        await _caseService.CreateTask(createTaskRequest, cancellationToken);
    }

    private async Task<string> GetContractNumber(long caseId, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(caseId, cancellationToken);
        if (string.IsNullOrWhiteSpace(caseDetail?.Data?.ContractNumber))
            return _defaultContractNumber;

        return caseDetail.Data.ContractNumber;
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
            customers.Add(CustomersOnSABuffer.Find(c => c.CustomerOnSAId == household.CustomerOnSAId1.Value)
                                    ?? await _customerOnSAService.GetCustomer(household.CustomerOnSAId1.Value, cancellationToken));
        }
        if (household.CustomerOnSAId2 is not null)
        {
            customers.Add(CustomersOnSABuffer.Find(c => c.CustomerOnSAId == household.CustomerOnSAId2.Value)
                                    ?? await _customerOnSAService.GetCustomer(household.CustomerOnSAId2.Value, cancellationToken));
        }

        return customers;
    }

    private static UpdateCustomerDetailRequest MapUpdateCustomerOnSaRequest(CustomerOnSA customerOnSa, CustomerChangeMetadata customerChangeMetadata, CustomerChangeData? changeData)
    {
        var updateRequest = new UpdateCustomerDetailRequest
        {
            CustomerOnSAId = customerOnSa.CustomerOnSAId,
            CustomerAdditionalData = customerOnSa.CustomerAdditionalData,
            CustomerChangeMetadata = customerChangeMetadata
        };

        updateRequest.UpdateCustomerChangeDataObject(changeData);

        return updateRequest;
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
            Mandant = (SharedTypes.GrpcTypes.Mandants)mandantId,
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

        await _salesArrangementService.SetFlowSwitch(houseHold.SalesArrangementId, flowSwitchId, false, cancellationToken);
    }

    private async Task UpdateFirstSignatureDateSaParams(DateTime signatureDate, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        salesArrangement.Mortgage.FirstSignatureDate = signatureDate;
        await _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Mortgage = salesArrangement.Mortgage
        },
            cancellationToken);
    }

    private async Task SumlCall(Household houseHold, CancellationToken cancellationToken)
    {
        await SumlCallForSpecifiedCustomer(houseHold.CustomerOnSAId1!.Value, cancellationToken);

        if (houseHold.CustomerOnSAId2 is not null)
        {
            await SumlCallForSpecifiedCustomer(houseHold.CustomerOnSAId2.Value, cancellationToken);
        }
    }

    private async Task SumlCallForSpecifiedCustomer(int customerOnSaId, CancellationToken cancellationToken)
    {
        var customerOnSa = await _customerOnSAService.GetCustomer(customerOnSaId, cancellationToken);
        CustomersOnSABuffer.Add(customerOnSa);

        var kbIdentity = customerOnSa.CustomerIdentifiers.GetKbIdentity();
        await _sulmClientHelper.StartUse(kbIdentity.IdentityId, ISulmClient.PurposeMLAP, cancellationToken);
    }

    private async Task AddFirstSignatureDateIfNotSetYet(DocumentOnSa documentOnSa, SalesArrangement salesArrangement, DateTime signatureDate, CancellationToken cancellationToken)
    {
        var salesArrangementType = await _commonSigningMethods.GetSalesArrangementType(salesArrangement, cancellationToken);
        if (salesArrangementType.SalesArrangementCategory == SalesArrangementCategories.ServiceRequest.ToByte()
            && salesArrangement.FirstSignatureDate is null)
        {
            //SalesArrangement FirstSignatureDate
            await UpdateSalesArrangementFirstSignatureDate(salesArrangement, signatureDate, cancellationToken);
        }

        if (documentOnSa.DocumentTypeId.GetValueOrDefault() == DocumentTypes.ZADOSTHU.ToByte()
        && await _dbContext.DocumentOnSa.Where(d => d.SalesArrangementId == documentOnSa.SalesArrangementId && d.DocumentTypeId == DocumentTypes.ZADOSTHU.ToByte()).AllAsync(r => !r.IsSigned, cancellationToken))
        {
            var result = await _easClient.AddFirstSignatureDate((int)salesArrangement.CaseId, signatureDate, cancellationToken);

            if (result is not null && result.CommonValue != 0)
            {
                throw new CisValidationException(ErrorCodeMapper.EasAddFirstSignatureDateReturnedErrorState, $"Eas AddFirstSignatureDate returned error state: {result.CommonValue} with message: {result.CommonText}");
            }

            //SalesArrangement FirstSignatureDate
            await UpdateSalesArrangementFirstSignatureDate(salesArrangement, signatureDate, cancellationToken);

            //SalesArrangement parameters FirstSignatureDate
            await UpdateFirstSignatureDateSaParams(signatureDate, salesArrangement, cancellationToken);

            //KonsDb 
            await _productService.UpdateMortgage(salesArrangement.CaseId, cancellationToken);
        }
    }

    private async Task UpdateSalesArrangementFirstSignatureDate(SalesArrangement salesArrangement, DateTime signatureDate, CancellationToken cancellationToken)
    {
        await _salesArrangementService.UpdateSalesArrangement(new()
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            FirstSignatureDate = Timestamp.FromDateTime(DateTime.SpecifyKind(signatureDate, DateTimeKind.Utc))
        }
        , cancellationToken);
    }

    private void UpdateDocumentOnSa(DocumentOnSa documentOnSa, System.DateTime signatureDate)
    {
        documentOnSa.IsSigned = true;
        documentOnSa.SignatureDateTime = signatureDate;
        documentOnSa.SignatureConfirmedBy = _currentUser.User?.Id;
    }

    private string GetAuthorUserLoginForDocumentUpload(User user)
    {
        if (!string.IsNullOrWhiteSpace(user.UserInfo.Icp))
            return user.UserInfo.Icp;
        else if (!string.IsNullOrWhiteSpace(user.UserInfo.Cpm))
            return user.UserInfo.Cpm;
        else if (_currentUser.User?.Id is not null)
            return _currentUser.User!.Id.ToString(CultureInfo.InvariantCulture);
        else
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.CannotGetNobyUserIdentifier);
    }
}
