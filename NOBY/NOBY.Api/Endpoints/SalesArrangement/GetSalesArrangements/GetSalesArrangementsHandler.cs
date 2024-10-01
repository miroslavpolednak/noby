using DomainServices.SalesArrangementService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Contracts;
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
        var saStates = await _codebookService.SalesArrangementStates(cancellationToken);

        var model = result.SalesArrangements
			.Where(t => t.IsInState(SalesArrangementHelpers.AllExceptNewSalesArrangementStates) && !ISalesArrangementAuthorizationService.RefinancingSATypes.Contains(t.SalesArrangementTypeId))
            .Select(t =>
            {
                var state = saStates.First(x => x.Id == t.State);

                var model = new SalesArrangementGetSalesArrangementsItem
                {
                    SalesArrangementId = t.SalesArrangementId,
                    SalesArrangementTypeId = t.SalesArrangementTypeId,
                    State = (EnumSalesArrangementStates)t.State,
                    StateName = state.Name,
                    StateIndicator = (EnumStateIndicators)state.Indicator,
                    OfferId = t.OfferId,
                    CreatedBy = t.Created.UserName,
                    CreatedTime = t.Created.DateTime
                };

                return model;
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