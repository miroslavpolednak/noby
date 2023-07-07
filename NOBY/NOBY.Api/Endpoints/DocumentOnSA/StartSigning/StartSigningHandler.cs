using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients;
using NOBY.Api.Endpoints.Customer;
using NOBY.Api.Endpoints.Customer.GetCustomerDetailWithChanges;
using _DocOnSA = DomainServices.DocumentOnSAService.Contracts;
namespace NOBY.Api.Endpoints.DocumentOnSA.StartSigning;

internal sealed class StartSigningHandler : IRequestHandler<StartSigningRequest, StartSigningResponse>
{
    private const string _signatureAnchorOne = "X_SIG_1";
    private const string _signatureAnchorTwo = "X_SIG_2";

    private readonly IDocumentOnSAServiceClient _client;
    private readonly ICodebookServiceClient _codebookServiceClient;
    private readonly ICustomerOnSAServiceClient _customerOnSAServiceClient;
    private readonly CustomerWithChangedDataService _changedDataService;

    public StartSigningHandler(
        IDocumentOnSAServiceClient client,
        ICodebookServiceClient codebookServiceClient,
        ICustomerOnSAServiceClient customerOnSAServiceClient,
        CustomerWithChangedDataService changedDataService)
    {
        _client = client;
        _codebookServiceClient = codebookServiceClient;
        _customerOnSAServiceClient = customerOnSAServiceClient;
        _changedDataService = changedDataService;
    }

    public async Task<StartSigningResponse> Handle(StartSigningRequest request, CancellationToken cancellationToken)
    {
        var result = await _client.StartSigning(new()
        {
            DocumentTypeId = request.DocumentTypeId,
            SalesArrangementId = request.SalesArrangementId,
            SignatureMethodCode = request.SignatureMethodCode,
            SignatureTypeId = request.SignatureTypeId,
            CustomerOnSAId1 = request.CustomerOnSAId1,
            CustomerOnSAId2 = request.CustomerOnSAId2,
            CustomerOnSAId1SigningIdentity = request.CustomerOnSAId1 is not null ? await MapCustomerOnSAIdentity(request.CustomerOnSAId1.Value, _signatureAnchorOne, cancellationToken) : null,
            CustomerOnSAId2SigningIdentity = request.CustomerOnSAId2 is not null ? await MapCustomerOnSAIdentity(request.CustomerOnSAId2.Value, _signatureAnchorTwo, cancellationToken) : null
        }, cancellationToken);

        return await MapToResponse(result, cancellationToken);
    }

    private async Task<_DocOnSA.SigningIdentity> MapCustomerOnSAIdentity(int customerOnSAId, string signatureAnchor, CancellationToken cancellationToken)
    {
        var customerOnSa = await _customerOnSAServiceClient.GetCustomer(customerOnSAId, cancellationToken);
        var (detailWithChangedData, _) = await _changedDataService.GetCustomerWithChangedData<GetCustomerDetailWithChangesResponse>(customerOnSa, cancellationToken);

        var signingIdentity = new _DocOnSA.SigningIdentity();

        // Service without household from SalesArrangement SalesArrangement object.Parameters.Applicant 
        // ToDo ask how map service request without household from SalesArrangement object.Parameters.Applicant

        // Product, CRS and Service with household mapping
        signingIdentity.CustomerIdentifiers.AddRange(customerOnSa.CustomerIdentifiers.Select(s => new CIS.Infrastructure.gRPC.CisTypes.Identity
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
        return signingIdentity;
    }

    private async Task<StartSigningResponse> MapToResponse(DomainServices.DocumentOnSAService.Contracts.StartSigningResponse result, CancellationToken cancellationToken)
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
            SignatureMethodCode = result.DocumentOnSa.SignatureMethodCode,
            SignatureTypeId = result.DocumentOnSa.SignatureTypeId,
            SignatureState = DocumentOnSaMetadataManager.GetSignatureState(new()
            {
                DocumentOnSAId = result.DocumentOnSa.DocumentOnSAId,
                EArchivId = result.DocumentOnSa.EArchivId,
                IsSigned = result.DocumentOnSa.IsSigned
            },
            signatureStates
            ),
            EACodeMainItem = DocumentOnSaMetadataManager.GetEaCodeMainItem(result.DocumentOnSa.DocumentTypeId.GetValueOrDefault(), documentTypes, eACodeMains)
        };
    }
}
