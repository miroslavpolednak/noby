﻿using System.Runtime.CompilerServices;
using CIS.Core.Attributes;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.HouseholdService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal sealed class FormsService
{
    private readonly IMediator _mediator;
    private readonly IDataAggregatorServiceClient _dataAggregatorService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICodebookServiceClient _codebookService;

    public FormsService(IMediator mediator,
                        IDataAggregatorServiceClient dataAggregatorService,
                        IHouseholdServiceClient householdService,
                        ICodebookServiceClient codebookService)
    {
        _mediator = mediator;
        _dataAggregatorService = dataAggregatorService;
        _householdService = householdService;
        _codebookService = codebookService;
    }

    public Task<SalesArrangement> LoadSalesArrangement(int salesArrangementId, CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetSalesArrangementRequest { SalesArrangementId = salesArrangementId }, cancellationToken);
    }

    public async Task<SalesArrangementTypesResponse.Types.SalesArrangementTypeItem> LoadSalesArrangementType(int salesArrangementTypeId, CancellationToken cancellationToken)
    {
        var types = await _codebookService.SalesArrangementTypes(cancellationToken);

        return types.First(t => t.Id == salesArrangementTypeId);
    }

    public async IAsyncEnumerable<DynamicFormValues> CreateProductDynamicFormValues(SalesArrangement salesArrangement, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var householdTypes = await _codebookService.HouseholdTypes(cancellationToken);
        var houseHolds = await _householdService.GetHouseholdList(salesArrangement.SalesArrangementId, cancellationToken);

        foreach (var household in houseHolds)
        {
            yield return new DynamicFormValues
            {
                HouseholdId = household.HouseholdId,
                DocumentTypeId = householdTypes.Single(r => r.Id == household.HouseholdTypeId).DocumentTypeId!.Value
            };
        }
    }

    public async Task<DynamicFormValues> CreateServiceDynamicFormValues(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);

        return new DynamicFormValues
        {
            DocumentTypeId = documentTypes.Single(d => d.SalesArrangementTypeId == salesArrangement.SalesArrangementTypeId).Id
        };
    }

    public Task<GetEasFormResponse> LoadServiceForm(SalesArrangement salesArrangement, IEnumerable<DynamicFormValues> dynamicFormValues, CancellationToken cancellationToken)
    {
        return _dataAggregatorService.GetEasForm(new GetEasFormRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            UserId = salesArrangement.Created.UserId!.Value,
            EasFormRequestType = EasFormRequestType.Service,
            DynamicFormValues = { dynamicFormValues }
        }, cancellationToken);
    }

    public async Task<GetEasFormResponse> LoadProductForm(SalesArrangement salesArrangement, IEnumerable<DynamicFormValues> dynamicFormValues, bool isCancelled, CancellationToken cancellationToken)
    {
        FormValidations.CheckArrangement(salesArrangement);

        var response = await _dataAggregatorService.GetEasForm(new GetEasFormRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            UserId = salesArrangement.Created.UserId!.Value,
            EasFormRequestType = EasFormRequestType.Product,
            DynamicFormValues = { dynamicFormValues },
            IsCancelled = isCancelled
        }, cancellationToken);

        FormValidations.CheckFormData(response.Product);

        return response;
    }
}