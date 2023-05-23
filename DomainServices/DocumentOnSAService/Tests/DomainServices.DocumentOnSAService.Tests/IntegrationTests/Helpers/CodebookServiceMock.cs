using CIS.Foms.Types.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints;
using DomainServices.CodebookService.Contracts.Endpoints.BankCodes;
using DomainServices.CodebookService.Contracts.Endpoints.CaseStates;
using DomainServices.CodebookService.Contracts.Endpoints.Channels;
using DomainServices.CodebookService.Contracts.Endpoints.CollateralTypes;
using DomainServices.CodebookService.Contracts.Endpoints.ContactTypes;
using DomainServices.CodebookService.Contracts.Endpoints.Countries;
using DomainServices.CodebookService.Contracts.Endpoints.CountryCodePhoneIdc;
using DomainServices.CodebookService.Contracts.Endpoints.Currencies;
using DomainServices.CodebookService.Contracts.Endpoints.CustomerProfiles;
using DomainServices.CodebookService.Contracts.Endpoints.CustomerRoles;
using DomainServices.CodebookService.Contracts.Endpoints.Developers;
using DomainServices.CodebookService.Contracts.Endpoints.DeveloperSearch;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentFileTypes;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentOnSATypes;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateTypes;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVariants;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVersions;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTypes;
using DomainServices.CodebookService.Contracts.Endpoints.DrawingDurations;
using DomainServices.CodebookService.Contracts.Endpoints.DrawingTypes;
using DomainServices.CodebookService.Contracts.Endpoints.EaCodesMain;
using DomainServices.CodebookService.Contracts.Endpoints.EducationLevels;
using DomainServices.CodebookService.Contracts.Endpoints.Fees;
using DomainServices.CodebookService.Contracts.Endpoints.FixedRatePeriods;
using DomainServices.CodebookService.Contracts.Endpoints.FormTypes;
using DomainServices.CodebookService.Contracts.Endpoints.Genders;
using DomainServices.CodebookService.Contracts.Endpoints.GetDeveloper;
using DomainServices.CodebookService.Contracts.Endpoints.GetDeveloperProject;
using DomainServices.CodebookService.Contracts.Endpoints.GetGeneralDocumentList;
using DomainServices.CodebookService.Contracts.Endpoints.GetOperator;
using DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;
using DomainServices.CodebookService.Contracts.Endpoints.HousingConditions;
using DomainServices.CodebookService.Contracts.Endpoints.IdentificationDocumentTypes;
using DomainServices.CodebookService.Contracts.Endpoints.IdentitySchemes;
using DomainServices.CodebookService.Contracts.Endpoints.JobTypes;
using DomainServices.CodebookService.Contracts.Endpoints.LegalCapacityRestrictionTypes;
using DomainServices.CodebookService.Contracts.Endpoints.LoanInterestRateAnnouncedTypes;
using DomainServices.CodebookService.Contracts.Endpoints.LoanKinds;
using DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes;
using DomainServices.CodebookService.Contracts.Endpoints.Mandants;
using DomainServices.CodebookService.Contracts.Endpoints.MaritalStatuses;
using DomainServices.CodebookService.Contracts.Endpoints.MarketingActions;
using DomainServices.CodebookService.Contracts.Endpoints.NetMonthEarnings;
using DomainServices.CodebookService.Contracts.Endpoints.ObligationLaExposures;
using DomainServices.CodebookService.Contracts.Endpoints.ObligationTypes;
using DomainServices.CodebookService.Contracts.Endpoints.PaymentDays;
using DomainServices.CodebookService.Contracts.Endpoints.PayoutTypes;
using DomainServices.CodebookService.Contracts.Endpoints.PostCodes;
using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;
using DomainServices.CodebookService.Contracts.Endpoints.ProfessionCategories;
using DomainServices.CodebookService.Contracts.Endpoints.ProfessionTypes;
using DomainServices.CodebookService.Contracts.Endpoints.ProofTypes;
using DomainServices.CodebookService.Contracts.Endpoints.PropertySettlements;
using DomainServices.CodebookService.Contracts.Endpoints.RealEstatePurchaseTypes;
using DomainServices.CodebookService.Contracts.Endpoints.RealEstateTypes;
using DomainServices.CodebookService.Contracts.Endpoints.RelationshipCustomerProductTypes;
using DomainServices.CodebookService.Contracts.Endpoints.RepaymentScheduleTypes;
using DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;
using DomainServices.CodebookService.Contracts.Endpoints.SignatureTypes;
using DomainServices.CodebookService.Contracts.Endpoints.SigningMethodsForNaturalPerson;
using DomainServices.CodebookService.Contracts.Endpoints.SmsNotificationTypes;
using DomainServices.CodebookService.Contracts.Endpoints.StatementFrequencies;
using DomainServices.CodebookService.Contracts.Endpoints.StatementTypes;
using DomainServices.CodebookService.Contracts.Endpoints.TinFormatsByCountry;
using DomainServices.CodebookService.Contracts.Endpoints.TinNoFillReasonsByCountry;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowConsultationMatrix;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStates;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStatesNoby;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskTypes;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;

