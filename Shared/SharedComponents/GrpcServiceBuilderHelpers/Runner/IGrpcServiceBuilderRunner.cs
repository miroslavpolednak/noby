namespace SharedComponents.GrpcServiceBuilderHelpers;

public interface IGrpcServiceBuilderRunner<TConfiguration>
    where TConfiguration : class
{
    /// <summary>
    /// Spustí aplikaci. Provádí registraci služeb, následně vytváří WebApplication a registruje middlewares.
    /// </summary>
    /// <remarks>
    /// Celý startup aplikace běží v try-catch bloku, který je v případě pádu aplikace logován.
    /// </remarks>
    void Run();
}

public interface IGrpcServiceBuilderRunner
{
    /// <summary>
    /// Spustí aplikaci. Provádí registraci služeb, následně vytváří WebApplication a registruje middlewares.
    /// </summary>
    /// <remarks>
    /// Celý startup aplikace běží v try-catch bloku, který je v případě pádu aplikace logován.
    /// </remarks>
    void Run();
}