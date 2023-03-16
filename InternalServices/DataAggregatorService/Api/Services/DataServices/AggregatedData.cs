using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

internal class AggregatedData
{
    protected readonly CodebookManager _codebookManager = new();

    public AggregatedData()
    {
        Custom = new CustomData(this);
    }

    public CustomData Custom { get; }

    public SalesArrangement SalesArrangement { get; set; } = null!;

    public Case Case { get; set; } = null!;

    public GetMortgageOfferDetailResponse Offer { get; set; } = null!;

    public GetMortgageOfferFPScheduleResponse OfferPaymentSchedule { get; set; } = null!;

    public User User { get; set; } = null!;

    public CustomerDetailResponse Customer { get; set; } = null!;

    public MortgageData Mortgage { get; set; } = null!;

    public HouseholdInfo HouseholdMain { get; set; } = null!;

    public HouseholdInfo? HouseholdCodebtor { get; set; }

    public CustomerOnSA CustomerOnSaDebtor { get; set; } = null!;

    public CustomerOnSA? CustomerOnSaCodebtor { get; set; }

    public Task LoadCodebooks(ICodebookServiceClients codebookService, CancellationToken cancellationToken)
    {
        ConfigureCodebooks(_codebookManager);

        return _codebookManager.Load(codebookService, cancellationToken);
    }

    public virtual Task LoadAdditionalData(CancellationToken cancellationToken) => Task.CompletedTask;

    protected virtual void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
    }
}