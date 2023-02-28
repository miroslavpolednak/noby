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
    private readonly SulmService.ISulmClient _sulmClient;

    public FormsService(IMediator mediator,
                        IDataAggregatorServiceClient dataAggregator,
                        ICodebookServiceClients codebookService,
                        SulmService.ISulmClient sulmClient)
    {
        _mediator = mediator;
        _dataAggregator = dataAggregator;
        _codebookService = codebookService;
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
}