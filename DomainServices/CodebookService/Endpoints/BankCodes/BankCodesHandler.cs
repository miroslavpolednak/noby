using DomainServices.CodebookService.Contracts.Endpoints.BankCodes;

namespace DomainServices.CodebookService.Endpoints.BankCodes
{
    public class BankCodesHandler
        : IRequestHandler<BankCodesRequest, List<BankCodeItem>>
    {

        public async Task<List<BankCodeItem>> Handle(BankCodesRequest request, CancellationToken cancellationToken)
        {
            return await FastMemoryCache.GetOrCreate<BankCodeItem>(nameof(BankCodesHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<BankCodeItem>(_sqlQuery, cancellationToken)
            );
        }

        const string _sqlQuery = @"SELECT KOD_BANKY 'BankCode', NAZOV_BANKY 'Name', SKRAT_NAZOV_BANKY 'ShortName', SKRATKA_STATU_PRE_IBAN 'State', CASE WHEN SYSDATETIME() <= ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                                   FROM SBR.CIS_KODY_BANK ORDER BY KOD_BANKY ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<BankCodesHandler> _logger;

        public BankCodesHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<BankCodesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}


//BankCode = KOD_BANKY

//Name = NAZOV_BANKY
//    ShortName = SKRAT_NAZOV_BANKY
//State = SKRATKA_STATU_PRE_IBAN

//IsValid = sysdate > PLATNOST_DO