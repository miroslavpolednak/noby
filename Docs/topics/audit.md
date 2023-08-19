# Auditní logování
Auditní logování je nezávislé na standardním logu - nepoužívá se pro něj *Serilog* a má vlastní DI komponenty.
Události auditního logu se ukládají do vlastní databáze a z ní jsou pak přenášeny do auditního logu KB.
Podpora pro auditní logování je v projektu **CIS.Infrastructure.Audit**.

Typy auditních událostí včetně popisu jaké údaje se pro každou událost mají sbírat jsou popsané na [Confluence stránce auditního logování](https://wiki.kb.cz/pages/viewpage.action?pageId=542682986).
Každá auditní událost má vlastní ID (*NOBY_001, NOBY_002...*) a musí být zastoupena vlastní položkou v enumu `CIS.Infrastructure.Audit.AuditEventTypes`.

Přidání auditního logu do aplikace během startupu:
```csharp
using CIS.Infrastructure.Audit;
...
builder.AddCisAudit();
```

Zápis auditní události:
```csharp
private readonly IAuditLogger _auditLogger;
public MyClass(IAuditLogger auditLogger) {
	_auditLogger = auditLogger;
}
...
public void MyMethod() {
	_auditLogger.Log(AuditEventTypes.Noby004, "Případ byl stornován");

	// nebo s automatickým přidáním aktuálně přihlášeného uživatele do kolekce identities
	_auditLogger.LogWithCurrentUser(AuditEventTypes.Noby004, "Případ byl stornován");
}
```

## Postup pro přidání nové auditní události
1) Confluence - přidání auditní události do [seznamu](https://wiki.kb.cz/pages/viewpage.action?pageId=645510406)
2) Confluence - přidání sbíraných údajů do [seznamu](https://wiki.kb.cz/pages/viewpage.action?pageId=645510420)
3) přidání události do enumu `CIS.Infrastructure.Audit.AuditEventTypes`
4) registrace auditní události v KB

