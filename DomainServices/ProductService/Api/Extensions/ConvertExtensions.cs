namespace DomainServices.ProductService.Api;

internal static class ConvertExtensions
{
	public static decimal? ToDecimal(this double? value)
		=> value.HasValue ? Convert.ToDecimal(value.Value) : null;

	public static decimal ToDecimal(this double? value, decimal defaultValue)
		=> value.HasValue ? Convert.ToDecimal(value.Value) : defaultValue;

	public static decimal ToDecimal(this double value)
		=> Convert.ToDecimal(value);
}
