using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients;
using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.VersionData;

[TransientService, SelfService]
internal sealed class LoanApplicationVersionDataProvider : IDocumentVersionDataProvider
{
    private readonly IDocumentVersionDataProvider _documentVersionDataProvider;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly IUserServiceClient _userService;

    public LoanApplicationVersionDataProvider(IDocumentVersionDataProvider documentVersionDataProvider,
                                              ICodebookServiceClient codebookService,
                                              ISalesArrangementServiceClient salesArrangementService, 
                                              IHouseholdServiceClient householdService, 
                                              IUserServiceClient userService)
    {
        _documentVersionDataProvider = documentVersionDataProvider;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _householdService = householdService;
        _userService = userService;
    }

    public async Task<DocumentVersionData> GetDocumentVersionData(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        var versionData = await _documentVersionDataProvider.GetDocumentVersionData(request, cancellationToken);

        var variants = await _codebookService.DocumentTemplateVariants(cancellationToken);

        DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem variant;

        if (request.DocumentTemplateVariantId.HasValue)
        {
            VersionVariantHelper.CheckIfDocumentVariantExists(variants, versionData.VersionId, request.DocumentTemplateVariantId.Value);

            variant = variants.First(v => v.Id == request.DocumentTemplateVariantId.Value && v.DocumentTemplateVersionId == versionData.VersionId);
        }
        else
        {
            var variantName = await LoadVariantName(request, cancellationToken);
            variant = VersionVariantHelper.GetDocumentVariant(variants, versionData.VersionId, variantName);
        }

        return versionData with { VariantId = variant.Id, VariantName = variant.DocumentVariant };
    }

    private async Task<string> LoadVariantName(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        request.InputParameters.ValidateSalesArrangementId();

        var salesArrangementId = request.InputParameters.SalesArrangementId!.Value;

        var numberOfDebtorsTask = LoadNumberOfDebtors(salesArrangementId, request.DocumentTypeId, cancellationToken);
        var isCreatorBrokerTask = IsCreatorBroker(salesArrangementId, cancellationToken);

        await Task.WhenAll(numberOfDebtorsTask, isCreatorBrokerTask);

        return numberOfDebtorsTask.Result switch
        {
            1 when isCreatorBrokerTask.Result => "A",
            1 => "B",
            2 when isCreatorBrokerTask.Result => "C",
            2 => "D",
            _ => throw new NotImplementedException()
        };
    }

    private async Task<bool> IsCreatorBroker(int salesArrangementId, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);

        User? user = null;

        if (salesArrangement.Created.UserId.HasValue)
            user = await _userService.GetUser(salesArrangement.Created.UserId.Value, cancellationToken);

        return user?.UserIdentifiers.Any(i => i.IdentityScheme == UserIdentity.Types.UserIdentitySchemes.BrokerId) ?? false;
    }

    private async Task<int> LoadNumberOfDebtors(int salesArrangementId, int documentTypeId, CancellationToken cancellationToken)
    {
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);

        var household = households.FirstOrDefault(h => h.HouseholdTypeId == (int)GetHouseholdType(documentTypeId))
                        ?? throw new CisValidationException($"Household with type {GetHouseholdType(documentTypeId)} was not found");

        if (household.CustomerOnSAId1.HasValue && household.CustomerOnSAId2.HasValue)
            return 2;

        return 1;
    }

    private static HouseholdTypes GetHouseholdType(int documentTypeId)
    {
        return documentTypeId switch
        {
            4 => HouseholdTypes.Main,
            5 => HouseholdTypes.Codebtor,
            _ => throw new ArgumentOutOfRangeException(nameof(documentTypeId), documentTypeId, null)
        };
    }
}