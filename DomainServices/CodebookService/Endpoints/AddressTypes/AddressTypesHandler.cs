using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.AddressTypes;

namespace DomainServices.CodebookService.Endpoints.AddressTypes;

public class AddressTypesHandler : IRequestHandler<AddressTypesRequest, List<GenericCodebookItemWithCode>>
{
    public Task<List<GenericCodebookItemWithCode>> Handle(AddressTypesRequest request, CancellationToken cancellationToken)
    {
        var values = FastEnum.GetValues<CIS.Foms.Enums.AddressTypes>()
                .Where(t => t > 0)
                .Select(t => new GenericCodebookItemWithCode
                {
                    Id = (int)t,
                    Code = t.ToString(),
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? ""
                })
                .ToList();

        return Task.FromResult(values);
    }
}


