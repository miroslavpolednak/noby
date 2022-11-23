using System.Collections.Immutable;
using System.Runtime.Serialization;

namespace CIS.Core.Exceptions;

/// <summary>
/// HTTP 409. Vyhazovat pokud prováděná akce je v konfliktu s existující byznys logikou. Podporuje kolekci chybových hlášení.
/// </summary>
/// <remarks>
/// Např. pokud mám vrátit detail klienta, ale v CM je více klientů se stejným ID.
/// </remarks>
public sealed class CisConflictException 
    : BaseCisException
{
    /// <summary>
    /// Seznam chyb.
    /// </summary>
    /// <remarks>
    /// Key: CIS error kód <br/>
    /// Message: chybová zpráva
    /// </remarks>
    public IImmutableList<(string Key, string Message)>? Errors { get; init; }

    public CisConflictException(string message)
        : base(BaseCisException.UnknownExceptionCode, message)
    {
    }

    public CisConflictException(int exceptionCode, string message)
        : base(exceptionCode, message)
    {
    }

    public CisConflictException(IEnumerable<(string Key, string Message)> errors, string message = "")
        : base(BaseCisException.UnknownExceptionCode, message)
    {
        Errors = errors.ToImmutableList();
    }

    public CisConflictException(IEnumerable<(int Key, string Message)> errors, string message = "")
        : base(BaseCisException.UnknownExceptionCode, message)
    {
        Errors = errors.Select(t => (t.Key.ToString(System.Globalization.CultureInfo.InvariantCulture), t.Message)).ToImmutableList();
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        if (Errors is not null)
            info.AddValue(nameof(Errors), Errors, typeof(List<(string Key, string Message)>));
    }
}
