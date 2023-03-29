using FluentValidation;

namespace CIS.Infrastructure.CisMediatR.GrpcValidation;

public static class GrpcValidationExtensions
{
    public static IRuleBuilderOptions<TRequest, TElement> WithErrorCode<TRequest, TElement>(this IRuleBuilderOptions<TRequest, TElement> builder, int errorCode)
        => builder.WithErrorCode(errorCode.ToString(System.Globalization.CultureInfo.InvariantCulture));

    public static IRuleBuilderOptions<TRequest, TElement> ThrowCisException<TRequest, TElement>(this IRuleBuilderOptions<TRequest, TElement> builder, GrpcValidationBehaviorExceptionTypes exceptionType)
        => builder.WithState(t => exceptionType);
}
