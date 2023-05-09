﻿using System.ComponentModel.DataAnnotations;
using DomainServices.SalesArrangementService.Clients;
using CIS.Core;
using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangements;

internal sealed class GetSalesArrangementsHandler
    : IRequestHandler<GetSalesArrangementsRequest, List<Dto.SalesArrangementListItem>>
{
    public async Task<List<Dto.SalesArrangementListItem>> Handle(GetSalesArrangementsRequest request, CancellationToken cancellationToken)
    {
        var result = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken: cancellationToken);

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
            var saType = saTypeList.FirstOrDefault(x => x.Id == t.SalesArrangementTypeId);
            t.ProductName = productTypes.FirstOrDefault(t => t.Id == saType?.ProductTypeId)?.Name;
            t.SalesArrangementTypeText = saType?.Name;
        });

        return model;
    }

    private readonly ICodebookServiceClients _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public GetSalesArrangementsHandler(
        ISalesArrangementServiceClient salesArrangementService, 
        ICodebookServiceClients codebookService)
    {
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
    }
}