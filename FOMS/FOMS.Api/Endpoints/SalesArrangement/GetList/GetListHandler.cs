using System.ComponentModel.DataAnnotations;
using DomainServices.SalesArrangementService.Abstraction;
using CIS.Core;
using DomainServices.CodebookService.Abstraction;

namespace FOMS.Api.Endpoints.SalesArrangement.Handlers;

internal class GetListHandler
    : IRequestHandler<Dto.GetListRequest, List<Dto.SalesArrangementListItem>>
{
    public async Task<List<Dto.SalesArrangementListItem>> Handle(Dto.GetListRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetListHandler), request.CaseId);

        var result = ServiceCallResult.Resolve<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementListResponse>(await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken: cancellationToken));

        _logger.FoundItems(result.SalesArrangements.Count);

        // seznam typu k doplneni nazvu SA
        var saTypeList = await _codebookService.SalesArrangementTypes(cancellationToken);
        
        return result.SalesArrangements.Select(t => new Dto.SalesArrangementListItem
        {
            SalesArrangementTypeId = t.SalesArrangementTypeId,
            SalesArrangementTypeText = saTypeList.First(x => x.Id == t.SalesArrangementTypeId).Name,
            State = (CIS.Core.Enums.SalesArrangementStates)t.State,
            StateText = ((CIS.Core.Enums.SalesArrangementStates)t.State).GetAttribute<DisplayAttribute>()?.Name ?? "",
            OfferId = t.OfferId,
            CreatedBy = t.Created.UserName,
            CreatedTime = t.Created.DateTime
        }).ToList();
    }

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<GetListHandler> _logger;

    public GetListHandler(ISalesArrangementServiceAbstraction salesArrangementService, ICodebookServiceAbstraction codebookService, ILogger<GetListHandler> logger)
    {
        _logger = logger;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
    }
}