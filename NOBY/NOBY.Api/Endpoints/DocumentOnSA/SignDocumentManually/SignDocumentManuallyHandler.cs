using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using FastEnumUtility;
using static DomainServices.HouseholdService.Contracts.GetCustomerChangeMetadataResponse.Types;
using _CustomerService = DomainServices.CustomerService.Contracts;

namespace NOBY.Api.Endpoints.DocumentOnSA.SignDocumentManually;

internal sealed class SignDocumentManuallyHandler : IRequestHandler<SignDocumentManuallyRequest>
{
    private readonly IDocumentOnSAServiceClient _documentOnSaClient;

    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly ICodebookServiceClient _codebookServiceClients;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly ICustomerOnSAServiceClient _customerOnSAServiceClient;
    private readonly ICustomerServiceClient _customerServiceClient;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly ICustomerChangeDataMerger _customerChangeDataMerger;

    public SignDocumentManuallyHandler(
            IDocumentOnSAServiceClient documentOnSaClient,
            ISalesArrangementServiceClient arrangementServiceClient,
            ICodebookServiceClient codebookServiceClients,
            IHouseholdServiceClient householdClient,
            ICustomerOnSAServiceClient customerOnSAServiceClient,
            ICustomerServiceClient customerServiceClient,
            ICaseServiceClient caseServiceClient,
            ICustomerChangeDataMerger customerChangeDataMerger)
    {
        _documentOnSaClient = documentOnSaClient;
        _arrangementServiceClient = arrangementServiceClient;
        _codebookServiceClients = codebookServiceClients;
        _householdClient = householdClient;
        _customerOnSAServiceClient = customerOnSAServiceClient;
        _customerServiceClient = customerServiceClient;
        _caseServiceClient = caseServiceClient;
        _customerChangeDataMerger = customerChangeDataMerger;
    }

    public async Task Handle(SignDocumentManuallyRequest request, CancellationToken cancellationToken)
    {
        var documentOnSas = await _documentOnSaClient.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        if (!documentOnSas.DocumentsOnSAToSign.Any(d => d.DocumentOnSAId == request.DocumentOnSAId))
        {
            throw new NobyValidationException($"DocumetnOnSa {request.DocumentOnSAId} not exist for SalesArrangement {request.SalesArrangementId}");
        }

        var documentOnSa = documentOnSas.DocumentsOnSAToSign.Single(r => r.DocumentOnSAId == request.DocumentOnSAId);

        await _documentOnSaClient.SignDocument(request.DocumentOnSAId, SignatureTypes.Paper.ToByte(), cancellationToken);

        if (documentOnSa.HouseholdId is null)
        {
            // CRS
            if (documentOnSa.DocumentTypeId == DocumentTypes.DANRESID.ToByte()) // 13
            {
                await ProcessCrsDocumentOnSa(documentOnSa, cancellationToken);
            }
        }
        else
        {
            await ProcessDocumentOnSaWithHousehold(request, documentOnSa, cancellationToken);
        }
    }

    private async Task ProcessCrsDocumentOnSa(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var customerOnSa = await _customerOnSAServiceClient.GetCustomer(documentOnSa.CustomerOnSAId!.Value, cancellationToken);

        var customerChangeMetadata = await GetCustomerOnSaMetadata(documentOnSa, customerOnSa, cancellationToken);

        if (customerChangeMetadata.CustomerChangeMetadata.WasCRSChanged)
        {
            var customerDetail = await _customerServiceClient.GetCustomerDetail(customerOnSa.CustomerIdentifiers.First(r => r.IdentityScheme == Identity.Types.IdentitySchemes.Kb), cancellationToken);
            _customerChangeDataMerger.MergeTaxResidence(customerDetail?.NaturalPerson!, customerOnSa);
            var updateCustomerRequest = MapUpdateCustomerRequest((int)CIS.Foms.Enums.Mandants.Kb, customerDetail!);
            await _customerServiceClient.UpdateCustomer(updateCustomerRequest, cancellationToken);
            // Throw away locally stored CRS data (keep client changes) 
            var jsonCustomerChangeDataWithoutCrs = _customerChangeDataMerger.TrowAwayLocallyStoredCrsData(customerOnSa);
            await _customerOnSAServiceClient.UpdateCustomerDetail(MapUpdateCustomerOnSaRequest(customerOnSa, jsonCustomerChangeDataWithoutCrs), cancellationToken);
        }
    }


