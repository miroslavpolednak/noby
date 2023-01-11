using CIS.Core.Attributes;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregator;
using CIS.InternalServices.DataAggregator.EasForms;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal sealed class FormsService
{
    private readonly IMediator _mediator;
    private readonly IDataAggregator _dataAggregator;
    private readonly ICodebookServiceClients _codebookService;
    private readonly ICaseServiceClient _caseService;
    private readonly Eas.IEasClient _easClient;
    private readonly SulmService.ISulmClient _sulmClient;

    public FormsService(IMediator mediator,
                        IDataAggregator dataAggregator,
                        ICodebookServiceClients codebookService,
                        ICaseServiceClient caseService,
                        Eas.IEasClient easClient,
                        SulmService.ISulmClient sulmClient)
    {
        _mediator = mediator;
        _dataAggregator = dataAggregator;
        _codebookService = codebookService;
        _caseService = caseService;
        _easClient = easClient;
        _sulmClient = sulmClient;
    }

    public async Task<SalesArrangementCategories> LoadSalesArrangementCategory(int salesArrangementId, CancellationToken cancellationToken)
    {
        var salesArrangement = await _mediator.Send(new GetSalesArrangementRequest { SalesArrangementId = salesArrangementId }, cancellationToken);

        var types = await _codebookService.SalesArrangementTypes(cancellationToken);

        return (SalesArrangementCategories)types.First(t => t.Id == salesArrangement.SalesArrangementTypeId).SalesArrangementCategory;
    }

    public async Task<IEasForm<IServiceFormData>> LoadServiceForm(int salesArrangementId)
    {
        var serviceForm = await _dataAggregator.GetEasForm<IServiceFormData>(salesArrangementId);

        return serviceForm;
    }

    public async Task<IEasForm<IProductFormData>> LoadProductForm(int salesArrangementId)
    {
        var productForm = await _dataAggregator.GetEasForm<IProductFormData>(salesArrangementId);

        FormValidations.CheckFormData(productForm.FormData);

        return productForm;
    }

    public async Task AddFirstSignatureDate(IProductFormData formData)
    {
        await _easClient.AddFirstSignatureDate((int)formData.SalesArrangement.CaseId, (int)formData.SalesArrangement.CaseId, DateTime.Now.Date);
    }

    public async Task CallSulm(IProductFormData formData, CancellationToken cancellation)
    {
        var customersOnSa = formData.HouseholdData
                                    .CustomersOnSa
                                    .Select(customer => customer.CustomerIdentifiers.FirstOrDefault(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb))
                                    .Where(kbIdentity => kbIdentity is not null);

        // HFICH-2426
        foreach (var kbIdentity in customersOnSa)
        {
            await _sulmClient.StopUse(kbIdentity.IdentityId, "MLAP", cancellation);
            await _sulmClient.StartUse(kbIdentity.IdentityId, "MLAP", cancellation);
        }
    }

    public async Task UpdateContractNumber(IProductFormData formData, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(formData.SalesArrangement.ContractNumber))
            return;

        var mainHousehold = formData.HouseholdData.Households.Single(i => i.HouseholdType == HouseholdTypes.Main);
        var mainCustomerOnSa = formData.HouseholdData.CustomersOnSa.Single(i => i.CustomerOnSAId == mainHousehold.CustomerOnSaId1!.Value);

        var identityMp = mainCustomerOnSa.CustomerIdentifiers.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp);
        var contractNumber = await _easClient.GetContractNumber(identityMp.IdentityId, (int)formData.SalesArrangement.CaseId);

        await UpdateSalesArrangement(formData.SalesArrangement, contractNumber, cancellationToken);
        await UpdateCase(formData.SalesArrangement.CaseId, contractNumber, cancellationToken);
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