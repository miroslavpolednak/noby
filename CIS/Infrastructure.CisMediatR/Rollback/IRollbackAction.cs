namespace CIS.Infrastructure.CisMediatR.Rollback;

/// <summary>
/// Deklarace kontraktu pro tridu s kodem pro provedeni rollbacku Mediatr handleru.
/// </summary>
/// <typeparam name="TRequest">Mediatr Request type</typeparam>
public interface IRollbackAction<TRequest>
    where TRequest : IBaseRequest
{
    /// <summary>
    /// Metoda s implementaci vlastniho rollbacku
    /// </summary>
    /// <param name="exception">Vyjimka, ktera rollback spustila</param>
    /// <param name="request">Puvodni Mediatr request</param>
    Task ExecuteRollback(Exception exception, TRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Pokud bude nastaveno na True, tak se misto exception, ktera rollback zpusobila, vrati exception z OverrideException()
    /// </summary>
    bool OverrideThrownException { get => false; }

    /// <summary>
    /// Vytvoreni exception misto puvodni, ktera spustila rollback
    /// </summary>
    /// <param name="exception">Puvodni vyjimka</param>
    Exception OnOverrideException(Exception exception) => throw new NotImplementedException();
}
