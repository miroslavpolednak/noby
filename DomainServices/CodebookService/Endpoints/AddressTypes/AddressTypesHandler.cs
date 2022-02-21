using DomainServices.CodebookService.Contracts.Endpoints.AddressTypes;

namespace DomainServices.CodebookService.Endpoints.AddressTypes;

public class AddressTypesHandler : IRequestHandler<AddressTypesRequest, List<AddressTypeItem>>
{
    public Task<List<AddressTypeItem>> Handle(AddressTypesRequest request, CancellationToken cancellationToken)
    => Task.FromResult(new List<AddressTypeItem>() 
    { 
        new AddressTypeItem { Id = 1, Code = "PERMANENT", Name = "Trvalá adresa" },
        new AddressTypeItem { Id = 2, Code = "MAILING", Name = "Korespondenční" },
        new AddressTypeItem { Id = 3, Code = "ABROAD", Name = "Adresa v zahraničí pro daňové nerezidenty" }
    });
}


