using System.ComponentModel.DataAnnotations;
using DomainServices.SalesArrangementService.Abstraction;
using CIS.Core;
using DomainServices.CodebookService.Abstraction;
using DSContracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetList;

internal class GetListHandler
    : IRequestHandler<GetListRequest, List<Dto.SalesArrangementListItem>>
{
    public async Task<List<Dto.SalesArrangementListItem>> Handle(GetListRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetListHandler), request.CaseId);

        var result = ServiceCallResult.Resolve<DSContracts.GetSalesArrangementListResponse>(await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken: cancellationToken));

        _logger.FoundItems(result.SalesArrangements.Count);

        // seznam typu k doplneni nazvu SA
        var saTypeList = await _codebookService.SalesArrangementTypes(cancellationToken);
        
        return result.SalesArrangements.Select(t => new Dto.SalesArrangementListItem
        {
            SalesArrangementTypeId = t.SalesArrangementTypeId,
            SalesArrangementTypeText = saTypeList.First(x => x.Id == t.SalesArrangementTypeId).Name,
            State = (CIS.Foms.Enums.SalesArrangementStates)t.State,
            StateText = ((CIS.Foms.Enums.SalesArrangementStates)t.State).GetAttribute<DisplayAttribute>()?.Name ?? "",
            OfferId = t.OfferId,
            CreatedBy = t.Created.UserName,
            CreatedTime = t.Created.DateTime
        }).ToList();
    }

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<GetListHandler> _logger;

    public GetListHandler(
        ISalesArrangementServiceAbstraction salesArrangementService, 
        ICodebookServiceAbstraction codebookService, 
        ILogger<GetListHandler> logger)
    {
        _logger = logger;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
    }
}