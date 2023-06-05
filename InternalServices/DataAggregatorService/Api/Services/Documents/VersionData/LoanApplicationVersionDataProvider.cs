using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients;
using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.VersionData;

[TransientService, SelfService]
internal sealed class LoanApplicationVersionDataProvider : DocumentVersionDataProviderBase
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly IUserServiceClient _userService;

    public LoanApplicationVersionDataProvider(IDocumentVersionDataProvider documentVersionDataProvider,
                                              ICodebookServiceClient codebookService,
                                              ISalesArrangementServiceClient salesArrangementService,
                                              IHouseholdServiceClient householdService,
                                              IUserServiceClient userService)
        : base(documentVersionDataProvider, codebookService)
    {
        _salesArrangementService = salesArrangementService;
        _householdService = householdService;
        _userService = userService;
    }

    protected override async Task<string> LoadVariantName(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        request.InputParameters.ValidateSalesArrangementId();

        var salesArrangementId = request.InputParameters.SalesArrangementId!.Value;

        var numberOfDebtorsTask = LoadNumberOfDebtors(salesArrangementId, (DocumentType)request.DocumentTypeId, cancellationToken);
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

    private async Task<int> LoadNumberOfDebtors(int salesArrangementId, DocumentType documentType, CancellationToken cancellationToken)
    {
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);

        var household = households.FirstOrDefault(h => h.HouseholdTypeId == (int)GetHouseholdType(documentType))
                        ?? throw new CisValidationException($"Household with type {GetHouseholdType(documentType)} was not found");

        return household is { CustomerOnSAId1: not null, CustomerOnSAId2: not null } ? 2 : 1;
    }

    private static HouseholdTypes GetHouseholdType(DocumentType documentType)
    {
        return documentType switch
        {
            DocumentType.ZADOSTHU => HouseholdTypes.Main,
            DocumentType.ZADOSTHD or DocumentType.ZADOSTHD_SERVICE => HouseholdTypes.Codebtor,
            _ => throw new ArgumentOutOfRangeException(nameof(documentType), documentType, null)
        };
    }
}