using CIS.Core.Attributes;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal sealed class FormsService
{
    private readonly IMediator _mediator;
    private readonly IDataAggregatorServiceClient _dataAggregator;
    private readonly ICodebookServiceClients _codebookService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICaseServiceClient _caseService;
    private readonly IEasClient _easClient;

    public FormsService(IMediator mediator,
                        IDataAggregatorServiceClient dataAggregator,
                        ICodebookServiceClients codebookService,
                        IHouseholdServiceClient householdService,
                        ICustomerOnSAServiceClient customerOnSAService,
                        ICaseServiceClient caseService,
                        IEasClient easClient)
    {
        _mediator = mediator;
        _dataAggregator = dataAggregator;
        _codebookService = codebookService;
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
        _caseService = caseService;
        _easClient = easClient;
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