public class CodebookServiceMock : ICodebookServiceClients
{
    #region UsedByDocumentOnSa

    public Task<List<DocumentTypeItem>> DocumentTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<DocumentTypeItem>
        {
            new DocumentTypeItem{Id = 1 , ShortName="NABIDKA" , SalesArrangementTypeId=null,EACodeMainId=605569,IsFormIdRequested=false,FileName="Nabidka_HU"},
            new DocumentTypeItem{Id = 2 , ShortName="KALKULHU", SalesArrangementTypeId=null,EACodeMainId=null  ,IsFormIdRequested=false,FileName="Kalkulace_HU"},
            new DocumentTypeItem{Id = 3 , ShortName="SPLKALHU", SalesArrangementTypeId=null,EACodeMainId=null  ,IsFormIdRequested=false,FileName="Splatkovy_kalendar"},
            new DocumentTypeItem{Id = 4 , ShortName="ZADOSTHU", SalesArrangementTypeId=null,EACodeMainId=608248,IsFormIdRequested=true ,FileName="Zadost_HU1"},
            new DocumentTypeItem{Id = 5 , ShortName="ZADOSTHD", SalesArrangementTypeId=null,EACodeMainId=608243,IsFormIdRequested=true ,FileName="Zadost_HD2"},
            new DocumentTypeItem{Id = 6 , ShortName="ZADOCERP", SalesArrangementTypeId=6   ,EACodeMainId=613226,IsFormIdRequested=true ,FileName="Cerpani_HU"},
            new DocumentTypeItem{Id = 7 , ShortName="ZADOOPCI", SalesArrangementTypeId=null,EACodeMainId=608578,IsFormIdRequested=true ,FileName="Zadost_Flexi"},
            new DocumentTypeItem{Id = 8 , ShortName="ZAOZMPAR", SalesArrangementTypeId=7   ,EACodeMainId=608279,IsFormIdRequested=true ,FileName="Zadost_obecna"},
            new DocumentTypeItem{Id = 9 , ShortName="ZAOZMDLU", SalesArrangementTypeId=9   ,EACodeMainId=608580,IsFormIdRequested=true ,FileName="Zadost_dluznik"},
            new DocumentTypeItem{Id = 10, ShortName="ZAODHUBN", SalesArrangementTypeId=8   ,EACodeMainId=608579,IsFormIdRequested=true ,FileName="Zadost_bezNem"},
            new DocumentTypeItem{Id = 11, ShortName="ZUSTAVSI", SalesArrangementTypeId=12  ,EACodeMainId=608524,IsFormIdRequested=true ,FileName="Zustavajici_v_HU"},
            new DocumentTypeItem{Id = 12, ShortName="PRISTOUP", SalesArrangementTypeId=10  ,EACodeMainId=608524,IsFormIdRequested=true ,FileName="Pristupujici_k_HU"},
            new DocumentTypeItem{Id = 13, ShortName="DANRESID", SalesArrangementTypeId=null,EACodeMainId=616578,IsFormIdRequested=true ,FileName="Prohlaseni_dan"},
            new DocumentTypeItem{Id = 14, ShortName="ZMENKLDA", SalesArrangementTypeId=null,EACodeMainId=616525,IsFormIdRequested=true ,FileName="Klientdata_zmena"},
            new DocumentTypeItem{Id = 15, ShortName="ODSTOUP" , SalesArrangementTypeId=null,EACodeMainId=608522,IsFormIdRequested=false,FileName="Ukonceni_zadosti_HU"},
            new DocumentTypeItem{Id = 16, ShortName="ZADOSTHD", SalesArrangementTypeId=11  ,EACodeMainId=608243,IsFormIdRequested=true ,FileName="Pridani_spoludluznika"}
            });
    }

    public Task<List<SigningMethodsForNaturalPersonItem>> SigningMethodsForNaturalPerson(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<SigningMethodsForNaturalPersonItem>
        {
            new SigningMethodsForNaturalPersonItem() { Code = "OFFERED", Order = 4, Name = "Delegovaná metoda podpisu", Description = "deprecated", IsValid = true, StarbuildEnumId = 2 },
            new SigningMethodsForNaturalPersonItem() { Code = "PHYSICAL", Order = 1, Name = "Ruční podpis", Description = "Fyzický/ruční podpis dokumentu.", IsValid = true, StarbuildEnumId = 1 },
            new SigningMethodsForNaturalPersonItem() { Code = "DELEGATE", Order = 1, Name = "Přímé bankovnictví", Description = "Přímé bankovnictví - Delegovaná metoda podpisu", IsValid = true, StarbuildEnumId = 2 },
            new SigningMethodsForNaturalPersonItem() { Code = "PAAT", Order = 1, Name = "KB klíč", Description = null, IsValid = true, StarbuildEnumId = 2 },
            new SigningMethodsForNaturalPersonItem() { Code = "INT_CERT_FILE", Order = 2, Name = "Interní certifikát v souboru", Description = null, IsValid = true, StarbuildEnumId = 2 },
            new SigningMethodsForNaturalPersonItem() { Code = "APOC", Order = 3, Name = "Automatizovaný Podpis Osobním Certifikátem", Description = null, IsValid = true, StarbuildEnumId = 2 },
        });
    }

    public Task<List<SalesArrangementTypeItem>> SalesArrangementTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<SalesArrangementTypeItem>{
            new SalesArrangementTypeItem{Id=1, Name="Žádost o hypotéční úvěr 20001",SalesArrangementCategory=1, ProductTypeId=20001, Description = null},
            new SalesArrangementTypeItem{Id=2 ,Name="Žádost o hypoteční překlenovací úvěry",  SalesArrangementCategory=1,ProductTypeId=20002, Description = null},
            new SalesArrangementTypeItem{Id=3 ,Name="Žádost o hypoteční úvěr bez příjmu",SalesArrangementCategory = 1, ProductTypeId = 20003, Description = null},
            new SalesArrangementTypeItem{Id=4 ,Name="Žádost o doprodej neúčelové části",SalesArrangementCategory = 1, ProductTypeId = 20004, Description = null},
            new SalesArrangementTypeItem{Id=5 ,Name="Žádost o americkou hypotéku",SalesArrangementCategory = 1, ProductTypeId = 20010, Description = null},
            new SalesArrangementTypeItem{Id=6 ,Name="Žádost o čerpání", SalesArrangementCategory = 2, Description = null},
            new SalesArrangementTypeItem{Id=7 ,Name="Žádost o změnu podmínek smlouvy o hypotečním úvěru", SalesArrangementCategory = 2, Description = null},
            new SalesArrangementTypeItem{Id=8 ,Name="Žádost o změnu / doplnění podmínek smlouvy o hypotečním úvěru bez nemovitosti", SalesArrangementCategory = 2, Description = null},
            new SalesArrangementTypeItem{Id=9 ,Name="Žádost o změnu dlužníků", SalesArrangementCategory = 2, Description = null},
            new SalesArrangementTypeItem{Id=10,Name="Údaje o přistupujícím k dluhu", SalesArrangementCategory = 2, Description = null},
            new SalesArrangementTypeItem{Id=11,Name="Žádost o přidání spoludlužníka", SalesArrangementCategory = 2, Description = null},
            new SalesArrangementTypeItem{Id=12,Name="Údaje o zůstávajícím v dluhu", SalesArrangementCategory = 2, Description = null},
        });
    }

    #endregion

    #region ResrOfCodebook

    public Task<List<GenericCodebookItem>> AcademicDegreesAfter(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> AcademicDegreesBefore(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> ActionCodesSavings(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> ActionCodesSavingsLoan(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItemWithCode>> AddressTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<BankCodeItem>> BankCodes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<CaseStateItem>> CaseStates(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<ChannelItem>> Channels(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> ClassificationOfEconomicActivities(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<CollateralTypeItem>> CollateralTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<ContactTypeItem>> ContactTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<CountriesItem>> Countries(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<CountryCodePhoneIdcItem>> CountryCodePhoneIdc(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<CurrenciesItem>> Currencies(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<CustomerProfileItem>> CustomerProfiles(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<CustomerRoleItem>> CustomerRoles(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DeveloperProjectItem>> DeveloperProjects(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DeveloperItemOld>> Developers(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DocumentFileTypeItem>> DocumentFileTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DocumentOnSATypeItem>> DocumentOnSATypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DocumentTemplateTypeItem>> DocumentTemplateTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DocumentTemplateVariantItem>> DocumentTemplateVariants(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DocumentTemplateVersionItem>> DocumentTemplateVersions(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DrawingDurationItem>> DrawingDurations(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DrawingTypeItem>> DrawingTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<EaCodeMainItem>> EaCodesMain(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<EducationLevelItem>> EducationLevels(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItemWithCode>> EmploymentTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<FeeItem>> Fees(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<FixedRatePeriodsItem>> FixedRatePeriods(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<FormTypeItem>> FormTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenderItem>> Genders(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<DeveloperItem> GetDeveloper(int developerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<DeveloperProjectItem> GetDeveloperProject(int developerId, int developerProjectId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GetGeneralDocumentListItem>> GetGeneralDocumentList(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<GetOperatorItem> GetOperator(string login, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<HouseholdTypeItem>> HouseholdTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<HousingConditionItem>> HousingConditions(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<IdentificationDocumentTypesItem>> IdentificationDocumentTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> IdentificationSubjectMethods(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<IdentitySchemeItem>> IdentitySchemes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItemWithCode>> IncomeForeignTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItemWithCode>> IncomeMainTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItemWithRdmCode>> IncomeMainTypesAML(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItemWithCode>> IncomeOtherTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<JobTypeItem>> JobTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<LegalCapacityRestrictionTypeItem>> LegalCapacityRestrictionTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<LoanInterestRateAnnouncedTypeItem>> LoanInterestRateAnnouncedTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<LoanKindsItem>> LoanKinds(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<LoanPurposesItem>> LoanPurposes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<MandantsItem>> Mandants(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<MaritalStatusItem>> MaritalStatuses(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<MarketingActionItem>> MarketingActions(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> MktActionCodesSavings(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> Nationalities(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<NetMonthEarningItem>> NetMonthEarnings(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItemWithCode>> ObligationCorrectionTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<ObligationLaExposureItem>> ObligationLaExposures(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<ObligationTypesItem>> ObligationTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<PaymentDayItem>> PaymentDays(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<PayoutTypeItem>> PayoutTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<PostCodeItem>> PostCodes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProductTypeItem>> ProductTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProfessionCategoryItem>> ProfessionCategories(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProfessionTypeItem>> ProfessionTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProofTypeItem>> ProofTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<PropertySettlementItem>> PropertySettlements(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<RealEstatePurchaseTypeItem>> RealEstatePurchaseTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<RealEstateTypeItem>> RealEstateTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<RelationshipCustomerProductTypeItem>> RelationshipCustomerProductTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<RepaymentScheduleTypeItem>> RepaymentScheduleTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<RiskApplicationTypeItem>> RiskApplicationTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SalesArrangementStateItem>> SalesArrangementStates(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }



    public Task<List<GenericCodebookItem>> SignatureStatesNoby(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SignatureTypeItem>> SignatureTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SmsNotificationTypeItem>> SmsNotificationTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<StatementFrequencyItem>> StatementFrequencies(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItemWithCodeAndDefault>> StatementSubscriptionTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<StatementTypeItem>> StatementTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<TinFormatItem>> TinFormatsByCountry(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<TinNoFillReasonItem>> TinNoFillReasonsByCountry(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<WorkflowConsultationMatrixItem>> WorkflowConsultationMatrix(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> WorkflowTaskCategories(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> WorkflowTaskConsultationTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> WorkflowTaskSigningResponseTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<WorkflowTaskStateItem>> WorkflowTaskStates(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<WorkflowTaskStateNobyItem>> WorkflowTaskStatesNoby(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<WorkflowTaskTypeItem>> WorkflowTaskTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenericCodebookItem>> WorkSectors(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion
}
