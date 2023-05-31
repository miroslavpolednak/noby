using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CodebookService.Clients.Services;

public class CodebookServiceMock 
    : CodebookServiceBaseMock, ICodebookServiceClient
{
    public override Task<List<DocumentTypesResponse.Types.DocumentTypeItem>> DocumentTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<Contracts.v1.DocumentTypesResponse.Types.DocumentTypeItem>
        {
            new() {Id = 1 , ShortName="NABIDKA" , SalesArrangementTypeId=null,EACodeMainId=605569, FileName="Nabidka_HU"},
            new() {Id = 2 , ShortName="KALKULHU", SalesArrangementTypeId=null,EACodeMainId=null  , FileName="Kalkulace_HU"},
            new() {Id = 3 , ShortName="SPLKALHU", SalesArrangementTypeId=null,EACodeMainId=null  , FileName="Splatkovy_kalendar"},
            new() {Id = 4 , ShortName="ZADOSTHU", SalesArrangementTypeId=null,EACodeMainId=608248, FileName="Zadost_HU1"},
            new() {Id = 5 , ShortName="ZADOSTHD", SalesArrangementTypeId=null,EACodeMainId=608243, FileName="Zadost_HD2"},
            new() {Id = 6 , ShortName="ZADOCERP", SalesArrangementTypeId=6   ,EACodeMainId=613226, FileName="Cerpani_HU"},
            new() {Id = 7 , ShortName="ZADOOPCI", SalesArrangementTypeId=null,EACodeMainId=608578, FileName="Zadost_Flexi"},
            new() {Id = 8 , ShortName="ZAOZMPAR", SalesArrangementTypeId=7   ,EACodeMainId=608279, FileName="Zadost_obecna"},
            new() {Id = 9 , ShortName="ZAOZMDLU", SalesArrangementTypeId=9   ,EACodeMainId=608580, FileName="Zadost_dluznik"},
            new() {Id = 10, ShortName="ZAODHUBN", SalesArrangementTypeId=8   ,EACodeMainId=608579, FileName="Zadost_bezNem"},
            new() {Id = 11, ShortName="ZUSTAVSI", SalesArrangementTypeId=12  ,EACodeMainId=608524, FileName="Zustavajici_v_HU"},
            new() {Id = 12, ShortName="PRISTOUP", SalesArrangementTypeId=10  ,EACodeMainId=608524, FileName="Pristupujici_k_HU"},
            new() {Id = 13, ShortName="DANRESID", SalesArrangementTypeId=null,EACodeMainId=616578, FileName="Prohlaseni_dan"},
            new() {Id = 14, ShortName="ZMENKLDA", SalesArrangementTypeId=null,EACodeMainId=616525, FileName="Klientdata_zmena"},
            new() {Id = 15, ShortName="ODSTOUP" , SalesArrangementTypeId=null,EACodeMainId=608522, FileName="Ukonceni_zadosti_HU"},
            new() {Id = 16, ShortName="ZADOSTHD", SalesArrangementTypeId=11  ,EACodeMainId=608243, FileName="Pridani_spoludluznika"}
            });
    }

    public override Task<List<SalesArrangementTypesResponse.Types.SalesArrangementTypeItem>> SalesArrangementTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<Contracts.v1.SalesArrangementTypesResponse.Types.SalesArrangementTypeItem>{
            new() {Id=1, Name="Žádost o hypotéční úvěr 20001",SalesArrangementCategory=1, ProductTypeId=20001, Description = null},
            new() {Id=2 ,Name="Žádost o hypoteční překlenovací úvěry",  SalesArrangementCategory=1,ProductTypeId=20002, Description = null},
            new() {Id=3 ,Name="Žádost o hypoteční úvěr bez příjmu",SalesArrangementCategory = 1, ProductTypeId = 20003, Description = null},
            new() {Id=4 ,Name="Žádost o doprodej neúčelové části",SalesArrangementCategory = 1, ProductTypeId = 20004, Description = null},
            new() {Id=5 ,Name="Žádost o americkou hypotéku",SalesArrangementCategory = 1, ProductTypeId = 20010, Description = null},
            new() {Id=6 ,Name="Žádost o čerpání", SalesArrangementCategory = 2, Description = null},
            new() {Id=7 ,Name="Žádost o změnu podmínek smlouvy o hypotečním úvěru", SalesArrangementCategory = 2, Description = null},
            new() {Id=8 ,Name="Žádost o změnu / doplnění podmínek smlouvy o hypotečním úvěru bez nemovitosti", SalesArrangementCategory = 2, Description = null},
            new() {Id=9 ,Name="Žádost o změnu dlužníků", SalesArrangementCategory = 2, Description = null},
            new() {Id=10,Name="Údaje o přistupujícím k dluhu", SalesArrangementCategory = 2, Description = null},
            new() {Id=11,Name="Žádost o přidání spoludlužníka", SalesArrangementCategory = 2, Description = null},
            new() {Id=12,Name="Údaje o zůstávajícím v dluhu", SalesArrangementCategory = 2, Description = null},
        });
    }

    public override Task<List<SigningMethodsForNaturalPersonResponse.Types.SigningMethodsForNaturalPersonItem>> SigningMethodsForNaturalPerson(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<Contracts.v1.SigningMethodsForNaturalPersonResponse.Types.SigningMethodsForNaturalPersonItem>
        {
            new() { Code = "OFFERED", Order = 4, Name = "Delegovaná metoda podpisu", Description = "deprecated", IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "PHYSICAL", Order = 1, Name = "Ruční podpis", Description = "Fyzický/ruční podpis dokumentu.", IsValid = true, StarbuildEnumId = 1 },
            new() { Code = "DELEGATE", Order = 1, Name = "Přímé bankovnictví", Description = "Přímé bankovnictví - Delegovaná metoda podpisu", IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "PAAT", Order = 1, Name = "KB klíč", Description = null, IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "INT_CERT_FILE", Order = 2, Name = "Interní certifikát v souboru", Description = null, IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "APOC", Order = 3, Name = "Automatizovaný Podpis Osobním Certifikátem", Description = null, IsValid = true, StarbuildEnumId = 2 },
        });
    }
}
