using _DocSaEnum = DomainServices.DocumentOnSAService.Contracts;
using _SharedEnum = SharedTypes.Enums;

namespace NOBY.Api.Extensions;

public static class EnumExtensions
{
    public static EnumSigningSource MapToCisEnum(this _DocSaEnum.Source source)
    {
        return source switch
        {
            _DocSaEnum.Source.Noby => EnumSigningSource.Noby,
            _DocSaEnum.Source.Workflow => EnumSigningSource.Workflow,
            _ => EnumSigningSource.Unknown
        };
    }

    public static _SharedEnum.Source MapToDocOnSaEnum(this _DocSaEnum.Source source)
    {
        return source switch
        {
            _DocSaEnum.Source.Noby => _SharedEnum.Source.Noby,
            _DocSaEnum.Source.Workflow => _SharedEnum.Source.Workflow,
            _ => _SharedEnum.Source.Unknown
        };
    }
}
