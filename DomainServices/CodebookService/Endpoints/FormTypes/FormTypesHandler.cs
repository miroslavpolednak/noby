﻿using DomainServices.CodebookService.Contracts.Endpoints.FormTypes;
using static Azure.Core.HttpHeader;

namespace DomainServices.CodebookService.Endpoints.FormTypes;

public class FormTypesHandler
    : IRequestHandler<FormTypesRequest, List<FormTypeItem>>
{
    public async Task<List<FormTypeItem>> Handle(FormTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<FormTypeItem>(nameof(FormTypesHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<FormTypeItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT FORMULAR_ID 'Id', CISLO 'Type', VERZE 'Version', NAZEV 'Name', MANDANT 'MandantId', CASE WHEN SYSDATETIME() BETWEEN PLATNOST_OD AND ISNULL(PLATNOST_DO, '9999-12-31') THEN 1 ELSE 0 END 'IsValid'
                               FROM [SBR].[CIS_FORMULARE] ORDER BY FORMULAR_ID ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<FormTypesHandler> _logger;

    public FormTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<FormTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}   