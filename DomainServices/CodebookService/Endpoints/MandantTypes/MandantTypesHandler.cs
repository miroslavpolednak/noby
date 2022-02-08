using DomainServices.CodebookService.Contracts.Endpoints.MandantTypes;
using System.Reflection;

namespace DomainServices.CodebookService.Endpoints.MandantTypes;

public class MandantTypesHandler
    : IRequestHandler<MandantTypesRequest, List<MandantTypesItem>>
{
    static TAttribute GetAttribute<TAttribute>(Enum enumValue) where TAttribute : Attribute
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>()!;
    }

    public Task<List<MandantTypesItem>> Handle(MandantTypesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = Enum.GetValues<CIS.Core.Mandants>()
            .Where(t => t > 0)
            .Select(t => new MandantTypesItem
            {
                Code = t.ToString(),
                StarbuildId = (int)t,
                Name = GetAttribute<System.ComponentModel.DescriptionAttribute>(t).Description
            })
            .ToList();

        return Task.FromResult(values);
    }
}
