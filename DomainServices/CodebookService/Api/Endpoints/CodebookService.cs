using CIS.Core.Data;
using CIS.Foms.Enums;
using CIS.Infrastructure.Data.Synchronous;
using DomainServices.CodebookService.Api.Database;
using DomainServices.CodebookService.Api.Extensions;
using DomainServices.CodebookService.Contracts.v1;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CodebookService.Api.Endpoints;

[Authorize]
internal sealed class CodebookService
    : Contracts.v1.CodebookService.CodebookServiceBase
{
    public override Task<GenericCodebookResponse> AcademicDegreesAfter(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItems(SqlQueries.AcademicDegreesAfter);

    public override Task<GenericCodebookResponse> AcademicDegreesBefore(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetGenericItems(SqlQueries.AcademicDegreesBefore);

    public override Task<GenericCodebookWithCodeResponse> AddressTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItemsWithCode<CIS.Foms.Enums.AddressTypes>();

    public override Task<BankCodesResponse> BankCodes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<BankCodesResponse, BankCodesResponse.Types.BankCodeItem>(new BankCodesResponse(), SqlQueries.BankCodes);

    public override Task<CaseStatesResponse> CaseStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new CaseStatesResponse(), () => FastEnum.GetValues<CIS.Foms.Enums.CaseStates>()
            .Select(t => new CaseStatesResponse.Types.CaseStateItem
            {
                Id = (int)t,
                Code = t.ToString(),
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                IsDefault = t.HasAttribute<CIS.Core.Attributes.CisDefaultValueAttribute>()
            }));

    public override Task<GenericCodebookResponse> ClassificationOfEconomicActivities(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItems(SqlQueries.ClassificationOfEconomicActivities);

    public override Task<CollateralTypesResponse> CollateralTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<CollateralTypesResponse, CollateralTypesResponse.Types.CollateralTypeItem>(new CollateralTypesResponse(), SqlQueries.CollateralTypes);

    public override Task<ContactTypesResponse> ContactTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new ContactTypesResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<ContactTypesResponse.Types.ContactTypeItem>(SqlQueries.ContactTypes);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList("SELECT [ContactTypeId], [MpDigiApiCode] FROM [dbo].[ContactTypeExtension]");
            items.ForEach(item =>
            {
                item.MpDigiApiCode = extensions.FirstOrDefault(t => t.ContactTypeId == item.Id)?.MpDigiApiCode;
            });
            return items;
        });

    public override Task<CountriesResponse> Countries(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<CountriesResponse, CountriesResponse.Types.CountryItem>(new CountriesResponse(), SqlQueries.Countries);

    public override Task<CountryCodePhoneIdcResponse> CountryCodePhoneIdc(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new CountryCodePhoneIdcResponse(), () => new List<Contracts.v1.CountryCodePhoneIdcResponse.Types.CountryCodePhoneIdcItem>
        {
            new() { Id = "AD+376", Name = "AD", Idc = "+376",  IsValid = true, IsPriority = false, IsDefault = false },
            new() { Id = "AE+971", Name = "AE", Idc = "+971",  IsValid = true, IsPriority = false, IsDefault = false },
            new() { Id = "AF+93", Name = "AF", Idc = "+93",  IsValid = true, IsPriority = false, IsDefault = false },
            new() { Id = "AG+1268", Name = "AG", Idc = "+1268",  IsValid = true, IsPriority = false, IsDefault = false },
            new() { Id = "AI+1264", Name = "AI", Idc = "+1264",  IsValid = true, IsPriority = false, IsDefault = false },
            new() { Id = "CZ+420", Name = "CZ", Idc = "+420",  IsValid = true, IsPriority = true, IsDefault = true },
        });

    public override Task<CurrenciesResponse> Currencies(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<CurrenciesResponse, CurrenciesResponse.Types.CurrencyItem>(new CurrenciesResponse(), SqlQueries.Currencies);

    public override Task<GenericCodebookWithCodeResponse> CustomerProfiles(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItemsWithCode<CIS.Foms.Enums.CustomerProfiles>();

    public override Task<CustomerRolesResponse> CustomerRoles(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new CustomerRolesResponse(), () =>
        {
            return FastEnum.GetValues<CIS.Foms.Enums.CustomerRoles>()
                .Select(t => new Contracts.v1.CustomerRolesResponse.Types.CustomerRoleItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    RdmCode = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? "",
                    NameNoby = t switch
                    {
                        CIS.Foms.Enums.CustomerRoles.Debtor => "Hlavní žadatel",
                        CIS.Foms.Enums.CustomerRoles.Codebtor => "Spoludlužník",
                        CIS.Foms.Enums.CustomerRoles.Garantor => "Ručitel",
                        _ => ""
                    },
                })
                .ToList();
        });

    public override Task<DeveloperSearchResponse> DeveloperSearch(DeveloperSearchRequest request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Term))
            return Task.FromResult(new DeveloperSearchResponse());

        return Helpers.GetItems(new DeveloperSearchResponse(), () =>
        {
            var terms = request.Term.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var termsValues = String.Join(",", terms.Select(t => $"('{t}')"));

            var developersAndProjectsQuery = SqlQueries.DeveloperSearchWithProjects.Replace("<terms>", termsValues);
            var developersQuery = SqlQueries.DeveloperSearch.Replace("<terms>", termsValues);

            var developersAndProjects = _xxd.ExecuteDapperRawSqlToList<DeveloperSearchResponse.Types.DeveloperSearchItem>(developersAndProjectsQuery);
            var developers = _xxd.ExecuteDapperRawSqlToList<DeveloperSearchResponse.Types.DeveloperSearchItem>(developersQuery);

            return developersAndProjects.Concat(developers).ToList();
        });
    }

    public override Task<DocumentFileTypesResponse> DocumentFileTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new DocumentFileTypesResponse(), () =>
        {
            return FastEnum.GetValues<CIS.Foms.Enums.DocumentFileType>()
                .Select(t => new DocumentFileTypesResponse.Types.DocumentFileTypeItem()
                {
                    Id = (int)t,
                    DocumenFileType = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    IsPrintingSupported = t == CIS.Foms.Enums.DocumentFileType.PdfA || t == CIS.Foms.Enums.DocumentFileType.OpenForm
                })
                .ToList();
        });

    public override Task<DocumentOnSATypesResponse> DocumentOnSATypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<DocumentOnSATypesResponse, DocumentOnSATypesResponse.Types.DocumentOnSATypeItem>(new DocumentOnSATypesResponse(), SqlQueries.DocumentOnSATypes);

    public override Task<DocumentTemplateTypesResponse> DocumentTemplateTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new DocumentTemplateTypesResponse(), () =>
        {
            return FastEnum.GetValues<CIS.Foms.Enums.DocumentFileType>()
                .Select(t => new DocumentTemplateTypesResponse.Types.DocumentTemplateTypeItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    ShortName = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? ""
                })
                .ToList();
        });

    public override Task<DocumentTemplateVariantsResponse> DocumentTemplateVariants(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<DocumentTemplateVariantsResponse, DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem>(new DocumentTemplateVariantsResponse(), SqlQueries.DocumentTemplateVariants);

    public override Task<DocumentTemplateVersionsResponse> DocumentTemplateVersions(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<DocumentTemplateVersionsResponse, DocumentTemplateVersionsResponse.Types.DocumentTemplateVersionItem>(new DocumentTemplateVersionsResponse(), SqlQueries.DocumentTemplateVersions);

    public override Task<DocumentTypesResponse> DocumentTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<DocumentTypesResponse, DocumentTypesResponse.Types.DocumentTypeItem>(new DocumentTypesResponse(), SqlQueries.DocumentTypes);

    public override Task<DrawingDurationsResponse> DrawingDurations(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<DrawingDurationsResponse, DrawingDurationsResponse.Types.DrawingDurationItem>(new DrawingDurationsResponse(), SqlQueries.DrawingDurations);

    public override Task<DrawingTypesResponse> DrawingTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new DrawingTypesResponse(), () =>
        {
            return FastEnum.GetValues<CIS.Foms.Enums.DrawingTypes>()
                .Select(t => new DrawingTypesResponse.Types.DrawingTypeItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    StarbuildId = t.GetAttribute<CIS.Core.Attributes.CisStarbuildIdAttribute>()?.StarbuildId
                })
                .ToList();
        });

    public override Task<EaCodesMainResponse> EaCodesMain(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new EaCodesMainResponse(), () =>
        {
            var items = _selfDb.ExecuteDapperRawSqlToList<EaCodesMainResponse.Types.EaCodesMainItem>(SqlQueries.EaCodesMain1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.EaCodesMain2);
            items.ForEach(item =>
            {
                item.IsFormIdRequested = extensions.FirstOrDefault(t => t.EaCodesMainId == item.Id)?.IsFormIdRequested ?? false;
            });
            return items;
        });

    public override Task<EducationLevelsResponse> EducationLevels(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<EducationLevelsResponse, EducationLevelsResponse.Types.EducationLevelItem>(new EducationLevelsResponse(), SqlQueries.EducationLevels);

    public override Task<GenericCodebookWithCodeResponse> EmploymentTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItemsWithCode(SqlQueries.EmploymentTypes);

    public override Task<FeesResponse> Fees(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<FeesResponse, FeesResponse.Types.FeeItem>(new FeesResponse(), SqlQueries.Fees);

    public override Task<FixedRatePeriodsResponse> FixedRatePeriods(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<FixedRatePeriodsResponse, FixedRatePeriodsResponse.Types.FixedRatePeriodItem>(new FixedRatePeriodsResponse(), SqlQueries.FixedRatePeriods);

    public override Task<FormTypesResponse> FormTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<FormTypesResponse, FormTypesResponse.Types.FormTypeItem>(new FormTypesResponse(), SqlQueries.FormTypes);

    public override Task<GendersResponse> Genders(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new GendersResponse(), () =>
        {
            return FastEnum.GetValues<CIS.Foms.Enums.Genders>()
                .Select(t => new GendersResponse.Types.GenderItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    KonsDBCode = (int)t,
                    KbCmCode = t == CIS.Foms.Enums.Genders.Male ? "M" : "F",
                    StarBuildJsonCode = t == CIS.Foms.Enums.Genders.Male ? "M" : "Z"
                })
                .ToList();
        });

    public override Task<GetDeveloperResponse> GetDeveloper(GetDeveloperRequest request, ServerCallContext context)
        => _xxd.GetItems<GetDeveloperResponse, GetDeveloperResponse.Types.GetDeveloperItem>(new GetDeveloperResponse(), SqlQueries.GetDeveloper, new { request.DeveloperId });

    public override Task<GetDeveloperProjectResponse> GetDeveloperProject(GetDeveloperProjectRequest request, ServerCallContext context)
        => _xxd.GetItems<GetDeveloperProjectResponse, GetDeveloperProjectResponse.Types.GetDeveloperProjectItem>(new GetDeveloperProjectResponse(), SqlQueries.GetDeveloperProject, new { request.DeveloperProjectId, request.DeveloperId });

    public override Task<GetGeneralDocumentListResponse> GetGeneralDocumentList(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<GetGeneralDocumentListResponse, GetGeneralDocumentListResponse.Types.GetGeneralDocumentListItem>(new GetGeneralDocumentListResponse(), SqlQueries.GetGeneralDocumentList);

    public override Task<GetOperatorResponse> GetOperator(GetOperatorRequest request, ServerCallContext context)
        => _xxd.GetItems<GetOperatorResponse, GetOperatorResponse.Types.GetOperatorItem>(new GetOperatorResponse(), SqlQueries.GetOperator, new { request.PerformerLogin });

    public override Task<HouseholdTypesResponse> HouseholdTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new HouseholdTypesResponse(), () =>
        {
            return FastEnum.GetValues<CIS.Foms.Enums.HouseholdTypes>()
                .Select(t => new HouseholdTypesResponse.Types.HouseholdTypeItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    RdmCode = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? "",
                    DocumentTypeId = t switch
                    {
                        CIS.Foms.Enums.HouseholdTypes.Main => 4,
                        CIS.Foms.Enums.HouseholdTypes.Codebtor => 5,
                        _ => null
                    }
                })
            .ToList();
        });

    public override Task<HousingConditionsResponse> HousingConditions(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<HousingConditionsResponse, HousingConditionsResponse.Types.HousingConditionItem>(new HousingConditionsResponse(), SqlQueries.HousingConditions);

    public override Task<ChannelsResponse> Channels(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new ChannelsResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<ChannelsResponse.Types.ChannelItem>(SqlQueries.Channels1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.Channels2);
            items.ForEach(item =>
            {
                item.RdmCbChannelCode = extensions.FirstOrDefault(t => t.ChannelId == item.Id)?.RdmCbChannelCode;
            });
            return items;
        });

    public override Task<IdentificationDocumentTypesResponse> IdentificationDocumentTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new IdentificationDocumentTypesResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem>(SqlQueries.IdentificationDocumentTypes1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.IdentificationDocumentTypes2);
            items.ForEach(item =>
            {
                item.MpDigiApiCode = extensions.FirstOrDefault(t => t.IdentificationDocumentTypeId == item.Id)?.MpDigiApiCode;
            });
            return items;
        });

    public override Task<GenericCodebookResponse> IdentificationSubjectMethods(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new GenericCodebookResponse(), () => new List<GenericCodebookResponse.Types.GenericCodebookItem>
        {
            new() { Id = 1, Name = "za fyzické přítomnosti", IsValid = true },
            new() { Id = 3, Name = "ověření notářem, krajským nebo obecním úřadem", IsValid = true },
            new() { Id = 8, Name = "zástupce MPSS", IsValid = true }
        });

    public override Task<IdentitySchemesResponse> IdentitySchemes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new IdentitySchemesResponse(), () => new List<IdentitySchemesResponse.Types.IdentitySchemeItem>
        {
            new() { Id = 1, Code = "KBID", Name = "Identifikátor KB klienta v Customer managementu", MandantId = 2, Category = "Customer", ChannelId = null },
            new() { Id = 2, Code = "MPSBID", Name = "PartnerId ve Starbuildu", MandantId = 1, Category = "Customer", ChannelId = null },
            new() { Id = 3, Code = "MPEKID", Name = "KlientId v eKmenu", MandantId = 1, Category = "Customer", ChannelId = null },
            new() { Id = 4, Code = "KBUID", Name = null, MandantId = 2, Category = "User", ChannelId = 4 },
            new() { Id = 5, Code = "M04ID", Name = null, MandantId = 1, Category = "User", ChannelId = 1 },
            new() { Id = 6, Code = "M17ID", Name = null, MandantId = 1, Category = "User", ChannelId = 1 },
            new() { Id = 7, Code = "BrokerId", Name = null, MandantId = 2, Category = "User", ChannelId = 6 },
            new() { Id = 8, Code = "MPAD", Name = null, MandantId = 1, Category = "User", ChannelId = 1 }
        });

    public override Task<GenericCodebookWithCodeResponse> IncomeForeignTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItemsWithCode(SqlQueries.IncomeForeignTypes);

    public override Task<GenericCodebookWithCodeResponse> IncomeMainTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItemsWithCode(SqlQueries.IncomeMainTypes);

    public override Task<IncomeMainTypesAMLResponse> IncomeMainTypesAML(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new IncomeMainTypesAMLResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<IncomeMainTypesAMLResponse.Types.IncomeMainTypesAMLItem>(SqlQueries.IncomeMainTypesAML1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.IncomeMainTypesAML2);
            items.ForEach(item =>
            {
                item.RdmCode = extensions.FirstOrDefault(t => t.Id == item.Id)?.RdmCode;
            });
            return items;
        });

    public override Task<GenericCodebookWithCodeResponse> IncomeOtherTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItemsWithCode(SqlQueries.IncomeOtherTypes);

    public override Task<JobTypesResponse> JobTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<JobTypesResponse, JobTypesResponse.Types.JobTypeItem>(new JobTypesResponse(), SqlQueries.JobTypes);

    public override Task<LegalCapacityRestrictionTypesResponse> LegalCapacityRestrictionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new LegalCapacityRestrictionTypesResponse(), () =>
        {
            return FastEnum.GetValues<CIS.Foms.Enums.LegalCapacityRestrictions>()
                .Select(t => new LegalCapacityRestrictionTypesResponse.Types.LegalCapacityRestrictionTypeItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    Description = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Description ?? "",
                    RdmCode = t.ToString()
                })
            .ToList();
        });

    public override Task<GenericCodebookWithCodeResponse> LoanInterestRateAnnouncedTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItemsWithCode<LoanInterestRateAnnouncedTypes>();

    public override Task<LoanKindsResponse> LoanKinds(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<LoanKindsResponse, LoanKindsResponse.Types.LoanKindItem>(new LoanKindsResponse(), SqlQueries.LoanKinds);

    public override Task<LoanPurposesResponse> LoanPurposes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new LoanPurposesResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToDynamicList(SqlQueries.LoanPurposes);
            return items.Select(t =>
            {
                var item = new LoanPurposesResponse.Types.LoanPurposeItem
                {
                    Id = t.Id,
                    C4MId = t.C4MId,
                    IsValid = t.IsValid,
                    MandantId = t.MandantId,
                    Name = t.Name,
                    Order = t.Order
                };
                item.ProductTypeIds.AddRange(((string)t.ProductTypeIds).ParseIDs());
                return item;
            });
        });

    public override Task<GenericCodebookWithCodeResponse> Mandants(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItemsWithCode<CIS.Foms.Enums.Mandants>();

    public override Task<MaritalStatusesResponse> MaritalStatuses(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new MaritalStatusesResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<MaritalStatusesResponse.Types.MaritalStatuseItem>(SqlQueries.MaritalStatuses1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.MaritalStatuses2);
            items.ForEach(item =>
            {
                item.RdmMaritalStatusCode = extensions.FirstOrDefault(t => t.MaritalStatusId == item.Id)?.RDMCode;
            });
            return items;
        });

    private readonly IConnectionProvider _selfDb;
    private readonly IConnectionProvider<IKonsdbDapperConnectionProvider> _konsdb;
    private readonly IConnectionProvider<IXxdHfDapperConnectionProvider> _xxdhf;
    private readonly IConnectionProvider<IXxdDapperConnectionProvider> _xxd;

    public CodebookService(
        IConnectionProvider selfDb,
        IConnectionProvider<IKonsdbDapperConnectionProvider> konsdb, 
        IConnectionProvider<IXxdHfDapperConnectionProvider> xxdhf, 
        IConnectionProvider<IXxdDapperConnectionProvider> xxd)
    {
        _selfDb = selfDb;
        _konsdb = konsdb;
        _xxdhf = xxdhf;
        _xxd = xxd;
    }
}