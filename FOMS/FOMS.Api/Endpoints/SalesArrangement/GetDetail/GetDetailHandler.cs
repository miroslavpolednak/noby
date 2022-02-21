using System.ComponentModel.DataAnnotations;
using DomainServices.SalesArrangementService.Abstraction;
using CIS.Core;
using DomainServices.CodebookService.Abstraction;
using DSContracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail;

internal class GetDetailHandler
    : IRequestHandler<GetDetailRequest, GetDetailResponse>
{
    public async Task<GetDetailResponse> Handle(GetDetailRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetDetailHandler), request.SalesArrangementId);

        // instance SA
        var saInstance = ServiceCallResult.Resolve<DSContracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));
        
        
        return new GetDetailResponse()
        {
            SalesArrangementId = saInstance.SalesArrangementId,
            SalesArrangementTypeId = saInstance.SalesArrangementTypeId
        };
    }

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<GetDetailHandler> _logger;

    public GetDetailHandler(
        ISalesArrangementServiceAbstraction salesArrangementService, 
        ICodebookServiceAbstraction codebookService, 
        ILogger<GetDetailHandler> logger)
    {
        _logger = logger;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
    }
}