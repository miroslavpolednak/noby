using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ResidencyTypes;

namespace DomainServices.CodebookService.Endpoints.ResidencyTypes
{
    public class ResidencyTypesHandler
        : IRequestHandler<ResidencyTypesRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(ResidencyTypesRequest request, CancellationToken cancellationToken)
        {
            return new List<GenericCodebookItem>
            {
                new GenericCodebookItem { Id = 25, Name = "25 - občan ČR s trvalým pobytem na území ČR" },
                new GenericCodebookItem { Id = 26, Name = "26 - občan ČR bez trvalého pobytu na území ČR" },
                new GenericCodebookItem { Id = 21, Name = "21 - občan EU s průkazem nebo potvrzením o pobytu na území ČR" },
                new GenericCodebookItem { Id = 22, Name = "22 - občan EU bez průkazu nebo povolení o pobytu na území ČR" },
                new GenericCodebookItem { Id = 23, Name = "23 - občan 3. státu s trvalým pobytem na území ČR" },
                new GenericCodebookItem { Id = 24, Name = "24 - občan 3. státu bez trvalého pobytu na území ČR" }
            };
        }
    }
}
