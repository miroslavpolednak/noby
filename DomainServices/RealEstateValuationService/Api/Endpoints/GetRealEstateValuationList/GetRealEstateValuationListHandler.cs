﻿using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetRealEstateValuationList;

internal sealed class GetRealEstateValuationListHandler
    : IRequestHandler<GetRealEstateValuationListRequest, GetRealEstateValuationListResponse>
{
    public async Task<GetRealEstateValuationListResponse> Handle(GetRealEstateValuationListRequest request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.RealEstateValuations
            .AsNoTracking()
            .Where(t => t.CaseId == request.CaseId)
            .OrderBy(t => t.RealEstateValuationId)
            .ToListAsync(cancellationToken);

        var response = new GetRealEstateValuationListResponse();

        response.RealEstateValuationList.AddRange(list.Select(t =>
        {
            var item = new RealEstateValuationListItem
            {
                RealEstateTypeId = t.RealEstateTypeId,
                CaseId = t.CaseId,
                IsLoanRealEstate = t.IsLoanRealEstate,
                DeveloperApplied = t.DeveloperApplied,
                DeveloperAllowed = t.DeveloperAllowed,
                RealEstateValuationId = t.RealEstateValuationId,
                ValuationStateId = t.ValuationStateId,
                ValuationTypeId = (Contracts.ValuationTypes)t.ValuationTypeId,
                IsRevaluationRequired = t.IsRevaluationRequired,
                ValuationSentDate = t.ValuationSentDate,
                RealEstateStateId = t.RealEstateStateId,
                Address = t.Address,
                OrderId = t.OrderId,
                PreorderId = t.PreorderId,
                IsOnlineDisqualified = t.IsOnlineDisqualified,
                ValuationResultCurrentPrice = t.ValuationResultCurrentPrice,
                ValuationResultFuturePrice = t.ValuationResultFuturePrice
            };

            if (!string.IsNullOrEmpty(t.PossibleValuationTypeId)) 
            {
                item.PossibleValuationTypeId.AddRange(t.PossibleValuationTypeId!.Split(',').Select(t => Convert.ToInt32(t, CultureInfo.InvariantCulture)));
            }

            return item;
        }));

        return response;
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public GetRealEstateValuationListHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
