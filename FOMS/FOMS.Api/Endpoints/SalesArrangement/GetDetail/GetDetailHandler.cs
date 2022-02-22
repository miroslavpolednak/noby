using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using SaContracts = DomainServices.SalesArrangementService.Contracts;
using CaseContracts = DomainServices.CaseService.Contracts;
using OfferContracts = DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail;

internal class GetDetailHandler
    : IRequestHandler<GetDetailRequest, GetDetailResponse>
{
    public async Task<GetDetailResponse> Handle(GetDetailRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetDetailHandler), request.SalesArrangementId);

        // instance SA
        var saInstance = ServiceCallResult.Resolve<SaContracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));
        // instance Case
        var saCase = ServiceCallResult.Resolve<CaseContracts.Case>(await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken));
        
        // check existing offerId
        if (!saInstance.OfferId.HasValue)
            throw new CisArgumentNullException(ErrorCodes.SalesArrangementOfferIdIsNull, $"Offer does not exists for Sales Arrangemetnt #{request.SalesArrangementId}", "OfferId");
        
        // kategorie produktu
        int? productTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken)).First(t => t.Id == saInstance.SalesArrangementTypeId).ProductTypeId
            ?? throw new CisNotFoundException(ErrorCodes.SalesArrangementProductCategoryNotFound, $"ProductCategory for SalesArrangementTypeId={saInstance.SalesArrangementTypeId} not found");
        var productCategory = (await _codebookService.ProductTypes(cancellationToken)).FirstOrDefault(t => t.Id == productTypeId)?.ProductCategory
            ?? throw new CisNotFoundException(ErrorCodes.SalesArrangementProductCategoryNotFound, $"ProductCategory for ProductTypeId={productTypeId} not found");

        object detailData;

        switch (productCategory)
        {
            case ProductTypeCategory.Mortgage:
                // get mortgage data
                var offerInstance = ServiceCallResult.Resolve<OfferContracts.SimulateMortgageResponse>(await _offerService.GetMortgageData(saInstance.OfferId.Value, cancellationToken));
                // create response object
                detailData = new Dto.MortgageDetailDto()
                {
                    ContractNumber = saCase.Data.ContractNumber,
                    InterestRate = offerInstance.Outputs.InterestRate,
                    FixationPeriod = offerInstance.Inputs.FixedLengthPeriod,
                    LoanAmount = offerInstance.Outputs.LoanAmount,
                    ExpectedDateOfDrawing = offerInstance.Inputs.ExpectedDateOfDrawing
                };
                break;
            
            default:
                throw new NotImplementedException("Processing for this ProductType has not been implemented");
        }
        
        return new GetDetailResponse()
        {
            SalesArrangementId = saInstance.SalesArrangementId,
            SalesArrangementTypeId = saInstance.SalesArrangementTypeId,
            Data = detailData
        };
    }

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<GetDetailHandler> _logger;
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    
    public GetDetailHandler(
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService,
        ISalesArrangementServiceAbstraction salesArrangementService, 
        ICodebookServiceAbstraction codebookService, 
        ILogger<GetDetailHandler> logger)
    {
        _logger = logger;
        _caseService = caseService;
        _codebookService = codebookService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
    }
}