using DomainServices.CodebookService.Contracts.Endpoints.PayoutTypes;

namespace DomainServices.CodebookService.Endpoints.PayoutTypes;

public sealed class PayoutTypesHandler
    : IRequestHandler<PayoutTypesRequest, List<PayoutTypeItem>>
{
    public Task<List<PayoutTypeItem>> Handle(PayoutTypesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.PayoutTypes>()
            .Select(t => new PayoutTypeItem
            {
                Id = (int)t,
                EnumValue = t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? ""
            })
            .ToList();

        return Task.FromResult(values);
    }
}
