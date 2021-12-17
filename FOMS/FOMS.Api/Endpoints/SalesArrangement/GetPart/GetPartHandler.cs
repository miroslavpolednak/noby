using CIS.Core.Results;

namespace FOMS.Api.Endpoints.SalesArrangement.Handlers;

/*internal class GetPartHandler
    : IRequestHandler<Dto.GetPartRequest, int>
{
    public async Task<object> Handle(Dto.GetPartRequest request, CancellationToken cancellationToken)
    {
        //var salesArrangement = resolveResult(await _salesArrangementService.GetSalesArrangementDetail(request.SalesArrangementId));
        //var processor = _documentFactory.CreateDocumentProcessor(salesArrangement.SalesArrangementType);
        return 0;
    }

    private DomainServices.SalesArrangementService.Contracts.GetSalesArrangementDetailResponse resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementDetailResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };

    private readonly ILogger<GetPartHandler> _logger;
    private readonly DocumentProcessing.IDocumentProcessorFactory _documentFactory;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;

    public GetPartHandler(
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService,
        DocumentProcessing.IDocumentProcessorFactory documentFactory,
        ILogger<GetPartHandler> logger)
    {
        _salesArrangementService = salesArrangementService;
        _logger = logger;
        _documentFactory = documentFactory;
    }
}*/
