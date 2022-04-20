using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ObligationTypes;

namespace DomainServices.CodebookService.Endpoints.ObligationTypes
{
    public class ObligationTypesHandler
        : IRequestHandler<ObligationTypesRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(ObligationTypesRequest request, CancellationToken cancellationToken)
        {
            // TODO: Redirect to real data source!    
            return new List<GenericCodebookItem>
            {
                new GenericCodebookItem() { Id = 1, Name = "Hypotéka" },            // code MORTGAGE
                new GenericCodebookItem() { Id = 2, Name = "Spotřební úvěr" },      // code UTILITY_LOAN
                new GenericCodebookItem() { Id = 3, Name = "Kreditní karta" },      // code CREDIT_CARD
                new GenericCodebookItem() { Id = 4, Name = "Debet / Kontokorent" }, // code DEBIT
                new GenericCodebookItem() { Id = 5, Name = "Nebankovní půjčka" },   // code NON_BANK_LOAN
            };
        }

        private List<GenericCodebookItem> GetMockData()
        {
            return new List<GenericCodebookItem>
            {
                new GenericCodebookItem() { Id = 1, Name = "Hypotéka" },            // code MORTGAGE
                new GenericCodebookItem() { Id = 2, Name = "Spotřební úvěr" },      // code UTILITY_LOAN
                new GenericCodebookItem() { Id = 3, Name = "Kreditní karta" },      // code CREDIT_CARD
                new GenericCodebookItem() { Id = 4, Name = "Debet / Kontokorent" }, // code DEBIT
                new GenericCodebookItem() { Id = 5, Name = "Nebankovní půjčka" },   // code NON_BANK_LOAN
            };
        }

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<ObligationTypesHandler> _logger;

        public ObligationTypesHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<ObligationTypesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}