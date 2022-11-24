using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;

namespace CIS.Infrastructure.WebApi.Validation;

/// <summary>
/// Validator ModelState-u, ktery ignoruje vychozi attribute-based modelState validace.
/// Pouzivam ho, kdyz mam FluentValidation a chci uplne ignorovat vychozi MVC chovani.
/// </summary>
/// <remarks>
/// builder.Services.AddSingleton<IObjectModelValidator, CIS.Infrastructure.WebApi.Validation.NullObjectModelValidator>();
/// </remarks>
public sealed class NullObjectModelValidator : IObjectModelValidator
{
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    public void Validate(ActionContext actionContext,
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        ValidationStateDictionary validationState, string prefix, object model)
    {

    }
}