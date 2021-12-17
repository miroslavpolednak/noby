﻿using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;

namespace DomainServices.CodebookService.Endpoints.SalesArrangementTypes
{
    public class ProductInstanceTypesHandler
        : IRequestHandler<SalesArrangementTypesRequest, List<SalesArrangementTypeItem>>
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<SalesArrangementTypeItem>> Handle(SalesArrangementTypesRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return new List<SalesArrangementTypeItem>
            {
                new SalesArrangementTypeItem() { Id = "1001001", Name = "Stavební spoření" },
                new SalesArrangementTypeItem() { Id = "1002001", Name = "Servisni form" },
                new SalesArrangementTypeItem() { Id = "3301001", Name = "Uver" },
            };
        }
    }
}
