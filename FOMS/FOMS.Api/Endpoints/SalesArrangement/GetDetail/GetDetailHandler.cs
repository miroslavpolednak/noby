using CIS.Foms.Enums;
using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.CodebookService.Abstraction;
using SaContracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail;

internal class GetDetailHandler
    : IRequestHandler<GetDetailRequest, GetDetailResponse>
{
    public async Task<GetDetailResponse> Handle(GetDetailRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetDetailHandler), request.SalesArrangementId);

        // instance SA
        var saInstance = ServiceCallResult.Resolve<SaContracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        // kategorie produktu
        int? productTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken)).First(t => t.Id == saInstance.SalesArrangementTypeId).ProductTypeId
            ?? throw new CisNotFoundException(ErrorCodes.SalesArrangementProductCategoryNotFound, $"ProductCategory for SalesArrangementTypeId={saInstance.SalesArrangementTypeId} not found");
        var productCategory = (await _codebookService.ProductTypes(cancellationToken)).FirstOrDefault(t => t.Id == productTypeId)?.ProductCategory
            ?? throw new CisNotFoundException(ErrorCodes.SalesArrangementProductCategoryNotFound, $"ProductCategory for ProductTypeId={productTypeId} not found");

        // data o SA
        object detailData = await _dataFactory
            .GetService(productCategory)
            .GetData(saInstance.CaseId, saInstance.OfferId, (SalesArrangementStates)saInstance.State, cancellationToken);

        return new GetDetailResponse()
        {
            SalesArrangementId = saInstance.SalesArrangementId,
            SalesArrangementTypeId = saInstance.SalesArrangementTypeId,
            ProductCategory = productCategory,
            CreatedBy = saInstance.Created.UserName,
            CreatedTime = saInstance.Created.DateTime,
            Data = detailData
        };
    }
    
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<GetDetailHandler> _logger;
    private readonly Services.SalesArrangementDataFactory _dataFactory;
    
    public GetDetailHandler(
        Services.SalesArrangementDataFactory dataFactory,
        ISalesArrangementServiceAbstraction salesArrangementService, 
        ICodebookServiceAbstraction codebookService, 
        ILogger<GetDetailHandler> logger)
    {
        _dataFactory = dataFactory;
        _logger = logger;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
    }
}