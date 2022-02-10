﻿using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ActionCodesSavings;

namespace DomainServices.CodebookService.Endpoints.ActionCodesSavings
{
    public class ActionCodesSavingsHandler
        : IRequestHandler<ActionCodesSavingsRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(ActionCodesSavingsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.ItemFoundInCache(_cacheKey);
                    return await _cache.GetAllAsync<GenericCodebookItem>(_cacheKey);
                }
                else
                {
                    _logger.TryAddItemToCache(_cacheKey);

                    var result = await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItem>(_sqlQuery, cancellationToken);
                    await _cache.SetAllAsync(_cacheKey, result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.GeneralException(ex);
                throw;
            }
        }

        private const string _sqlQuery =
            "SELECT ID_AKCE_SPO 'Id', NAZEV_AKCE_SPO 'Name', CAST(CASE WHEN PLATNOST_DO_ES IS NULL THEN 1 ELSE 0 END as bit) 'IsActual' FROM SBR.AKCE_SPORENI ORDER BY NAZEV_AKCE_SPO ASC";
        
        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<ActionCodesSavingsHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public ActionCodesSavingsHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<ActionCodesSavingsHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "ActionCodesSavings";
    }
}
