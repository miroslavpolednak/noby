using System.Diagnostics;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CIS.InternalServices.NotificationService.Api.ErrorHandling;

public class CustomValidationResponse : ValidationProblemDetails
{
    public CustomValidationResponse(ActionContext context)
    {
        Title = "One or more validation errors occurred.";
        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        Status = 400;
        Extensions["traceId"] = Activity.Current?.Id;
        ConstructErrorMessages(context);
    }
    
    private void ConstructErrorMessages(ActionContext context)
    {
        foreach (var (key, entry) in context.ModelState)
        {
            var errors = entry.Errors;
            var errorMessages = errors.Select(error => GetErrorMessage(entry, error)).ToArray();
            Errors.Add(key, errorMessages);
        }
    }
    private static string GetErrorMessage(ModelStateEntry entry, ModelError error)
    {
        var attempted = entry.AttemptedValue ?? string.Empty;
        return error.ErrorMessage.Replace($"'{attempted}'",$"'{UrlEncoder.Default.Encode(attempted)}'");
    }
    
}