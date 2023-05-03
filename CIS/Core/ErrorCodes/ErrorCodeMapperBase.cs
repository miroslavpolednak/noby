using CIS.Core.Exceptions;
using System.Collections.ObjectModel;

namespace CIS.Core.ErrorCodes;

/// <summary>
/// Base třída pro zadávání chybových hlášek zejména pro FluentValidation a Rpc exceptions.
/// </summary>
public abstract class ErrorCodeMapperBase
{
    // Global lock
    private static readonly object _lock = new();

    /// <summary>
    /// Slovník chybových hlášek [ExceptionCode, ExceptionMessage].
    /// </summary>
    public static IErrorCodesDictionary Messages { get; private set; } = null!;

    /// <summary>
    /// Vrátí chybovou hláškou podle zadaného ExceptionCode - ten musí být uvedený v překladovém slovníku Messages.
    /// </summary>
    /// <param name="exceptionCode">Kód chybové hlášky.</param>
    /// <param name="parameter">Volitelně parametr, který bude vložen do nalezené hlášky místo {PropertyValue}.</param>
    /// <exception cref="NotImplementedException">ExceptionCode nebyl nalezen v Messages.</exception>
    public static string GetMessage(int exceptionCode, object? parameter = null)
    {
        if (!Messages.ContainsKey(exceptionCode))
        {
            throw new NotImplementedException($"ExceptionCode {exceptionCode} not found in ErrorCodeMapper");
        }

        return parameter == null ? Messages[exceptionCode] : Messages[exceptionCode].Replace("{PropertyValue}", parameter.ToString());
    }

    /// <summary>
    /// Vytvoří vyjímku typu NotFound s textem pro daný ExceptionCode.
    /// </summary>
    /// <param name="exceptionCode">Kód chybové hlášky.</param>
    /// <param name="parameter">Volitelně ID entity, která nebyla nalezena.</param>
    /// <returns>Instance vyjímky, která má být vyvolána.</returns>
    public static CisNotFoundException CreateNotFoundException(int exceptionCode, object? parameter = null)
    {
        return new CisNotFoundException(exceptionCode, GetMessage(exceptionCode, parameter));
    }

    public static CisArgumentException CreateArgumentException(int exceptionCode, object? parameter = null)
    {
        return new CisArgumentException(exceptionCode, GetMessage(exceptionCode, parameter));
    }

    public static CisAlreadyExistsException CreateAlreadyExistsException(int exceptionCode, object? parameter = null)
    {
        return new CisAlreadyExistsException(exceptionCode, GetMessage(exceptionCode, parameter));
    }

    /// <summary>
    /// Vytvoří vyjímku typu ValidationFound s textem pro daný ExceptionCode.
    /// </summary>
    /// <param name="exceptionCode">Kód chybové hlášky.</param>
    /// <param name="parameter">Volitelně parametr, který má být vložen na placeholder {PropertyValue}.</param>
    /// <returns>Instance vyjímky, která má být vyvolána.</returns>
    public static CisValidationException CreateValidationException(int exceptionCode, object? parameter = null)
    {
        return new CisValidationException(exceptionCode, GetMessage(exceptionCode, parameter));
    }

    public static CisConfigurationException CreateConfigurationException(int exceptionCode, object? parameter = null)
    {
        return new CisConfigurationException(exceptionCode, GetMessage(exceptionCode, parameter));
    }

    /// <summary>
    /// Inicializuje kolekci chybových hlášek Messages. Tato kolekce je private, lze ji tedy z child třídy nastavit jen touto metodou.
    /// </summary>
    /// <remarks>Volat ve statickém konstruktoru child třídy?</remarks>
    /// <param name="messages">[ExceptionCode, ExceptionMessage]</param>
    protected static void SetMessages(IDictionary<int, string> messages)
    {
        lock (_lock)
        {
            if (Messages is null)
            {
                Messages = new ErrorCodesDictionary(messages);
            }
            else if (messages.Any() && !Messages.ContainsKey(messages.Keys.First()))
            {
                Messages = new ErrorCodesDictionary(Messages.Concat(messages).ToDictionary(x => x.Key, x => x.Value));
            }
        }
    }

    /// <summary>
    /// Implementace IErrorCodesDictionary je private, aby nešla kolekce chybových hlášek vytvořit jinde, než v této base třídě.
    /// </summary>
    /// <remarks>Pokud tedy nebude někdo příliš kreativní a nevytvoří si vlastní implementaci...</remarks>
    private sealed class ErrorCodesDictionary
        : ReadOnlyDictionary<int, string>, IErrorCodesDictionary
    {
        public ErrorCodesDictionary(IDictionary<int, string> dictionary)
            : base(dictionary)
        {
        }
    }
}