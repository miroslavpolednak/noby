using DomainServices.CodebookService.Contracts.Endpoints.CustomerProfiles;

namespace DomainServices.CodebookService.Endpoints.CustomerProfiles;

internal class CustomerProfilesHandler
    : IRequestHandler<CustomerProfilesRequest, List<CustomerProfileItem>>
{
    public Task<List<CustomerProfileItem>> Handle(CustomerProfilesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.CustomerProfiles>()
            .Select(t => new CustomerProfileItem()
            {
                Id = (int)t,
                EnumValue = t,
                Code = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? String.Empty,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? String.Empty,
            })
            .ToList();

        return Task.FromResult(values);
    }
}
