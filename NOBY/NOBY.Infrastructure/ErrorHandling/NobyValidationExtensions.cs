using FluentValidation;

namespace NOBY.Infrastructure.ErrorHandling;

public static class NobyValidationExtensions
{
    public static IRuleBuilderOptions<TRequest, TElement> WithErrorCode<TRequest, TElement>(this IRuleBuilderOptions<TRequest, TElement> builder, int errorCode)
        => builder.WithErrorCode(errorCode.ToString(System.Globalization.CultureInfo.InvariantCulture));
}
