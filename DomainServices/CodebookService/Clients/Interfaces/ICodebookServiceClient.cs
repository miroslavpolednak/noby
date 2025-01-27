﻿using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CodebookService.Clients;

public partial interface ICodebookServiceClient
{
    Task<List<DateOnly>> GetNonBankingDays(DateOnly dateFrom, DateOnly dateTo, CancellationToken cancellationToken = default);

    Task<GetOperatorResponse> GetOperator(string performerLogin, CancellationToken cancellationToken = default);

    Task<GetDeveloperResponse> GetDeveloper(int developerId, CancellationToken cancellationToken = default);

    Task<GetDeveloperProjectResponse> GetDeveloperProject(int developerId, int developerProjectId, CancellationToken cancellationToken = default);

    Task<List<DeveloperSearchResponse.Types.DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default);

    Task<(string AcvRealEstateTypeId, string BagmanRealEstateTypeId)> GetACVAndBagmanRealEstateType(int? realEstateStateId, int realEstateSubtypeId, int realEstateTypeId, CancellationToken cancellationToken = default);
}
