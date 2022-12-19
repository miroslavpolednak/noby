using CIS.Infrastructure.CisMediatR.Rollback;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Infrastructure.CisMediatR;

/// <summary>
/// Extension metody do startupu aplikace pro registraci behaviors.
/// </summary>
public static class CisMediatrStartupExtensions
{
    /// <summary>
    /// Pridava moznost rollbacku do Mediatr handleru. Rollback se spusti vyhozenim exception kdykoliv v prubehu exekuce handleru. Po ukonceni rollbacku se dana exception propaguje standardne dal do pipeline.
    /// </summary>
    /// <remarks>
    /// Moznost rollbacku se do Mediatr Requestu prida dedenim interface <see cref="IRollbackCapable">IRollbackCapable</see>, napr. class MyRequest : IRequest&lt;T&gt;, IRollbackCapable {}
    /// Dale je nutne vytvorit vlastni kod rollbacku. To je trida dedici z <see cref="IRollbackAction{TRequest}">IRollbackAction</see> - vlozeni teto tridy do DI je v gesci volajici aplikace.
    /// Pro prenos dat mezi Mediatr handlerem a rollback akci je pouzita scoped instance <see cref="IRollbackBag">IRollbackBag</see>. Do teto instance by mel handler postupne ukladat metadata potrebna pro rollback (napr. vytvorena Id entit).
    /// </remarks>
    public static IServiceCollection AddCisMediatrRollbackCapability(this IServiceCollection services)
    {
        services.AddScoped<IRollbackBag, RollbackBag>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RollbackBehavior<,>));

        return services;
    }
}
