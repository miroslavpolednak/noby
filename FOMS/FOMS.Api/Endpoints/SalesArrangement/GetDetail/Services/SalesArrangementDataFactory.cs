using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using CaseContracts = DomainServices.CaseService.Contracts;
using OfferContracts = DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail.Services;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class SalesArrangementDataFactory
{
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> _connectionProvider;

    public SalesArrangementDataFactory(
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService, 
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> connectionProvider)
    {
        _connectionProvider = connectionProvider;
        _caseService = caseService;
        _offerService = offerService;
    }
    
    public ISalesArrangementDataService GetService(ProductTypeCategory productTypeCategory)
        => productTypeCategory switch
        {
            ProductTypeCategory.Mortgage => new SalesArrangementDataMortgage(_offerService, _caseService, _connectionProvider),
            _ => throw new NotImplementedException($"Product category {productTypeCategory} not implemented")
        };
}