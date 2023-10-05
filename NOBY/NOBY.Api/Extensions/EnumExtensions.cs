using _CisEnum = SharedTypes.Enums;
using _DocSaEnum = DomainServices.DocumentOnSAService.Contracts;

namespace NOBY.Api.Extensions;

public static class EnumExtensions
{
    public static _CisEnum.Source MapToCisEnum(this _DocSaEnum.Source source)
    {
        return source switch
        {
            _DocSaEnum.Source.Noby => _CisEnum.Source.Noby,
            _DocSaEnum.Source.Workflow => _CisEnum.Source.Workflow,
            _ => _CisEnum.Source.Unknown
        };
    }
}
