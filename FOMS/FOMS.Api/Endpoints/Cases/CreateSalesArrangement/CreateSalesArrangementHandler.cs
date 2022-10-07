using _Pr = DomainServices.ProductService.Contracts;

namespace FOMS.Api.Endpoints.Cases.CreateSalesArrangement;

internal sealed class CreateSalesArrangementHandler
    : IRequestHandler<CreateSalesArrangementRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(CreateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // kontrola na kategorii
        if ((await _codebookService.SalesArrangementTypes(cancellationToken)).FirstOrDefault(t => t.Id == request.SalesArrangementTypeId)?.SalesArrangementCategory != 2)
            throw new CisValidationException($"SalesArrangement type not supported");

        // validace
        // https://wiki.kb.cz/pages/viewpage.action?pageId=424119307#id-%C4%8Cerp%C3%A1ni%C5%BD%C3%A1dosto%C4%8Derp%C3%A1n%C3%AD-Kontrolyp%C5%99edformul%C3%A1%C5%99em%C4%8Derp%C3%A1n%C3%AD
        var productInstance = ServiceCallResult.ResolveAndThrowIfError<_Pr.GetMortgageResponse>(await _productService.GetMortgage(request.CaseId, cancellationToken));

        if (productInstance.Mortgage.AvailableForDrawing <= 0M)
            throw new CisValidationException("Zůstatek pro čerpání je menší nebo rovný nule. Formulář nelze vytvořit");
        

        // vytvorit SA
        var newSaId = ServiceCallResult.ResolveAndThrowIfError<int>(await _salesArrangementService.CreateSalesArrangement(request.CaseId, request.SalesArrangementTypeId, null, cancellationToken));

        return new CreateSalesArrangementResponse
        {
            SalesArrangementId = newSaId
        };
    }

    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;
    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CreateSalesArrangementHandler(
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService, 
        DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService, 
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _codebookService = codebookService;
        _productService = productService;
        _salesArrangementService = salesArrangementService;
    }
}
