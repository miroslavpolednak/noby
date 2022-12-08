using System.Collections.Immutable;
using System.Runtime.Serialization;

namespace CIS.Core.Exceptions;

public abstract class BaseCisValidationException
    : BaseCisException
{
    /// <summary>
    /// Seznam chyb.
    /// </summary>
    /// <remarks>
    /// Key: CIS error kód <br/>
    /// Message: chybová zpráva
    /// </remarks>
    public ImmutableList<(string Key, string Message)>? Errors { get; init; }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public BaseCisValidationException(int exceptionCode, string message)
        : base(exceptionCode, message)
    {
        Errors = new List<(string Key, string Message)>
        {
            (Key: exceptionCode.ToString(), Message: message)
        }.ToImmutableList();
    }

    /// <param name="errors">Seznam chyb</param>
    /// <param name="message">Souhrná chybová zpráva</param>
    public BaseCisValidationException(IEnumerable<(string Key, string Message)> errors, string message = "")
        : base(BaseCisException.UnknownExceptionCode, message)
    {
        if (errors is null || !errors.Any())
            throw new ArgumentNullException(nameof(errors));

        Errors = errors.ToImmutableList();
    }

    /// <param name="errors">Seznam chyb</param>
    /// <param name="message">Souhrná chybová zpráva</param>
    public BaseCisValidationException(IEnumerable<(int Key, string Message)> errors, string message = "")
        : base(BaseCisException.UnknownExceptionCode, message)
    {
        if (errors is null || !errors.Any())
            throw new ArgumentNullException(nameof(errors));

        Errors = errors.Select(t => (t.Key.ToString(System.Globalization.CultureInfo.InvariantCulture), t.Message)).ToImmutableList();
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Errors), Errors, typeof(List<(string Key, string Message)>));
    }
}
