# Psaní technické dokumentace
Technická dokumentace - tj. dokumentace zdrojového kódu, patternů použitých při vývoji atd. - je psána ve formátu [Markdown](https://www.markdownguide.org/basic-syntax/) v samostatných *.md souborech.

Celá dokumentace je uložena v samostatném adresáři **/Docs**. Struktura adresáře je následující:

```
index.md				Homepage dokumentace
[topics]				Adresář s *.md soubory s ručně psanou dokumentací.
	*.md
[CIS.Core]				Adresář s automaticky vygenerovanou dokumentací pro CIS.Core projekt.
	index.md
	*.md
[CIS.Infrastructure.WebApi]		Adresář s automaticky vygenerovanou dokumentací pro CIS.Infrastructure.WebApi projekt.
	index.md
	*.md
[...]					Další adresáře s automaticky generovanou dokumentací, pojmenované podle projektu, který danou dokumentaci obsahuje.
```

## Automaticky generovaná dokumentace
Podkladem pro automaticky generovanou dokumentaci jsou [standardní XML komentáře](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags) v *.cs souborech. 
Je tedy dobré/nutné veřejně dostupné objekty, metody a třídy kvalitně popisovat již v kódu.  

Dokumentace je generována pomocí projektu [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation) dotnet toolem. 
Generování dokumentace se pustí souborem **generate_docs.bat** v rootu solution.
Nepoužíváme pro generování *BuildTask*, který je také k dispozici, protože potom padá CI/CD v Devops.
Dotnet tool je nutné mít nainstalovaný na PC:
```
dotnet tool install DefaultDocumentation.Console -g
```

**generate_docs.bat** generuje .md soubory pro všechny projekty zapojené v dokumentaci.
Pokud je potřeba přegenerovat pouze konkrétní projekt, stačí spustit dotnet tool pro tento projekt:
```
defaultdocumentation -j "./CIS/Core/DefaultDocumentation.json"
```

Aby generování dokumentace fungovalo správně, je potřeba:
- přidat do projektu soubor `DefaultDocumentation.json`
- přidat do projektu soubor `AssemblyDoc.cs`, který obsahuje popisy assembly a namespace-ů.
- vytvořit v **Docs** adresář se jménem dokumentovaného projektu.

Obsah souboru *DefaultDocumentation.json*:
```
{
  "LogLevel": "Warn",
  "AssemblyFilePath": "./bin/Debug/net7.0/CIS.Core.dll",
  "ProjectDirectoryPath": "./",
  "OutputDirectoryPath": "../../Docs/CIS.Core/",
  "GeneratedPages": "Namespaces,Classes,Interfaces",
  "GeneratedAccessModifiers": "Public"
}
```
Klíče `AssemblyFilePath` a `OutputDirectoryPath` se nastavují dle názvu aktuálního projektu.

## Ručně psaná dokumentace
Jedná se o popisy používaných patternů, dokumentaci a způsob použití jednotlivých infrastrukturních projektů atd. 
Všechny témata pro ručně psanou dokumentaci ukládáme jako .md soubory do adresáře /Docs/topics.
Zároveň je potřeba prolinkovat nově vytvořený soubor z hlavní stránky dokumentace (/Docs/index.md), kde musí v části "Témata" vzniknout nový hypertextový odkaz.

Nové soubory s dokumentací je kromě toho také potřeba přidat do solution tak, aby byly zobrazené i ve Visual Studiu.


