﻿using _Pr = DomainServices.ProductService.Contracts;

namespace FOMS.Api.Endpoints.Cases.CreateSalesArrangement;

internal sealed class CreateSalesArrangementHandler
    : IRequestHandler<CreateSalesArrangementRequest, CreateSalesArrangementResponse>
{
    public async Task<CreateSalesArrangementResponse> Handle(CreateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // kontrola na kategorii
        if ((await _codebookService.SalesArrangementTypes(cancellationToken)).FirstOrDefault(t => t.Id == request.SalesArrangementTypeId)?.SalesArrangementCategory != 2)
            throw new CisValidationException($"SalesArrangement type not supported");

        // pokud neprojde validace, primo ve Validate() se vyhodi exception
        var builder = await _createService
            .CreateBuilder(request.CaseId, request.SalesArrangementTypeId)
            .Validate(cancellationToken);

        // vytvorit SA
        var newSaId = ServiceCallResult.ResolveAndThrowIfError<int>(await _salesArrangementService.CreateSalesArrangement(request.CaseId, request.SalesArrangementTypeId, null, cancellationToken));

        // update SA params
        var updateRequest = await builder.CreateParameters(newSaId, cancellationToken);
        ServiceCallResult.Resolve(await _salesArrangementService.UpdateSalesArrangementParameters(updateRequest, cancellationToken));

        return new CreateSalesArrangementResponse
        {
            SalesArrangementId = newSaId
        };
    }

    private readonly Services.CreateSalesArrangementParametersFactory _createService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;
    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public CreateSalesArrangementHandler(
        Services.CreateSalesArrangementParametersFactory createService,
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService, 
        DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService, 
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _createService = createService;
        _codebookService = codebookService;
        _productService = productService;
        _salesArrangementService = salesArrangementService;
    }
}
