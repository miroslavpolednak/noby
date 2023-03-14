using CIS.Core.Attributes;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CaseService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.Eas.V1;
using ExternalServices.Eas.V1.CheckFormV2;
using ExternalServices.Eas.V1.EasWrapper;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
public class EasFormsManager
{
    private readonly IMediator _mediator;
    private readonly IEasClient _easClient;
    private readonly ICaseServiceClient _caseService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public EasFormsManager(IMediator mediator,
                           IEasClient easClient,
                           ICaseServiceClient caseService,
                           IHouseholdServiceClient householdService,
                           ICustomerOnSAServiceClient customerOnSAService)
    {
        _mediator = mediator;
        _easClient = easClient;
        _caseService = caseService;
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
    }

    public static int GetFormId(EasFormType type)
    {
        return type switch
        {
            EasFormType.F3601 => 3601001,
            EasFormType.F3602 => 3602001,
            EasFormType.F3700 => 3700001,
            _ => 0
        };
    }

    public Task<Response> CheckForms(CheckFormData checkFormData, CancellationToken cancellationToken) =>
        _easClient.CheckFormV2(checkFormData, cancellationToken);

    public async Task UpdateContractNumberIfNeeded(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(salesArrangement.ContractNumber))
            return;

        var households = await _householdService.GetHouseholdList(salesArrangement.SalesArrangementId, cancellationToken);
        var mainHousehold = households.Single(h => h.HouseholdTypeId == (int)HouseholdTypes.Main);

        var customersOnSa = await _customerOnSAService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);
        var mainCustomerOnSa = customersOnSa.Single(c => c.CustomerOnSAId == mainHousehold.CustomerOnSAId1!.Value);

        var identityMp = mainCustomerOnSa.CustomerIdentifiers.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp);

        if (identityMp is null)
            throw new InvalidOperationException($"CustomerOnSa {mainCustomerOnSa.CustomerOnSAId} does not have MP ID");

        var contractNumber = await _easClient.GetContractNumber(identityMp.IdentityId, (int)salesArrangement.CaseId, cancellationToken);

        await UpdateSalesArrangement(salesArrangement, contractNumber, cancellationToken);
        await UpdateCase(salesArrangement.CaseId, contractNumber, cancellationToken);

    }

    private async Task UpdateSalesArrangement(SalesArrangement salesArrangement, string contractNumber, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateSalesArrangementRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            ContractNumber = contractNumber,
            RiskBusinessCaseId = salesArrangement.RiskBusinessCaseId,
            SalesArrangementSignatureTypeId = salesArrangement.SalesArrangementSignatureTypeId,
            FirstSignedDate = salesArrangement.FirstSignedDate
        }, cancellationToken);

        salesArrangement.ContractNumber = contractNumber;
    }

    private async Task UpdateCase(long caseId, string contractNumber, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(caseId, cancellationToken);

        caseDetail.Data.ContractNumber = contractNumber;

        await _caseService.UpdateCaseData(caseId, caseDetail.Data, cancellationToken);
    }
}