﻿using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.GetRealEstateValuationList;

internal sealed class GetRealEstateValuationListHandler(RealEstateValuationServiceDbContext _dbContext)
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
                ValuationTypeId = (ValuationTypes)t.ValuationTypeId,
                IsRevaluationRequired = t.IsRevaluationRequired,
                ValuationSentDate = t.ValuationSentDate,
                RealEstateStateId = t.RealEstateStateId,
                Address = t.Address,
                OrderId = t.OrderId,
                PreorderId = t.PreorderId,
                IsOnlineDisqualified = t.IsOnlineDisqualified
            };

            if (t.Prices is not null)
            {
                item.Prices.AddRange(t.Prices.Select(tt => new PriceDetail
                {
                    Price = tt.Price,
                    PriceSourceType = tt.PriceSourceType
                }));
            }

            if (t.PossibleValuationTypeId is not null)
            {
                item.PossibleValuationTypeId.AddRange(t.PossibleValuationTypeId);
            }

            return item;
        }));

        return response;
    }
}
