﻿using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ExtendedObjects;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

internal class AggregatedData
{
    protected readonly CodebookManager _codebookManager = new();

    public AggregatedData()
    {
        Custom = new CustomData(this);

        Customer = new CustomerDetailExtended();
        Customer.EnableCodebooks(_codebookManager);
    }

    public StaticValues StaticValues => StaticValues.Instance;

    public CustomData Custom { get; }

    public SalesArrangement SalesArrangement { get; set; } = null!;

    public Case Case { get; set; } = null!;

    public GetMortgageOfferDetailResponse Offer { get; set; } = null!;

    public GetMortgageOfferFPScheduleResponse OfferPaymentSchedule { get; set; } = null!;

    public UserInfo User { get; set; } = null!;

    public CustomerDetailExtended Customer { get; }

    public CustomerOnSA? CustomerOnSA { get; set; }

    public MortgageData Mortgage { get; set; } = null!;

    public List<HouseholdInfo> Households { get; } = new(2);

    public HouseholdInfo? HouseholdMain { get; set; }

    public HouseholdInfo? HouseholdCodebtor { get; set; }

    public Task LoadCodebooks(ICodebookServiceClient codebookService, CancellationToken cancellationToken)
    {
        ConfigureCodebooks(_codebookManager);

        return ((ICodebookManagerConfigurator)_codebookManager).Load(codebookService, cancellationToken);
    }

    public virtual Task LoadAdditionalData(CancellationToken cancellationToken) => Task.CompletedTask;

    protected virtual void ConfigureCodebooks(ICodebookManagerConfigurator configurator) { }
}