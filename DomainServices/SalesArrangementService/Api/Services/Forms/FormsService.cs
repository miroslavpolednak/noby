using CIS.Core.Attributes;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.Eas.V1;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal sealed class FormsService
{
    private readonly IMediator _mediator;
    private readonly IDataAggregatorServiceClient _dataAggregator;
    private readonly ICodebookServiceClients _codebookService;
    private readonly ICaseServiceClient _caseService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSaService;
    private readonly IEasClient _easClient;
    private readonly SulmService.ISulmClient _sulmClient;

    public FormsService(IMediator mediator,
                        IDataAggregatorServiceClient dataAggregator,
                        ICodebookServiceClients codebookService,
                        ICaseServiceClient caseService,
                        IHouseholdServiceClient householdService,
                        ICustomerOnSAServiceClient customerOnSaService,
                        IEasClient easClient,
                        SulmService.ISulmClient sulmClient)
    {
        _mediator = mediator;
        _dataAggregator = dataAggregator;
        _codebookService = codebookService;
        _caseService = caseService;
        _householdService = householdService;
        _customerOnSaService = customerOnSaService;
        _easClient = easClient;
        _sulmClient = sulmClient;
    }

    public Task<SalesArrangement> LoadSalesArrangement(int salesArrangementId, CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetSalesArrangementRequest { SalesArrangementId = salesArrangementId }, cancellationToken);
    }

    public async Task<SalesArrangementCategories> LoadSalesArrangementCategory(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var types = await _codebookService.SalesArrangementTypes(cancellationToken);

        return (SalesArrangementCategories)types.First(t => t.Id == salesArrangement.SalesArrangementTypeId).SalesArrangementCategory;
    }

    public Task<GetEasFormResponse> LoadServiceForm(int salesArrangementId, IEnumerable<DynamicFormValues> dynamicFormValues, CancellationToken cancellationToken)
    {
        return _dataAggregator.GetEasForm(new GetEasFormRequest
        {
            SalesArrangementId = salesArrangementId,
            EasFormRequestType = EasFormRequestType.Service,
            DynamicFormValues = { dynamicFormValues }
        }, cancellationToken);
    }

    public async Task<GetEasFormResponse> LoadProductForm(SalesArrangement salesArrangement, IEnumerable<DynamicFormValues> dynamicFormValues, CancellationToken cancellationToken)
    {
        FormValidations.CheckArrangement(salesArrangement);

        var response = await _dataAggregator.GetEasForm(new GetEasFormRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            EasFormRequestType = EasFormRequestType.Product,
            DynamicFormValues = { dynamicFormValues }
        }, cancellationToken);

        FormValidations.CheckFormData(response.Product);

        return response;
    }

    public Task AddFirstSignatureDate(long caseId, CancellationToken cancellationToken)
    {
        return _easClient.AddFirstSignatureDate((int)caseId, DateTime.Now.Date, cancellationToken);
    }

    public async Task CallSulm(ProductData formData, CancellationToken cancellation)
    {
        var customersOnSa = formData.CustomersOnSa
                                    .Select(customer => customer.Identities.FirstOrDefault(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb))
                                    .Where(kbIdentity => kbIdentity is not null);

        // HFICH-2426
        foreach (var kbIdentity in customersOnSa)
        {
            await _sulmClient.StopUse(kbIdentity!.IdentityId, "MLAP", cancellation);
            await _sulmClient.StartUse(kbIdentity.IdentityId, "MLAP", cancellation);
        }
    }

    public async Task UpdateContractNumber(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(salesArrangement.ContractNumber))
            return;

        var households = await _householdService.GetHouseholdList(salesArrangement.SalesArrangementId, cancellationToken);
        var mainHousehold = households.Single(h => h.HouseholdTypeId == (int)HouseholdTypes.Main);

        var customersOnSa = await _customerOnSaService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);
        var mainCustomerOnSa = customersOnSa.Single(c => c.CustomerOnSAId == mainHousehold.CustomerOnSAId1!.Value);

        var identityMp = mainCustomerOnSa.CustomerIdentifiers.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp);

        //Probly better to throw exception
        if (identityMp is null)
            return;

        var contractNumber = await _easClient.GetContractNumber(identityMp.IdentityId, (int)salesArrangement.CaseId, cancellationToken);

        await UpdateSalesArrangement(salesArrangement, contractNumber, cancellationToken);
        await UpdateCase(salesArrangement.CaseId, contractNumber, cancellationToken);
    }

    private async Task UpdateSalesArrangement(SalesArrangement entity, string contractNumber, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateSalesArrangementRequest
                             {
                                 SalesArrangementId = entity.SalesArrangementId,
                                 ContractNumber = contractNumber,
                                 RiskBusinessCaseId = entity.RiskBusinessCaseId,
                                 FirstSignedDate = entity.FirstSignedDate,
                                 SalesArrangementSignatureTypeId = entity.SalesArrangementSignatureTypeId
                             },
                             cancellationToken);

        entity.ContractNumber = contractNumber;
    }

    private async Task UpdateCase(long caseId, string contractNumber, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(caseId, cancellationToken);

        caseDetail.Data.ContractNumber = contractNumber;

        await _caseService.UpdateCaseData(caseId, caseDetail.Data, cancellationToken);
    }
}