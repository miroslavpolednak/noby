using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CodebookService.Clients.Services;

public class CodebookServiceMock 
    : CodebookServiceBaseMock, ICodebookServiceClient
{
    public override Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> CaseStates(CancellationToken cancellationToken = default)
        => Helpers.GetGenericItems<CIS.Foms.Enums.CaseStates>();

    public override Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> AcademicDegreesBefore(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<GenericCodebookResponse.Types.GenericCodebookItem>
        {
            new() { Id = 0, Name = "Neuvedeno", IsValid = true }
        });
    }

    public override Task<List<CountriesResponse.Types.CountryItem>> Countries(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<CountriesResponse.Types.CountryItem>
        {
            new() { Id = 16, ShortName = "CZ", LongName = "Česká republika", IsDefault = true }
        });
    }

    public override Task<List<DocumentTypesResponse.Types.DocumentTypeItem>> DocumentTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<DocumentTypesResponse.Types.DocumentTypeItem>
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

    public override Task<List<DocumentTemplateVersionsResponse.Types.DocumentTemplateVersionItem>> DocumentTemplateVersions(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<DocumentTemplateVersionsResponse.Types.DocumentTemplateVersionItem>
        {
            new() { Id = 1, DocumentTypeId = 1, DocumentVersion = "001", IsValid = true },
            new() { Id = 2, DocumentTypeId = 2, DocumentVersion = "001", IsValid = true },
            new() { Id = 3, DocumentTypeId = 3, DocumentVersion = "001", IsValid = true },
            new() { Id = 4, DocumentTypeId = 4, DocumentVersion = "001", IsValid = true },
            new() { Id = 5, DocumentTypeId = 5, DocumentVersion = "001", IsValid = true },
            new() { Id = 6, DocumentTypeId = 6, DocumentVersion = "001", IsValid = true },
        });
    }

    public override Task<List<DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem>> DocumentTemplateVariants(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem>
        {
            new() { Id = 1, DocumentTemplateVersionId = 4, DocumentVariant = "A" },
            new() { Id = 2, DocumentTemplateVersionId = 4, DocumentVariant = "B" },
            new() { Id = 3, DocumentTemplateVersionId = 4, DocumentVariant = "C" },
            new() { Id = 4, DocumentTemplateVersionId = 4, DocumentVariant = "D" },
        });
    }

    public override Task<List<DrawingTypesResponse.Types.DrawingTypeItem>> DrawingTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<DrawingTypesResponse.Types.DrawingTypeItem>
        {
            new() { Id = 1, Name = "Postupné", StarbuildId = 1 },
            new() { Id = 2, Name = "Jednorázové", StarbuildId = 0 }
        });
    }

    public override Task<List<DrawingDurationsResponse.Types.DrawingDurationItem>> DrawingDurations(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<DrawingDurationsResponse.Types.DrawingDurationItem>
        {
            new() { Id = 1, DrawingDuration = 12, IsValid = true }
        });
    }

    public override Task<List<EducationLevelsResponse.Types.EducationLevelItem>> EducationLevels(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<EducationLevelsResponse.Types.EducationLevelItem>
        {
            new() { Id = 0, Name = "Neuvedeno", IsValid = true }
        });
    }

    public override Task<List<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem>> IdentificationDocumentTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem>
        {
            new() { Id = 0, Name = "Nedefinovaný", ShortName = "ND" },
            new() { Id = 1, Name = "Občabský průkaz", ShortName = "OP" }
        });
    }

    public override Task<List<SalesArrangementTypesResponse.Types.SalesArrangementTypeItem>> SalesArrangementTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<Contracts.v1.SalesArrangementTypesResponse.Types.SalesArrangementTypeItem>{
            new() {Id=1, Name="Žádost o hypotéční úvěr 20001",SalesArrangementCategory=1, Description = null},
            new() {Id=2 ,Name="Žádost o hypoteční překlenovací úvěry",  SalesArrangementCategory=1, Description = null},
            new() {Id=3 ,Name="Žádost o hypoteční úvěr bez příjmu",SalesArrangementCategory = 1, Description = null},
            new() {Id=4 ,Name="Žádost o doprodej neúčelové části",SalesArrangementCategory = 1, Description = null},
            new() {Id=5 ,Name="Žádost o americkou hypotéku",SalesArrangementCategory = 1, Description = null},
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

    public override Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> SignatureTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<GenericCodebookResponse.Types.GenericCodebookItem>
        {
            new() { Id = 1, Name = "Papírově", Code = "Paper", IsValid = true },
            new() { Id = 2, Name = "Biometricky", Code = "Biometric", IsValid = true },
            new() { Id = 3, Name = "Elektronicky", Code = "Eletronic", IsValid = true },

        });
    }

    public override Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> LoanKinds(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<GenericCodebookResponse.Types.GenericCodebookItem>
        {
            new() { Id = 2001, MandantId = 2, Name = "Hypotéka bez nemovitosti", IsValid = true },
            new() { Id = 2000, MandantId = 2, Name = "Standard", IsValid = true }
        });
    }

    public override Task<List<LoanPurposesResponse.Types.LoanPurposeItem>> LoanPurposes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<LoanPurposesResponse.Types.LoanPurposeItem>
        {
            new() { Id = 210, Name = "Neúčelové", IsValid = true, C4MId = 20, Order = 10}
        });
    }

    public override Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> MaritalStatuses(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<GenericCodebookResponse.Types.GenericCodebookItem>
        {
            new() { Id = 0, Name = "Neuvedeno", IsValid = true }
        });
    }

    public override Task<List<ProductTypesResponse.Types.ProductTypeItem>> ProductTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<ProductTypesResponse.Types.ProductTypeItem>
        {
            new() { Id = 20001, Name = "Hypoteční úvěr", IsValid = true }
        });
    }

    public override Task<List<PropertySettlementsResponse.Types.PropertySettlementItem>> PropertySettlements(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<PropertySettlementsResponse.Types.PropertySettlementItem>
        {
            new() { Id = 0, Name = "Netýká se", IsValid = true }
        });
    }

    public override Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> RealEstateTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<GenericCodebookResponse.Types.GenericCodebookItem>
        {
            new() { Id = 0, Name = "Unknown", IsValid = true }
        });
    }

    public override Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> RealEstatePurchaseTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<GenericCodebookResponse.Types.GenericCodebookItem>
        {
            new() { Id = 0, Name = "Unknown", IsValid = true }
        });
    }

    public override Task<List<GendersResponse.Types.GenderItem>> Genders(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<GendersResponse.Types.GenderItem>
        {
            new() { Id = 0, Name = "Unknown" }
        });
    }

    public override Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> EmploymentTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<GenericCodebookResponse.Types.GenericCodebookItem>
        {
            new() { Id = 0, Name = "Unknown" }
        });
    }

    public override Task<List<ObligationTypesResponse.Types.ObligationTypeItem>> ObligationTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<ObligationTypesResponse.Types.ObligationTypeItem>
        {
            new() { Id = 0, Name = "Unknown" }
        });
    }

    public override Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> LegalCapacityRestrictionTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<GenericCodebookResponse.Types.GenericCodebookItem>
        {
            new() { Id = 0, Name = "Unknown" }
        });
    }

    public override Task<List<SalesArrangementStatesResponse.Types.SalesArrangementStateItem>> SalesArrangementStates(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<SalesArrangementStatesResponse.Types.SalesArrangementStateItem>
        {
            new() { Id = 0, Name = "Unknown" }
        });
    }
}
