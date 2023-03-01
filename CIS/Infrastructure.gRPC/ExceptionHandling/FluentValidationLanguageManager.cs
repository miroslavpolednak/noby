namespace CIS.Infrastructure.gRPC.ExceptionHandling;

internal sealed class FluentValidationLanguageManager
    : FluentValidation.Resources.LanguageManager
{
    public const string DefaultLanguage = "en";

    public FluentValidationLanguageManager(CIS.Core.ErrorCodes.IErrorCodesDictionary messages)
    {
        foreach (var message in messages)
        {
            AddTranslation(DefaultLanguage, message.Key.ToString(System.Globalization.CultureInfo.InvariantCulture), message.Value);
        }
    }
}
