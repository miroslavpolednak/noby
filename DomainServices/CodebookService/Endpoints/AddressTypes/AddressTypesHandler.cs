using DomainServices.CodebookService.Contracts.Endpoints.AddressTypes;

namespace DomainServices.CodebookService.Endpoints.AddressTypes;

public class AddressTypesHandler : IRequestHandler<AddressTypesRequest, List<AddressTypeItem>>
{
    public Task<List<AddressTypeItem>> Handle(AddressTypesRequest request, CancellationToken cancellationToken)
    {
        var values = Enum.GetValues<CIS.Foms.Enums.AddressTypes>()
                .Where(t => t > 0)
                .Select(t => new AddressTypeItem
                {
                    Id = (int)t,
                    Code = t.ToString(),
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? ""
                })
                .ToList();

        return Task.FromResult(values);
    }
}


