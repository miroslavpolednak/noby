using CIS.Core;
using DomainServices.CodebookService.Contracts.v1;
using FastEnumUtility;

namespace DomainServices.CodebookService.Clients;

internal static class Helpers
{
    public static Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> GetGenericItems<TEnum>(bool useCode = false)
        where TEnum : struct, Enum
    {
#pragma warning disable CA1305 // Specify IFormatProvider
        return Task.FromResult(FastEnum.GetValues<TEnum>()
            .Where(t => Convert.ToInt32(t) > 0)
            .Select(t => new GenericCodebookResponse.Types.GenericCodebookItem
            {
                Id = Convert.ToInt32(t),
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                Code = useCode ? t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? t.ToString() : null,
                Description = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Description,
                IsValid = true,
                IsDefault = t.HasAttribute<CIS.Core.Attributes.CisDefaultValueAttribute>() ? true : null
            })
            .ToList()!);
#pragma warning restore CA1305 // Specify IFormatProvider
    }
}
