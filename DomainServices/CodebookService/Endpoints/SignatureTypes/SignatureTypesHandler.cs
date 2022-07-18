using DomainServices.CodebookService.Contracts.Endpoints.SignatureTypes;

namespace DomainServices.CodebookService.Endpoints.SignatureTypes;

public class SignatureTypesHandler
    : IRequestHandler<SignatureTypesRequest, List<SignatureTypeItem>>
{
    public Task<List<SignatureTypeItem>> Handle(SignatureTypesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.SignatureTypes>()
            .Select(t => new SignatureTypeItem
            {
                Id = (int)t,
                EnumValue = t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                IsDefault = t.HasAttribute<CIS.Core.Attributes.CisDefaultValueAttribute>()
            })
            .ToList();

        return Task.FromResult(values);
    }
}