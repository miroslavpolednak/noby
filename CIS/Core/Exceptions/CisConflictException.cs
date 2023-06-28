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
    /// <param name="message">Chybová zpráva</param>
    public CisConflictException(string message)
        : base(BaseCisException.UnknownExceptionCode, message)
    { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public CisConflictException(int exceptionCode, string message)
        : base(exceptionCode, message)
    { }
}