using _Db = DomainServices.DocumentOnSAService.Api.Database.Enums;
using _Contract = DomainServices.DocumentOnSAService.Contracts;

namespace DomainServices.DocumentOnSAService.Api.Extensions;

internal static class EnumMappingExtensions
{
    public static _Contract.Source MapToContractEnum(this _Db.Source source)
    {
        return source switch
        {
            _Db.Source.Noby => _Contract.Source.Noby,
            _Db.Source.Workflow => _Contract.Source.Workflow,
            _ => _Contract.Source.Unknown
        };
    }
}
