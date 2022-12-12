using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices;

internal class AggregatedData
{
    public AggregatedData()
    {
        Custom = new CustomData(this);
    }

    public CustomData Custom { get; }

    public SalesArrangement SalesArrangement { get; set; }

    public Case Case { get; set; }

    public GetMortgageOfferDetailResponse Offer { get; set; }

    public GetMortgageOfferFPScheduleResponse OfferPaymentSchedule { get; set; }

    public User User { get; set; }

    public CustomerDetailResponse Customer { get; set; }

    public MortgageData Mortgage { get; set; }

    public DateTime CurrentDateTime => DateTime.Now;

    public virtual Task LoadCodebooks(ICodebookServiceClients codebookService) => Task.CompletedTask;
}