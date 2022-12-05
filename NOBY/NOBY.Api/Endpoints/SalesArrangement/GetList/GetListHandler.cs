using System.ComponentModel.DataAnnotations;
using DomainServices.SalesArrangementService.Clients;
using CIS.Core;
using DomainServices.CodebookService.Clients;
using DSContracts = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.GetList;

internal class GetListHandler
    : IRequestHandler<GetListRequest, List<Dto.SalesArrangementListItem>>
{
    public async Task<List<Dto.SalesArrangementListItem>> Handle(GetListRequest request, CancellationToken cancellationToken)
    {
        var result = ServiceCallResult.ResolveAndThrowIfError<DSContracts.GetSalesArrangementListResponse>(await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken: cancellationToken));

        _logger.FoundItems(result.SalesArrangements.Count);

        // seznam typu k doplneni nazvu SA
        var saTypeList = await _codebookService.SalesArrangementTypes(cancellationToken);
        var productTypes = await _codebookService.ProductTypes(cancellationToken);

        var model = result.SalesArrangements.Select(t => new Dto.SalesArrangementListItem
        {
            SalesArrangementId = t.SalesArrangementId,
            SalesArrangementTypeId = t.SalesArrangementTypeId,
            State = (CIS.Foms.Enums.SalesArrangementStates)t.State,
            StateText = ((CIS.Foms.Enums.SalesArrangementStates)t.State).GetAttribute<DisplayAttribute>()?.Name ?? "",
            OfferId = t.OfferId,
            CreatedBy = t.Created.UserName,
            CreatedTime = t.Created.DateTime
        }).ToList();

        model.ForEach(t =>
        {
            var saType = saTypeList.First(x => x.Id == t.SalesArrangementTypeId);
            t.ProductName = productTypes.First(t => t.Id == saType.ProductTypeId).Name;
            t.SalesArrangementTypeText = saType.Name;
        });

        return model;
    }

    private readonly ICodebookServiceClients _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ILogger<GetListHandler> _logger;

    public GetListHandler(
        ISalesArrangementServiceClient salesArrangementService, 
        ICodebookServiceClients codebookService, 
        ILogger<GetListHandler> logger)
    {
        _logger = logger;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
    }
}