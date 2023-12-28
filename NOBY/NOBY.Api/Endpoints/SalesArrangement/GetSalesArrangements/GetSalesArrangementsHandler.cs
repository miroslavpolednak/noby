using System.ComponentModel.DataAnnotations;
using DomainServices.SalesArrangementService.Clients;
using CIS.Core;
using DomainServices.CodebookService.Clients;
using SharedTypes.Enums;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangements;

internal sealed class GetSalesArrangementsHandler
    : IRequestHandler<GetSalesArrangementsRequest, List<SharedDto.SalesArrangementListItem>>
{
    public async Task<List<SharedDto.SalesArrangementListItem>> Handle(GetSalesArrangementsRequest request, CancellationToken cancellationToken)
    {
        var result = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken: cancellationToken);

        // seznam typu k doplneni nazvu SA
        var saTypeList = await _codebookService.SalesArrangementTypes(cancellationToken);
        var productTypes = await _codebookService.ProductTypes(cancellationToken);

        var model = result.SalesArrangements
            .Where(t => t.State != (int)SalesArrangementStates.NewArrangement)
            .Select(t => new SharedDto.SalesArrangementListItem
            {
                SalesArrangementId = t.SalesArrangementId,
                SalesArrangementTypeId = t.SalesArrangementTypeId,
                State = (SalesArrangementStates)t.State,
                StateText = ((SalesArrangementStates)t.State).GetAttribute<DisplayAttribute>()?.Name ?? "",
                OfferId = t.OfferId,
                CreatedBy = t.Created.UserName,
                CreatedTime = t.Created.DateTime
            })
            .ToList();

        model.ForEach(t =>
        {
            var saType = saTypeList.FirstOrDefault(x => x.Id == t.SalesArrangementTypeId);
            t.SalesArrangementTypeText = saType?.Name;
        });

        return model;
    }

    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public GetSalesArrangementsHandler(
        ISalesArrangementServiceClient salesArrangementService, 
        ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
    }
}