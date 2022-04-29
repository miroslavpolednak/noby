using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail.Services;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class SalesArrangementDataFactory
{
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> _connectionProvider;
    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public SalesArrangementDataFactory(
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService, 
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        CIS.Core.Data.IConnectionProvider<IKonsdbDapperConnectionProvider> connectionProvider)
    {
        _codebookService = codebookService;
        _connectionProvider = connectionProvider;
        _caseService = caseService;
        _offerService = offerService;
    }
    
    public ISalesArrangementDataService GetService(ProductTypeCategory productTypeCategory)
        => productTypeCategory switch
        {
            ProductTypeCategory.Mortgage => new SalesArrangementDataMortgage(_codebookService, _offerService, _caseService, _connectionProvider),
            _ => throw new NotImplementedException($"Product category {productTypeCategory} not implemented")
        };
}