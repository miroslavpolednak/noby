namespace NOBY.Infrastructure.ErrorHandling;

public sealed class ErrorCodeMapper
{
    public static IReadOnlyDictionary<int, ErrorCodeMapperItem> Messages { get; private set; } = null!;
    public static IReadOnlyDictionary<int, int> DsToApiCodeMapper { get; private set; } = null!;

    public static void Init()
    {
        var messages = new Dictionary<int, ErrorCodeMapperItem>()
        {
            { 90001, new("Nastala neočekávaná chyba, opakujte akci později prosím.") },
            { 90002, new("MP produkty nejsou podporovány.") },
            { 90003, new("Dokument nelze manuálně podepsat.") },
            { 90005, new("Chyba - tento subjekt nejde zvolit", "V rámci žádosti musí být subjekt vždy unikátní a není možné jej přiřadit vícekrát. Zvolte prosím jiný subjekt.") },
            { 90006, new("Chyba - bylo nalezeno více klientů dle zadaných kritérií.", "V žádosti není možné pokračovat, před jejich úpravou v KB. Požádejte prosím Klienta, aby se obrátil na pobočku KB, kde bude provedena jednoznačná identifikace.") },
            { 90007, new("Subjekt se nepodařilo ztotožnit v Základních registrech, nebo dalších návazných systémech.", "Zadaným kritériím neodpovídá žádný záznam, ale můžete změnit zadané údaje, nebo pokračovat v zadávání subjektu na detailní obrazovce.", ApiErrorItemServerity.Warning) },
            { 90008, new("Chyba - služba na ztotožnění subjektu v Základních registrech není dostupná.", "V žádosti je ovšem možné pokračovat v zadávání subjektu na detailní obrazovce.") },
            { 90009, new("Checkform odhalil chyby.") },
            { 90010, new("Chyba synchronizace dat.", "Klientská data na obchodním případu se zobrazí až po zpracování žádosti ve StarBuildu.", ApiErrorItemServerity.Warning) },
            { 90011, new("Zůstatek pro čerpání je menší nebo rovný nule. Formulář nelze vytvořit") },
            { 90012, new("Aktuální datum překračuje datum první anuitní splátky. Formulář nelze vytvořit") },
            { 90013, new("Neexistuje úvěrový účet. Formulář nelze vytvořit") },
            { 90014, new("Smlouva není podepsána. Formulář pro změnu nelze vytvořit.") },
            { 90015, new("Není vyplněna žádost o změnu Dlužníka. Nelze vytvořit změnu.") },
            { 90016, new("Nabídka nemá platnou garanci.") },
            { 90017, new("Nelze simulovat s garancí.", "Žádost neexistuje nebo neobsahuje platné datum garance.") },
            { 90018, new("Nelze pokračovat bez schválené individuální ceny.") },
            { 90019, new("Obchod nelze poskytnout.","Převažující měna příjmu nebo měna bydliště není v oboru povolených měn. V žádosti nebude možné dále pokračovat.") },
            { 90020, new("Chyba - služba pro našeptávání adres není aktuálně dostupná.", "Zadejte adresu ručně.") },
            { 90021, new("Chyba - nastala chyba při předání dat do systému CURE.", "Proto jsme automaticky odeslali úkol typu obecná Konzultace do Zpracovatelského centra na manuální úpravu a doplnění těchto údajů. Do doby zpracování úkolu ve Zpracovatelském centru, budou uloženy neaktuální informace o klientovi.") },
            { 90022, new("Nelze stáhnout dokument ze systému ePodpisy", "Dokument již není k nalezení v systému ePodpisy (pravděpodobně byl smazán).") },
            { 90023, new("Chyba propisu dat o novém klientovi", "Prosím vyčkejte, než bude záznam o novém klientovi vytvořen v databázi C4M. Za okamžik opakujte akci znovu") },
            { 90024, new("Chyba - pro klienta již existuje rozpracovaný obchodní případ", "Klient je účastníkem jiného obchodního případu čekajícího na schválení nebo zamítnutí. Pro více informací prosím kontaktujte tým zpracovatelů") },
            { 90025, new("Obchodní případ byl stornován.", "") }
        };

        Messages = messages.AsReadOnly();

        var mapper = new Dictionary<int, int>()
        {
            { 13035, 90025 }
        };

        DsToApiCodeMapper = mapper.AsReadOnly();
    }

    public sealed record ErrorCodeMapperItem(
        string Message,
        string? Description = null,
        ApiErrorItemServerity Severity = ApiErrorItemServerity.Error)
    { }
}