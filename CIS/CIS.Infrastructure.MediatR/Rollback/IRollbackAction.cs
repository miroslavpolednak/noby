namespace CIS.Infrastructure.MediatR.Rollback;

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
    /// <returns></returns>
    Task ExecuteRollback(Exception exception, TRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
