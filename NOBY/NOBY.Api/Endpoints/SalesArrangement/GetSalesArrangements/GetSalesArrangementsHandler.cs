using System.ComponentModel.DataAnnotations;
using DomainServices.SalesArrangementService.Clients;
using CIS.Core;
using DomainServices.CodebookService.Clients;
using CIS.Core.Security;
using NOBY.Services.SalesArrangementAuthorization;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangements;

internal sealed class GetSalesArrangementsHandler(
    ISalesArrangementServiceClient _salesArrangementService,
    ICodebookServiceClient _codebookService,
    ICurrentUserAccessor _currentUserAccessor)
        : IRequestHandler<GetSalesArrangementsRequest, List<SalesArrangementGetSalesArrangementsItem>>
{
    public async Task<List<SalesArrangementGetSalesArrangementsItem>> Handle(GetSalesArrangementsRequest request, CancellationToken cancellationToken)
    {
        var result = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken: cancellationToken);

        // seznam typu k doplneni nazvu SA
        var saTypeList = await _codebookService.SalesArrangementTypes(cancellationToken);
        var productTypes = await _codebookService.ProductTypes(cancellationToken);

        var query = result.SalesArrangements.Where(t => t.State != (int)SalesArrangementStates.NewArrangement);
        // refinancing
        if (!_currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_RefinancingAccess))
        {
            query = query.Where(t => !ISalesArrangementAuthorizationService.RefinancingSATypes.Contains(t.SalesArrangementTypeId));
        }
        // ostatni sa
        if (!_currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access))
        {
            query = query.Where(t => ISalesArrangementAuthorizationService.RefinancingSATypes.Contains(t.SalesArrangementTypeId));
        }

        var model = query
            .Select(t => new SalesArrangementGetSalesArrangementsItem
            {
                SalesArrangementId = t.SalesArrangementId,
                SalesArrangementTypeId = t.SalesArrangementTypeId,
                State = (EnumSalesArrangementStates)t.State,
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
}