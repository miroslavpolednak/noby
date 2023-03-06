# Validace requestů REST služeb / FE API
FluentValidation se do *MVC* pipeline vloží standardním způsobem během startupu extension metodou:

```csharp
builder.Services.AddControllers().AddFluentValidation();
```

Narozdíl od gRPC implementace se validuje request přicházející přímo do endpointu, nikoliv *MediatR* request.
