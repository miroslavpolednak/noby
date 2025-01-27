﻿using CIS.Core.Exceptions;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.VersionData;

[TransientService, SelfService]
internal sealed class LoanApplicationVersionDataProvider : DocumentVersionDataProviderBase
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly IUserServiceClient _userService;

    public LoanApplicationVersionDataProvider(IDocumentVersionDataProvider documentVersionDataProvider,
                                              ICodebookServiceClient codebookService,
                                              ISalesArrangementServiceClient salesArrangementService,
                                              ICaseServiceClient caseService,
                                              IHouseholdServiceClient householdService,
                                              IUserServiceClient userService)
        : base(documentVersionDataProvider, codebookService)
    {
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _householdService = householdService;
        _userService = userService;
    }

    protected override async Task<string> LoadVariantName(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        request.InputParameters.ValidateSalesArrangementId();

        var salesArrangementId = request.InputParameters.SalesArrangementId!.Value;

        var numberOfDebtorsTask = LoadNumberOfDebtors(salesArrangementId, (DocumentTypes)request.DocumentTypeId, cancellationToken);
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
        var saValidationResult = await _salesArrangementService.ValidateSalesArrangementId(salesArrangementId, true, cancellationToken);
        var caseValidationResult = await _caseService.ValidateCaseId(saValidationResult.CaseId!.Value, true, cancellationToken); 

        DomainServices.UserService.Clients.Dto.UserDto? user = null;

        if (caseValidationResult.OwnerUserId.HasValue)
            user = await _userService.GetUser(caseValidationResult.OwnerUserId.Value, cancellationToken);

        return user?.UserIdentifiers.Any(i => i.IdentityScheme == UserIdentity.Types.UserIdentitySchemes.BrokerId) ?? false;
    }

    private async Task<int> LoadNumberOfDebtors(int salesArrangementId, DocumentTypes documentType, CancellationToken cancellationToken)
    {
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);

        var household = households.FirstOrDefault(h => h.HouseholdTypeId == (int)GetHouseholdType(documentType))
                        ?? throw new CisValidationException($"Household with type {GetHouseholdType(documentType)} was not found");

        return household is { CustomerOnSAId1: not null, CustomerOnSAId2: not null } ? 2 : 1;
    }

    private static HouseholdTypes GetHouseholdType(DocumentTypes documentType)
    {
        return documentType switch
        {
            DocumentTypes.ZADOSTHU => HouseholdTypes.Main,
            DocumentTypes.ZADOSTHD or DocumentTypes.ZADOSTHD_SERVICE => HouseholdTypes.Codebtor,
            _ => throw new ArgumentOutOfRangeException(nameof(documentType), documentType, null)
        };
    }
}