using DomainServices.CodebookService.Contracts.v1;
using static DomainServices.CodebookService.Contracts.v1.DocumentTypesResponse.Types;
using static DomainServices.CodebookService.Contracts.v1.SalesArrangementTypesResponse.Types;
using static DomainServices.CodebookService.Contracts.v1.SigningMethodsForNaturalPersonResponse.Types;

namespace DomainServices.CodebookService.Clients.Services;

public class CodebookServiceMock
     : ICodebookServiceClient
{
    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> AcademicDegreesAfter(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> AcademicDegreesBefore(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem>> AddressTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<BankCodesResponse.Types.BankCodeItem>> BankCodes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithDefaultAndCodeResponse.Types.GenericCodebookWithDefaultAndCodeItem>> CaseStates(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<ChannelsResponse.Types.ChannelItem>> Channels(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> ClassificationOfEconomicActivities(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<CollateralTypesResponse.Types.CollateralTypeItem>> CollateralTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<ContactTypesResponse.Types.ContactTypeItem>> ContactTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<CountriesResponse.Types.CountryItem>> Countries(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<CountryCodePhoneIdcResponse.Types.CountryCodePhoneIdcItem>> CountryCodePhoneIdc(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<CurrenciesResponse.Types.CurrencyItem>> Currencies(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem>> CustomerProfiles(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<CustomerRolesResponse.Types.CustomerRoleItem>> CustomerRoles(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<DeveloperSearchResponse.Types.DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<DocumentFileTypesResponse.Types.DocumentFileTypeItem>> DocumentFileTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<DocumentOnSATypesResponse.Types.DocumentOnSATypeItem>> DocumentOnSATypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<DocumentTemplateTypesResponse.Types.DocumentTemplateTypeItem>> DocumentTemplateTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem>> DocumentTemplateVariants(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<DocumentTemplateVersionsResponse.Types.DocumentTemplateVersionItem>> DocumentTemplateVersions(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<DocumentTypesResponse.Types.DocumentTypeItem>> DocumentTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<DocumentTypeItem>
        {
            new() {Id = 1 , ShortName="NABIDKA" , SalesArrangementTypeId=null,EACodeMainId=605569,IsFormIdRequested=false,FileName="Nabidka_HU"},
            new() {Id = 2 , ShortName="KALKULHU", SalesArrangementTypeId=null,EACodeMainId=null  ,IsFormIdRequested=false,FileName="Kalkulace_HU"},
            new() {Id = 3 , ShortName="SPLKALHU", SalesArrangementTypeId=null,EACodeMainId=null  ,IsFormIdRequested=false,FileName="Splatkovy_kalendar"},
            new() {Id = 4 , ShortName="ZADOSTHU", SalesArrangementTypeId=null,EACodeMainId=608248,IsFormIdRequested=true ,FileName="Zadost_HU1"},
            new() {Id = 5 , ShortName="ZADOSTHD", SalesArrangementTypeId=null,EACodeMainId=608243,IsFormIdRequested=true ,FileName="Zadost_HD2"},
            new() {Id = 6 , ShortName="ZADOCERP", SalesArrangementTypeId=6   ,EACodeMainId=613226,IsFormIdRequested=true ,FileName="Cerpani_HU"},
            new() {Id = 7 , ShortName="ZADOOPCI", SalesArrangementTypeId=null,EACodeMainId=608578,IsFormIdRequested=true ,FileName="Zadost_Flexi"},
            new() {Id = 8 , ShortName="ZAOZMPAR", SalesArrangementTypeId=7   ,EACodeMainId=608279,IsFormIdRequested=true ,FileName="Zadost_obecna"},
            new() {Id = 9 , ShortName="ZAOZMDLU", SalesArrangementTypeId=9   ,EACodeMainId=608580,IsFormIdRequested=true ,FileName="Zadost_dluznik"},
            new() {Id = 10, ShortName="ZAODHUBN", SalesArrangementTypeId=8   ,EACodeMainId=608579,IsFormIdRequested=true ,FileName="Zadost_bezNem"},
            new() {Id = 11, ShortName="ZUSTAVSI", SalesArrangementTypeId=12  ,EACodeMainId=608524,IsFormIdRequested=true ,FileName="Zustavajici_v_HU"},
            new() {Id = 12, ShortName="PRISTOUP", SalesArrangementTypeId=10  ,EACodeMainId=608524,IsFormIdRequested=true ,FileName="Pristupujici_k_HU"},
            new() {Id = 13, ShortName="DANRESID", SalesArrangementTypeId=null,EACodeMainId=616578,IsFormIdRequested=true ,FileName="Prohlaseni_dan"},
            new() {Id = 14, ShortName="ZMENKLDA", SalesArrangementTypeId=null,EACodeMainId=616525,IsFormIdRequested=true ,FileName="Klientdata_zmena"},
            new() {Id = 15, ShortName="ODSTOUP" , SalesArrangementTypeId=null,EACodeMainId=608522,IsFormIdRequested=false,FileName="Ukonceni_zadosti_HU"},
            new() {Id = 16, ShortName="ZADOSTHD", SalesArrangementTypeId=11  ,EACodeMainId=608243,IsFormIdRequested=true ,FileName="Pridani_spoludluznika"}
            });
    }

    public virtual Task<List<DrawingDurationsResponse.Types.DrawingDurationItem>> DrawingDurations(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<DrawingTypesResponse.Types.DrawingTypeItem>> DrawingTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<EaCodesMainResponse.Types.EaCodesMainItem>> EaCodesMain(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<EducationLevelsResponse.Types.EducationLevelItem>> EducationLevels(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem>> EmploymentTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<FeesResponse.Types.FeeItem>> Fees(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<FixedRatePeriodsResponse.Types.FixedRatePeriodItem>> FixedRatePeriods(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<FormTypesResponse.Types.FormTypeItem>> FormTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GendersResponse.Types.GenderItem>> Genders(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<GetDeveloperResponse> GetDeveloper(int developerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<GetDeveloperProjectResponse> GetDeveloperProject(int developerId, int developerProjectId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GetGeneralDocumentListResponse.Types.GetGeneralDocumentListItem>> GetGeneralDocumentList(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<GetOperatorResponse> GetOperator(string performerLogin, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<HouseholdTypesResponse.Types.HouseholdTypeItem>> HouseholdTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<HousingConditionsResponse.Types.HousingConditionItem>> HousingConditions(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem>> IdentificationDocumentTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> IdentificationSubjectMethods(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<IdentitySchemesResponse.Types.IdentitySchemeItem>> IdentitySchemes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem>> IncomeForeignTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem>> IncomeMainTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithRdmCodeResponse.Types.GenericCodebookWithRdmCodeItem>> IncomeMainTypesAML(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem>> IncomeOtherTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithDefaultAndCodeResponse.Types.GenericCodebookWithDefaultAndCodeItem>> JobTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<LegalCapacityRestrictionTypesResponse.Types.LegalCapacityRestrictionTypeItem>> LegalCapacityRestrictionTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem>> LoanInterestRateAnnouncedTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookFullResponse.Types.GenericCodebookFullItem>> LoanKinds(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<LoanPurposesResponse.Types.LoanPurposeItem>> LoanPurposes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem>> Mandants(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<MaritalStatusesResponse.Types.MaritalStatuseItem>> MaritalStatuses(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<MarketingActionsResponse.Types.MarketingActionItem>> MarketingActions(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> Nationalities(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithRdmCodeResponse.Types.GenericCodebookWithRdmCodeItem>> NetMonthEarnings(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem>> ObligationCorrectionTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem>> ObligationLaExposures(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<ObligationTypesResponse.Types.ObligationTypeItem>> ObligationTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<PaymentDaysResponse.Types.PaymentDayItem>> PaymentDays(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> PayoutTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<PostCodesResponse.Types.PostCodeItem>> PostCodes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<ProductTypesResponse.Types.ProductTypeItem>> ProductTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<ProfessionCategoriesResponse.Types.ProfessionCategoryItem>> ProfessionCategories(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithRdmCodeResponse.Types.GenericCodebookWithRdmCodeItem>> ProfessionTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<ProofTypesResponse.Types.ProofTypeItem>> ProofTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<PropertySettlementsResponse.Types.PropertySettlementItem>> PropertySettlements(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookFullResponse.Types.GenericCodebookFullItem>> RealEstatePurchaseTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookFullResponse.Types.GenericCodebookFullItem>> RealEstateTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem>> RelationshipCustomerProductTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem>> RepaymentScheduleTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<RiskApplicationTypesResponse.Types.RiskApplicationTypeItem>> RiskApplicationTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<SalesArrangementStatesResponse.Types.SalesArrangementStateItem>> SalesArrangementStates(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<SalesArrangementTypesResponse.Types.SalesArrangementTypeItem>> SalesArrangementTypes(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<SalesArrangementTypeItem>{
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

    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> SignatureStatesNoby(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithDefaultAndCodeResponse.Types.GenericCodebookWithDefaultAndCodeItem>> SignatureTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<SigningMethodsForNaturalPersonResponse.Types.SigningMethodsForNaturalPersonItem>> SigningMethodsForNaturalPerson(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<SigningMethodsForNaturalPersonItem>
        {
            new() { Code = "OFFERED", Order = 4, Name = "Delegovaná metoda podpisu", Description = "deprecated", IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "PHYSICAL", Order = 1, Name = "Ruční podpis", Description = "Fyzický/ruční podpis dokumentu.", IsValid = true, StarbuildEnumId = 1 },
            new() { Code = "DELEGATE", Order = 1, Name = "Přímé bankovnictví", Description = "Přímé bankovnictví - Delegovaná metoda podpisu", IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "PAAT", Order = 1, Name = "KB klíč", Description = null, IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "INT_CERT_FILE", Order = 2, Name = "Interní certifikát v souboru", Description = null, IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "APOC", Order = 3, Name = "Automatizovaný Podpis Osobním Certifikátem", Description = null, IsValid = true, StarbuildEnumId = 2 },
        });
    }

    public virtual Task<List<SmsNotificationTypesResponse.Types.SmsNotificationTypeItem>> SmsNotificationTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<StatementFrequenciesResponse.Types.StatementFrequencyItem>> StatementFrequencies(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookWithDefaultAndCodeResponse.Types.GenericCodebookWithDefaultAndCodeItem>> StatementSubscriptionTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<StatementTypesResponse.Types.StatementTypeItem>> StatementTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<TinFormatsByCountryResponse.Types.TinFormatsByCountryItem>> TinFormatsByCountry(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<TinNoFillReasonsByCountryResponse.Types.TinNoFillReasonsByCountryItem>> TinNoFillReasonsByCountry(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<WorkflowConsultationMatrixResponse.Types.WorkflowConsultationMatrixItem>> WorkflowConsultationMatrix(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> WorkflowTaskCategories(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> WorkflowTaskConsultationTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> WorkflowTaskSigningResponseTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem>> WorkflowTaskStates(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<WorkflowTaskStatesNobyResponse.Types.WorkflowTaskStatesNobyItem>> WorkflowTaskStatesNoby(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<WorkflowTaskTypesResponse.Types.WorkflowTaskTypesItem>> WorkflowTaskTypes(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<GenericCodebookResponse.Types.GenericCodebookItem>> WorkSectors(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
