using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.VersionData;

[TransientService, SelfService]
internal class CustomerChange3602VersionDataProvider : DocumentVersionDataProviderBase
{
    private readonly IHouseholdServiceClient _householdService;

    public CustomerChange3602VersionDataProvider(IDocumentVersionDataProvider documentVersionDataProvider, ICodebookServiceClient codebookService, IHouseholdServiceClient householdService) 
        : base(documentVersionDataProvider, codebookService)
    {
        _householdService = householdService;
    }

    protected override async Task<string> LoadVariantName(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        request.InputParameters.ValidateSalesArrangementId();

        var salesArrangementId = request.InputParameters.SalesArrangementId!.Value;

        var numberOfDebtors = await LoadNumberOfDebtors(salesArrangementId, cancellationToken);

        return numberOfDebtors switch
        {
            1 => "A",
            2 => "B",
            _ => throw new NotImplementedException()
        };
    }

    private async Task<int> LoadNumberOfDebtors(int salesArrangementId, CancellationToken cancellationToken)
    {
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);

        var household = households.FirstOrDefault(h => h.HouseholdTypeId == (int)HouseholdTypes.Codebtor)
                        ?? throw new CisValidationException($"Household with the type HouseholdTypes.Codebtor was not found");

        return household is { CustomerOnSAId1: not null, CustomerOnSAId2: not null } ? 2 : 1;
    }
}