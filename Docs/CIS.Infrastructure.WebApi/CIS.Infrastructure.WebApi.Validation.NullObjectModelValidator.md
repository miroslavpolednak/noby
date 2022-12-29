#### [CIS.Infrastructure.WebApi](index.md 'index')
### [CIS.Infrastructure.WebApi.Validation](CIS.Infrastructure.WebApi.Validation.md 'CIS.Infrastructure.WebApi.Validation')

## NullObjectModelValidator Class

Validator ModelState-u, ktery ignoruje vychozi attribute-based modelState validace.  
Pouzivam ho, kdyz mam FluentValidation a chci uplne ignorovat vychozi MVC chovani.

```csharp
public sealed class NullObjectModelValidator :
Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; NullObjectModelValidator

Implements [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator 'Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator')

### Remarks
builder.Services.AddSingleton<IObjectModelValidator, CIS.Infrastructure.WebApi.Validation.NullObjectModelValidator>();