    private async Task ProcessDocumentOnSaWithHousehold(SignDocumentManuallyRequest request, DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var mandantId = await GetMandantId(request, cancellationToken);

        if (mandantId != (int)CIS.Foms.Enums.Mandants.Kb)
        {
            throw new CisValidationException(90002, $"Mp products not supported (mandant {mandantId})");
        }

        var (household, customersOnSa) = await GetCustomersOnSa(documentOnSa, cancellationToken);
        foreach (var customerOnSa in customersOnSa)
        {
            var customerChangeMetadata = await GetCustomerOnSaMetadata(documentOnSa, customerOnSa, cancellationToken);
            if (customerChangeMetadata.CustomerChangeMetadata.WereClientDataChanged)
            {
                var customerDetail = await _customerServiceClient.GetCustomerDetail(customerOnSa.CustomerIdentifiers.First(r => r.IdentityScheme == Identity.Types.IdentitySchemes.Kb), cancellationToken);
                _customerChangeDataMerger.MergeClientData(customerDetail, customerOnSa);
                var updateCustomerRequest = MapUpdateCustomerRequest(mandantId.Value, customerDetail);
                await _customerServiceClient.UpdateCustomer(updateCustomerRequest, cancellationToken);
                //Throw away locally stored Client data (keep CRS changes) 
                var jsonCustomerChangeDataWithCrs = _customerChangeDataMerger.TrowAwayLocallyStoredClientData(customerOnSa);
                await _customerOnSAServiceClient.UpdateCustomerDetail(MapUpdateCustomerOnSaRequest(customerOnSa, jsonCustomerChangeDataWithCrs), cancellationToken);
            }
        }

        // HFICH-4165
        int flowSwitchId = household.HouseholdTypeId switch
        {
            (int)HouseholdTypes.Main => (int)FlowSwitches.Was3601MainChangedAfterSigning,
            (int)HouseholdTypes.Codebtor => (int)FlowSwitches.Was3602CodebtorChangedAfterSigning,
            _ => throw new NobyValidationException("Unsupported HouseholdType")
        };

        await _arrangementServiceClient.SetFlowSwitches(household.SalesArrangementId, new()
                    {
                        new() { FlowSwitchId = flowSwitchId, Value = false }
                    }, cancellationToken);
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

    private async Task<int?> GetMandantId(SignDocumentManuallyRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        if (salesArrangement is null)
        {
            throw new CisNotFoundException(19000, $"SalesArrangement{request.SalesArrangementId} does not exist.");
        }

        var caseDetail = await _caseServiceClient.GetCaseDetail(salesArrangement.CaseId, cancellationToken);
        var productTypes = await _codebookServiceClients.ProductTypes(cancellationToken);
        return productTypes.Single(r => r.Id == caseDetail.Data.ProductTypeId).MandantId;
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

    private async Task<(DomainServices.HouseholdService.Contracts.Household Household, List<CustomerOnSA> Customers)> GetCustomersOnSa(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var houseHold = await _householdClient.GetHousehold(documentOnSa.HouseholdId!.Value, cancellationToken);

        var customers = new List<CustomerOnSA>();
        if (houseHold.CustomerOnSAId1 is not null)
        {
            customers.Add(await _customerOnSAServiceClient.GetCustomer(houseHold.CustomerOnSAId1.Value, cancellationToken));
        }
        if (houseHold.CustomerOnSAId2 is not null)
        {
            customers.Add(await _customerOnSAServiceClient.GetCustomer(houseHold.CustomerOnSAId2.Value, cancellationToken));
        }

        return (houseHold, customers);
    }

    private async Task<GetCustomerChangeMetadataResponseItem> GetCustomerOnSaMetadata(DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign documentOnSa, CustomerOnSA customerOnSa, CancellationToken cancellationToken)
    {
        var customersChangeMetadata = await _customerOnSAServiceClient.GetCustomerChangeMetadata(documentOnSa.SalesArrangementId, cancellationToken);
        return customersChangeMetadata!.Single(r => r.CustomerOnSAId == customerOnSa.CustomerOnSAId);
    }
}
