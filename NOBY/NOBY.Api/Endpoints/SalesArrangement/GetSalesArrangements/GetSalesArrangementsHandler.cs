using System.ComponentModel.DataAnnotations;
using DomainServices.SalesArrangementService.Clients;
using CIS.Core;
using DomainServices.CodebookService.Clients;
using CIS.Core.Security;
using NOBY.Services.SalesArrangementAuthorization;

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

    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public GetSalesArrangementsHandler(
        ISalesArrangementServiceClient salesArrangementService,
        ICodebookServiceClient codebookService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _currentUserAccessor = currentUserAccessor;
    }
}