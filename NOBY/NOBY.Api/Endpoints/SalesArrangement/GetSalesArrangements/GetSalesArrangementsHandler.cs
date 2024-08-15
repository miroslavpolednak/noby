using System.ComponentModel.DataAnnotations;
using DomainServices.SalesArrangementService.Clients;
using CIS.Core;
using DomainServices.CodebookService.Clients;
using NOBY.Services.SalesArrangementAuthorization;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangements;

internal sealed class GetSalesArrangementsHandler(
    ISalesArrangementServiceClient _salesArrangementService,
    ICodebookServiceClient _codebookService)
        : IRequestHandler<GetSalesArrangementsRequest, List<SalesArrangementGetSalesArrangementsItem>>
{
    public async Task<List<SalesArrangementGetSalesArrangementsItem>> Handle(GetSalesArrangementsRequest request, CancellationToken cancellationToken)
    {
        var result = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken: cancellationToken);

        // seznam typu k doplneni nazvu SA
        var saTypeList = await _codebookService.SalesArrangementTypes(cancellationToken);
        var productTypes = await _codebookService.ProductTypes(cancellationToken);

        var model = result.SalesArrangements
            .Where(t => t.State != (int)EnumSalesArrangementStates.NewArrangement && !ISalesArrangementAuthorizationService.RefinancingSATypes.Contains(t.SalesArrangementTypeId))
            .Select(t => new SalesArrangementGetSalesArrangementsItem
            {
                SalesArrangementId = t.SalesArrangementId,
                SalesArrangementTypeId = t.SalesArrangementTypeId,
                State = (EnumSalesArrangementStates)t.State,
                StateText = ((EnumSalesArrangementStates)t.State).GetAttribute<DisplayAttribute>()?.Name ?? "",
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
}