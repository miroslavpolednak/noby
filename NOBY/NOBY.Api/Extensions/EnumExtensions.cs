using _DocSaEnum = DomainServices.DocumentOnSAService.Contracts;

namespace NOBY.Api.Extensions;

public static class EnumExtensions
{
    public static Source MapToCisEnum(this _DocSaEnum.Source source)
    {
        return source switch
        {
            _DocSaEnum.Source.Noby => Source.Noby,
            _DocSaEnum.Source.Workflow => Source.Workflow,
            _ => Source.Unknown
        };
    }
}
