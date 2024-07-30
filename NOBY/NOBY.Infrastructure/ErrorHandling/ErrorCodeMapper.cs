namespace NOBY.Infrastructure.ErrorHandling;

public sealed class ErrorCodeMapper
{
    public const int DefaultExceptionCode = 90001;

    public static IReadOnlyDictionary<int, ErrorCodeMapperItem> Messages { get; private set; } = null!;
    public static IReadOnlyDictionary<int, ErrorCodeDsToApiItem> DsToApiCodeMapper { get; private set; } = null!;

    public static void Init()
    {
        var messages = new Dictionary<int, ErrorCodeMapperItem>()
        {
            { 90001, new("Nastala neočekávaná chyba, opakujte akci později prosím.") },
            { 90002, new("MP produkty nejsou podporovány.") },
            { 90003, new("Dokument nelze manuálně podepsat.") },
            { 90005, new("Chyba - tento subjekt nejde zvolit", "V rámci žádosti musí být subjekt vždy unikátní a není možné jej přiřadit vícekrát. Zvolte prosím jiný subjekt.") },
            { 90006, new("Chyba - bylo nalezeno více klientů dle zadaných kritérií.", "V žádosti není možné pokračovat, před jejich úpravou v KB. Požádejte prosím Klienta, aby se obrátil na pobočku KB, kde bude provedena jednoznačná identifikace.") },
            { 90007, new("Subjekt se nepodařilo ztotožnit v Základních registrech, nebo dalších návazných systémech.", "Zadaným kritériím neodpovídá žádný záznam, ale můžete změnit zadané údaje, nebo pokračovat v zadávání subjektu na detailní obrazovce.", ApiErrorItemServerity.Info) },
            { 90008, new("Chyba - služba na ztotožnění subjektu v Základních registrech není dostupná.", "V žádosti je ovšem možné pokračovat v zadávání subjektu na detailní obrazovce.") },
            { 90009, new("Checkform odhalil chyby.") },
            { 90010, new("Chyba synchronizace dat.", "Klientská data na obchodním případu se zobrazí až po zpracování žádosti ve StarBuildu.", ApiErrorItemServerity.Warning) },
            { 90011, new("Zůstatek pro čerpání je menší nebo rovný nule. Žádost nelze vytvořit") },
            { 90012, new("Aktuální datum překračuje datum první anuitní splátky. Žádost nelze vytvořit") },
            { 90013, new("Neexistuje úvěrový účet. Žádost nelze vytvořit") },
            { 90014, new("Smlouva není podepsána. Žádost nelze vytvořit") },
            { 90015, new("Není vyplněna žádost o změnu Dlužníka. Nelze vytvořit žádost změnu.") },
            { 90016, new("Nabídka nemá platnou garanci.") },
            { 90017, new("Nelze simulovat s garancí.", "Žádost neexistuje nebo neobsahuje platné datum garance.") },
            { 90018, new("Nelze pokračovat bez schválené individuální ceny.") },
            { 90019, new("Obchod nelze poskytnout.","Převažující měna příjmu nebo měna bydliště není v oboru povolených měn. V žádosti nebude možné dále pokračovat.") },
            { 90020, new("Chyba - služba pro našeptávání adres není aktuálně dostupná.", "Zadejte adresu ručně.") },
            { 90021, new("Chyba - nastala chyba při předání dat do systému CURE.", "Proto jsme automaticky odeslali úkol typu obecná Konzultace do Zpracovatelského centra na manuální úpravu a doplnění těchto údajů. Do doby zpracování úkolu ve Zpracovatelském centru, budou uloženy neaktuální informace o klientovi.") },
            { 90022, new("Nelze stáhnout dokument ze systému ePodpisy", "Dokument již není k nalezení v systému ePodpisy (pravděpodobně byl smazán).") },
            { 90023, new("Chyba propisu dat o novém klientovi", "Prosím vyčkejte, než bude záznam o novém klientovi vytvořen v databázi C4M. Za okamžik opakujte akci znovu") },
            { 90024, new("Chyba - pro klienta již existuje rozpracovaný obchodní případ", "Klient je účastníkem jiného obchodního případu čekajícího na schválení nebo zamítnutí. Pro více informací prosím kontaktujte tým zpracovatelů") },
            { 90025, new("Obchodní případ byl stornován.", "") },
            { 90026, new("Vámi hledaný obchodní případ byl vytvořen v jiném systému a není možné zde zobrazit jeho detail") },
            { 90027, new("Nemovitost není vhodná pro online ocenění", "Pro danou nemovitost není možné dokončit Online ocenění. Je potřeba zvolit jiný typ ocenění.") },
            { 90028, new("Žádost v aktuální stavu nelze měnit.") },
            { 90029, new("Překročen počet objektů úvěru", "Maximální počet objektů úvěru je 3. Upravte počet objednávek ocenění s označením objekt úvěru.") },
            { 90030, new("Chyba - soubor s tímto typem dokumentu není povoleno vložit do eArchivu") },
            { 90031, new("Chybí nemovitost k ocenění", "Je potřeba doplnit LV s alespoň jednou nemovitostí označenou k ocenění.") },
            { 90032, new("Nepovolená operace", "Vámi požadovaná operace není se zadanými parametry povolena.") },
            { 90033, new("Žadatel nenalezen na daném obchodním případu.") },
            { 90034, new("<Chyba simulace z DS>") },
            { 90035, new("Nepodařilo se stáhnout LV z katastru nemovitostí", "Je nám líto, ale v tuto chvíli se nedaří stáhnout požadovaný LV z katastru nemovitostí. Opakujte prosím akci později. Pokud se operace nezdaří do tří pracovních dnů, kontaktujte prosím zpracovatele.") },
            { 90036, new("Nelze stornovat podpis u CRS žádosti po prohlášení za podepsanou.") },
            { 90037, new("Nahrávaný soubor je závadný", "Nahrávaný soubor je poškozen či může být závadný. Nahrání se nezdařilo.") },
            { 90038, new("Název vkládaného dokumentu je větší než povolených 64 znaků.") },
            { 90039, new("Klientské údaje nejsou validní", "Zkontrolujte klientské údaje všech žadatelů. Validace klientských údajů spustíte tlačítkem DALŠÍ na obrazovce Detailu subjektu.") },
            { 90040, new("Existuje rozpracovaná žádost o úvěr. Nelze vytvořit žádost o změnu.") },
            { 90041, new("Smlouva je již podepsána. Nelze vytvořit tento typ změnové žádosti.") },
            { 90042, new("Nelze pokračovat, protože došlo ke změně daňové rezidence a klient má zadáno více jak 8 států daňové rezidence. Tuto změnu je možné provést pouze na pobočce.") },
            { 90043, new("Nenalezeno") },
            { 90044, new("Nepovolená hodnota pro Typ dokladu v rámci vybraného Státu vydání dokladu") },
            { 90045, new("Chyba migrace dat", "Nepodařilo se dozaložit obchodní případ z důvodu chyby v datech zdrojového systému") },
            { 90046, new("Nelze založit úkol", "Z důvodu aktuální nedostupnosti systému StarBuild nelze založit nový úkol. Opakujte prosím akci později.") },
            { 90047, new("LV nenalezeno", "Dle zadaných kritérií nebylo nalezeno žádné LV.") },
            { 90048, new("Sleva neodpovídá", "Výše slevy požadovaná v nabídce je odlišná od slevy zadané v úkolu Cenová výjimka. Zkontrolujte prosím kalkulaci nabídky a upravte výši slevy.") },
            { 90049, new("Sleva není schválena", "V nabídce je požadována sleva, pro kterou není schválený úkol Cenová výjimka.") },
            { 90050, new("Sleva zřejmě expirovala", "Úkol Cenové výjimky pro slevu požadovanou v nabídce je zrušený zřejmě z důvodu expirace platnosti.") },
            { 90051, new("Platnost budoucí úrokové sazby v minulosti", "Požadované datum platnosti budoucí úrokové sazby je v minulosti.") },
            { 90052, new("Není povolen souběh požadavků stejného typu. Zkontrolujte rozpracované požadavky.", "Není možné vytvořit nový požadavek, jelikož již existuje rozpracovaný požadavek z dřívějška.") },
            { 90053, new("Nelze podepsat elektronicky, protože je podepisováno na základě plné moci.") },
            { 90054, new("Soubor nelze stáhnout. Soubor neexistuje nebo je stále v procesu nahrávání do eArchivu, opakujte akci později.") },
            { 90055, new("Neaktuální parametry mimořádné splátky")},
            { 90056, new("Retence jsou zakázány") },
            { 90057, new("Refixace jsou zakázány") },
            { 90058, new("Mimořádná splátka je zakázaná") },
            { 90059, new("Interní refinancování je zakázáno") },
            { 90060, new("Sleva na úrokové sazbě je vyšší než úroková sazba.") },
            { 90061, new("Nestandardní přístup do kalkulace bez kontextu žádosti", "Vstupujete do kalkulace nestandardním způsobem a bez navázaného kontextu žádosti. Vraťte se na Rozcestník a vstupte standardním způsobem.") },
            { 90062, new("Nelze generovat dokument - blíží se termín refixace", "Není povoleno generovat dokument ve lhůtě 14 dní do nadcházející refixace.") },
            { 90064, new("Vložte alespoň jeden soubor") },
            { 90065, new("Vložte povinnou přílohu, tj. 2x fotografie nemovitosti (1x interiér + 1x exteriér) a doklad o výměře bytové jednotky") },
            { 90066, new("Výše mimořádné splátky je větší než zbývající jistina, vyberte celkové splacení nebo upravte částku.") },
            { 90067, new("Modré produkty jsou zakázány.") },
            { 90068, new("Nelze vygenerovat dodatek, protože hlavní dlužník či spoludlužníci nemají vyplněn jeden nebo více kontaktů. Doplňte klientům email resp. telefon (hlavní telefon) přes aplikaci CURE, počkete na synchronizaci kontaktů do systému Starbuild a generování dodatku opakujte.") },
            { 90069, new("--DS error--") },
            { 90070, new("Stav rozpracované žádosti se změnil na pozadí, vraťte se detail obchodního případu a pokračujte standardním průchodem přes aplikaci.") }
        };

        Messages = messages.AsReadOnly();

        var mapper = new Dictionary<int, ErrorCodeDsToApiItem>()
        {
            { 13035, new ErrorCodeDsToApiItem(90025, false) },
            { 22202, new ErrorCodeDsToApiItem(90027, false) },
            { 22015, new ErrorCodeDsToApiItem(90029, false) },
            { 22016, new ErrorCodeDsToApiItem(90031, false) },
            { 17103, new ErrorCodeDsToApiItem(90023, false) },
            { 17102, new ErrorCodeDsToApiItem(90024, false) },
            { 10020, new ErrorCodeDsToApiItem(90034, true)  },
            { 18087, new ErrorCodeDsToApiItem(90039, false) },
            { 19043, new ErrorCodeDsToApiItem(90021, false) },
            { 16082, new ErrorCodeDsToApiItem(90069, true) },
        };

        DsToApiCodeMapper = mapper.AsReadOnly();
    }

    public sealed record ErrorCodeDsToApiItem(int FeApiCode, bool PropagateDsError)
    { }

    public sealed record ErrorCodeMapperItem(
        string Message,
        string? Description = null,
        ApiErrorItemServerity Severity = ApiErrorItemServerity.Error)
    { }
}