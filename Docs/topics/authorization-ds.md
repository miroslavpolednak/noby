# Autorizace doménových služeb
Autorizace technických uživatelů funguje stejně jako standardní autorizace v ASP NET, tj. pomocí atributu `[Authorize]` na úrovni endpointu nebo gRPC service class.

Autorizace může fungovat dvojím způsobem:
a) autorizace pouze na login
b) autorizace na login a roli

## Implementace autorizace
Pro nastavení autorizace doménové služby je třeba zavolat tuto extension metodu při startupu:
```csharp
builder.AddCisServiceAuthentication()
```

Ukázka použití atributu `[Authorize]` pro zapnutí autorizace na úroveň služby nebo endpointu:
```csharp
// autorizace pro všechny endpointy služby
[Authorize]
internal sealed class CaseService : Contracts.v1.CaseService.CaseServiceBase
{
	// autorizace pro konkrétní endpoint
	[Authorize(Roles = "Read")]
	public override async Task<Empty> CompleteTask(CompleteTaskRequest request, ServerCallContext context)
		=> await _mediator.Send(request, context.CancellationToken);
}
```

## Konfigurace autorizace
Autorizace se nastavuje v *appsettings.json* v elementu **CisSecurity:ServiceAuthentication:AllowedUsers**, kde objekty v poli AllowedUsers má tuto strukturu:

```json
{
	// login technického uživatele
    "Username": "serviceUserUsername",
	// role přiřazené tomuto uživateli - element není povinný
    "Roles": [ "role1", ... ]
}
```

Kompletní konfigurace security pro doménovou službu tedy vypadá např. takto:

```json
"CisSecurity": {
	"ServiceAuthentication": {
		"Validator": "NativeActiveDirectory",
		"AllowedUsers": 
		[
			{
				"Username": "serviceUserUsername"
			},
			... // další technický uživatel
		]
	}
}
```