using DomainServices.CaseService.Contracts;
using DomainServices.CustomerService.Contracts;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms;

public interface IServiceFormData : IEasFormData
{
    SalesArrangement SalesArrangement { get; }

    Case Case { get; }

    MortgageData Mortgage { get; }

    CustomerDetailResponse Customer { get; }
}