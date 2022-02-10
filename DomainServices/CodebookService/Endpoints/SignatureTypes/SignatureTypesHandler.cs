using DomainServices.CodebookService.Contracts.Endpoints.SignatureTypes;

namespace DomainServices.CodebookService.Endpoints.SignatureTypes;

public class SignatureTypesHandler
    : IRequestHandler<SignatureTypesRequest, List<SignatureTypeItem>>
{
    public Task<List<SignatureTypeItem>> Handle(SignatureTypesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = Enum.GetValues<CIS.Core.Enums.SignatureTypes>()
            .Select(t => new SignatureTypeItem
            {
                Id = (int)t,
                Value = t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? ""
            })
            .ToList();

        return Task.FromResult(values);
    }
}