using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
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

    public override Task<GenericCodebookResponse> AddressTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.AddressTypes>();

    public override Task<BankCodesResponse> BankCodes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<BankCodesResponse, BankCodesResponse.Types.BankCodeItem>(new BankCodesResponse(), SqlQueries.BankCodes);

    public override Task<GenericCodebookWithDefaultAndCodeResponse> CaseStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItemsWithDefaultAndCode<CIS.Foms.Enums.CaseStates>();

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

    public override Task<GenericCodebookResponse> CustomerProfiles(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.CustomerProfiles>();

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

    public override async Task<DeveloperSearchResponse> DeveloperSearch(DeveloperSearchRequest request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Term))
            return new DeveloperSearchResponse();

        var terms = request.Term.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var termsValues = String.Join(",", terms.Select(t => $"('{t}')"));

        var developersAndProjectsQuery = SqlQueries.DeveloperSearchWithProjects.Replace("<terms>", termsValues);
        var developersQuery = SqlQueries.DeveloperSearch.Replace("<terms>", termsValues);

        var developersAndProjects = await _xxd.ExecuteDapperRawSqlToListAsync<DeveloperSearchResponse.Types.DeveloperSearchItem>(developersAndProjectsQuery);
        var developers = await _xxd.ExecuteDapperRawSqlToListAsync<DeveloperSearchResponse.Types.DeveloperSearchItem>(developersQuery);

        var data = developersAndProjects.Concat(developers).ToList();

        var result = new DeveloperSearchResponse();
        result.Items.AddRange(data);
        return result;
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

    public override Task<GenericCodebookResponse> EmploymentTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItems(SqlQueries.EmploymentTypes);

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

    public override async Task<GetDeveloperResponse> GetDeveloper(GetDeveloperRequest request, ServerCallContext context)
    {
        return (await _xxd.ExecuteDapperFirstOrDefaultAsync<GetDeveloperResponse>(SqlQueries.GetDeveloper, new { request.DeveloperId }))
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DeveloperNotFound, request.DeveloperId);
    }

    public override async Task<GetDeveloperProjectResponse> GetDeveloperProject(GetDeveloperProjectRequest request, ServerCallContext context)
    {
        return (await _xxd.ExecuteDapperFirstOrDefaultAsync<GetDeveloperProjectResponse>(SqlQueries.GetDeveloperProject, new { request.DeveloperProjectId, request.DeveloperId }))
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DeveloperProjectNotFound, request.DeveloperProjectId);
    }

    public override Task<GetGeneralDocumentListResponse> GetGeneralDocumentList(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<GetGeneralDocumentListResponse, GetGeneralDocumentListResponse.Types.GetGeneralDocumentListItem>(new GetGeneralDocumentListResponse(), SqlQueries.GetGeneralDocumentList);

    public override async Task<GetOperatorResponse> GetOperator(GetOperatorRequest request, ServerCallContext context)
    {
        using var connection = _xxd.Create();
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<GetOperatorResponse>(SqlQueries.GetOperator, new { request.PerformerLogin });
    }
    
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
            new() { Id = 4, Code = "KBUID", MandantId = 2, Category = "User", ChannelId = 4 },
            new() { Id = 5, Code = "M04ID", MandantId = 1, Category = "User", ChannelId = 1 },
            new() { Id = 6, Code = "M17ID", MandantId = 1, Category = "User", ChannelId = 1 },
            new() { Id = 7, Code = "BrokerId", MandantId = 2, Category = "User", ChannelId = 6 },
            new() { Id = 8, Code = "MPAD", MandantId = 1, Category = "User", ChannelId = 1 }
        });

    public override Task<GenericCodebookResponse> IncomeForeignTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItems(SqlQueries.IncomeForeignTypes);

    public override Task<GenericCodebookResponse> IncomeMainTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItems(SqlQueries.IncomeMainTypes);

    public override Task<GenericCodebookWithRdmCodeResponse> IncomeMainTypesAML(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new GenericCodebookWithRdmCodeResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<GenericCodebookWithRdmCodeResponse.Types.GenericCodebookWithRdmCodeItem>(SqlQueries.IncomeMainTypesAML1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.IncomeMainTypesAML2);
            items.ForEach(item =>
            {
                item.RdmCode = extensions.FirstOrDefault(t => t.Id == item.Id)?.RdmCode;
            });
            return items;
        });

    public override Task<GenericCodebookResponse> IncomeOtherTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItems(SqlQueries.IncomeOtherTypes);

    public override Task<GenericCodebookWithDefaultAndCodeResponse> JobTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItemsWithDefaultAndCode(SqlQueries.JobTypes);

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

    public override Task<GenericCodebookResponse> LoanInterestRateAnnouncedTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.LoanInterestRateAnnouncedTypes>();

    public override Task<GenericCodebookFullResponse> LoanKinds(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericFullItems(SqlQueries.LoanKinds);

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
                if (!string.IsNullOrEmpty(t.ProductTypeIds))
                {
                    item.ProductTypeIds.AddRange(((string)t.ProductTypeIds).ParseIDs());
                }
                return item;
            });
        });

    public override Task<GenericCodebookResponse> Mandants(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.Mandants>();

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

    public override Task<MarketingActionsResponse> MarketingActions(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<MarketingActionsResponse, MarketingActionsResponse.Types.MarketingActionItem>(new MarketingActionsResponse(), SqlQueries.MarketingActions);

    public override Task<GenericCodebookResponse> Nationalities(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _konsdb.GetGenericItems(SqlQueries.Nationalities);

    public override Task<GenericCodebookWithRdmCodeResponse> NetMonthEarnings(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new GenericCodebookWithRdmCodeResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<GenericCodebookWithRdmCodeResponse.Types.GenericCodebookWithRdmCodeItem>(SqlQueries.NetMonthEarnings1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.NetMonthEarnings2);
            items.ForEach(item =>
            {
                item.RdmCode = extensions.FirstOrDefault(t => t.NetMonthEarningId == item.Id)?.RdmCode;
            });
            return items;
        });

    public override Task<GenericCodebookResponse> ObligationCorrectionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItems(SqlQueries.ObligationCorrectionTypes);

    public override Task<ObligationLaExposuresResponse> ObligationLaExposures(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<ObligationLaExposuresResponse, ObligationLaExposuresResponse.Types.ObligationLaExposureItem>(new ObligationLaExposuresResponse(), SqlQueries.ObligationLaExposures);

    public override Task<ObligationTypesResponse> ObligationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new ObligationTypesResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<ObligationTypesResponse.Types.ObligationTypeItem>(SqlQueries.ObligationTypes1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.ObligationTypes2);
            items.ForEach(item =>
            {
                item.ObligationProperty = extensions.FirstOrDefault(t => t.ObligationTypeId == item.Id)?.ObligationProperty;
            });
            return items;
        });

    public override Task<PaymentDaysResponse> PaymentDays(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<PaymentDaysResponse, PaymentDaysResponse.Types.PaymentDayItem>(new PaymentDaysResponse(), SqlQueries.PaymentDays);

    public override Task<GenericCodebookResponse> PayoutTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.PayoutTypes>();

    public override Task<PostCodesResponse> PostCodes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<PostCodesResponse, PostCodesResponse.Types.PostCodeItem>(new PostCodesResponse(), SqlQueries.PostCodes);

    public override Task<ProductTypesResponse> ProductTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new ProductTypesResponse(), () =>
        {
            var items = _xxdhf.ExecuteDapperRawSqlToList<ProductTypesResponse.Types.ProductTypeItem>(SqlQueries.ProductTypes1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.ProductTypes2);
            var loanKinds = _xxd.GetOrCreateCachedResponse<GenericCodebookFullResponse.Types.GenericCodebookFullItem>(SqlQueries.LoanKinds, nameof(LoanKinds)).Select(t => t.Id).ToArray();

            items.ForEach(item =>
            {
                var types = item.MpHomeApiLoanType?.ParseIDs()?.Where(x => loanKinds.Contains(x)).ToList();
                if (types is not null)
                {
                    item.LoanKindIds.AddRange(types);
                }

                var ext = extensions.FirstOrDefault(x => x.ProductTypeId == item.Id);
                item.MpHomeApiLoanType = ext?.MpHomeApiLoanType;
                item.KonsDbLoanType = ext?.KonsDbLoanType;
                item.MandantId = ext?.MandantId;
            });
            return items;
        });

    public override Task<ProfessionCategoriesResponse> ProfessionCategories(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new ProfessionCategoriesResponse(), () =>
        {
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.ProfessionCategories);

            var items = new List<ProfessionCategoriesResponse.Types.ProfessionCategoryItem>() {
                new() { Id = 0, Name = "odmítl sdělit", IsValid = true},
                new() { Id = 1, Name = "státní zaměstnanec", IsValid = true},
                new() { Id = 2, Name = "zaměstnanec subjektu se státní majetkovou účastí", IsValid = true},
                new() { Id = 3, Name = "zaměstnanec subjektu se zahraničním vlastníkem", IsValid = true},
                new() { Id = 4, Name = "podnikatel", IsValid = true},
                new() { Id = 5, Name = "zaměstnanec soukromé společnosti", IsValid = true},
                new() { Id = 6, Name = "bez zaměstnání", IsValid = true},
                new() { Id = 7, Name = "nezjištěno", IsValid = true},
                new() { Id = 8, Name = "kombinace profesí", IsValid = true},
            };

            items.ForEach(item =>
            {
                var ext = extensions.FirstOrDefault(x => x.ProfessionCategoryId == item.Id);
                if (ext is not null)
                {
                    if (!string.IsNullOrEmpty(ext.ProfessionTypeIds))
                    {
                        item.ProfessionTypeIds.AddRange(((string)ext.ProfessionTypeIds).ParseIDs());
                    }
                    if (!string.IsNullOrEmpty(ext.IncomeMainTypeAMLIds))
                    {
                        item.IncomeMainTypeAMLIds.AddRange(((string)ext.IncomeMainTypeAMLIds).ParseIDs());
                    }
                }
            });
            return items;
        });

    public override Task<GenericCodebookWithRdmCodeResponse> ProfessionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItemsWithRdmCode(SqlQueries.ProfessionTypes);

    public override Task<ProofTypesResponse> ProofTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<ProofTypesResponse, ProofTypesResponse.Types.ProofTypeItem>(new ProofTypesResponse(), SqlQueries.ProofTypes);

    public override Task<PropertySettlementsResponse> PropertySettlements(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new PropertySettlementsResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToDynamicList(SqlQueries.PropertySettlements);
            return items.Select(t =>
            {
                var item = new PropertySettlementsResponse.Types.PropertySettlementItem
                {
                    Id = t.Id,
                    IsValid = t.IsValid,
                    Name = t.Name,
                    NameEnglish = t.NameEnglish,
                    Order = t.Order
                };
                if (!string.IsNullOrEmpty(t.MaritalStateId))
                {
                    item.MaritalStateIds.AddRange(((string)t.MaritalStateId).ParseIDs());
                }
                return item;
            }).ToList();
        });

    public override Task<GenericCodebookFullResponse> RealEstatePurchaseTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericFullItems(SqlQueries.RealEstatePurchaseTypes);

    public override Task<GenericCodebookFullResponse> RealEstateTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericFullItems(SqlQueries.RealEstateTypes);

    public override Task<RelationshipCustomerProductTypesResponse> RelationshipCustomerProductTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new RelationshipCustomerProductTypesResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem>(SqlQueries.RelationshipCustomerProductTypes1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.RelationshipCustomerProductTypes2);

            items.ForEach(item =>
            {
                var ext = extensions.FirstOrDefault(x => x.RelationshipCustomerProductTypeId == item.Id);
                item.RdmCode = ext?.RdmCode;
                item.MpDigiApiCode = ext?.MpDigiApiCode;
                item.NameNoby = ext?.NameNoby;
            });
            return items;
        });

    public override Task<GenericCodebookResponse> RepaymentScheduleTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new GenericCodebookResponse(), () =>
        {
            return new List<GenericCodebookResponse.Types.GenericCodebookItem>
            {
                new() { Id = 1, Name = "Anuitní", Code = "A" },
                new() { Id = 2, Name = "Postupné", Code = "P" },
                new() { Id = 3, Name = "Jednorázové - jedna splátka", Code = "OS" },
                new() { Id = 4, Name = "Jednorázové - více splátek", Code = "OM" },
                new() { Id = 5, Name = "Anuitní s mimořádnými splátkami", Code = "AX" },
                new() { Id = 6, Name = "Postupné s mimořádnými splátkami", Code = "PX" },
            };
        });

    public override Task<RiskApplicationTypesResponse> RiskApplicationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new RiskApplicationTypesResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToDynamicList(SqlQueries.RiskApplicationTypes);

            return items.Select(t =>
            {
                var item = new RiskApplicationTypesResponse.Types.RiskApplicationTypeItem
                {
                    Id = t.Id,
                    Name = t.Name,
                    C4MAplCode = t.C4MAplCode,
                    C4MAplTypeId = t.C4MAplTypeId,
                    IsValid = t.IsValid,
                    LoanKindId = t.LoanKindId,
                    LtvFrom = t.LtvFrom,
                    LtvTo = t.LtvTo,
                    MandantId = t.MandantId
                };
                if (!string.IsNullOrEmpty(t.MA))
                {
                    item.MarketingActions.AddRange(((string)t.MA).ParseIDs());
                }
                if (!string.IsNullOrEmpty(t.ProductId))
                {
                    item.ProductTypeId.AddRange(((string)t.ProductId).ParseIDs());
                }
                return item;
            });
        });

    public override Task<SalesArrangementStatesResponse> SalesArrangementStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new SalesArrangementStatesResponse(), () =>
        {
            return FastEnum.GetValues<CIS.Foms.Enums.SalesArrangementStates>()
                .Select(t => new SalesArrangementStatesResponse.Types.SalesArrangementStateItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    StarbuildId = t.GetAttribute<CIS.Core.Attributes.CisStarbuildIdAttribute>()?.StarbuildId,
                    IsDefault = t.HasAttribute<CIS.Core.Attributes.CisDefaultValueAttribute>()
                })
            .ToList();
        });

    public override Task<SalesArrangementTypesResponse> SalesArrangementTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<SalesArrangementTypesResponse, SalesArrangementTypesResponse.Types.SalesArrangementTypeItem>(new SalesArrangementTypesResponse(), SqlQueries.SalesArrangementTypes);

    public override Task<GenericCodebookResponse> SignatureStatesNoby(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new GenericCodebookResponse(), () => new List<Contracts.v1.GenericCodebookResponse.Types.GenericCodebookItem>
        {
            new() { Id = 1, Name ="připraveno", IsValid = true},
            new() { Id = 2, Name ="v procesu", IsValid = true},
            new() { Id = 3, Name ="čeká na sken", IsValid = true},
            new() { Id = 4, Name ="podepsáno", IsValid = true},
            new() { Id = 5, Name ="zrušeno", IsValid = true},
        });

    public override Task<GenericCodebookWithDefaultAndCodeResponse> SignatureTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItemsWithDefaultAndCode<CIS.Foms.Enums.SignatureTypes>();

    public override Task<SigningMethodsForNaturalPersonResponse> SigningMethodsForNaturalPerson(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new SigningMethodsForNaturalPersonResponse(), () => new List<SigningMethodsForNaturalPersonResponse.Types.SigningMethodsForNaturalPersonItem>
        {
            new() { Code = "OFFERED", Order = 4, Name = "Delegovaná metoda podpisu", Description = "deprecated", IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "PHYSICAL", Order = 1, Name = "Ruční podpis", Description = "Fyzický/ruční podpis dokumentu.", IsValid = true, StarbuildEnumId = 1 },
            new() { Code = "DELEGATE", Order = 1, Name = "Přímé bankovnictví", Description = "Přímé bankovnictví - Delegovaná metoda podpisu", IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "PAAT", Order = 1, Name = "KB klíč", IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "INT_CERT_FILE", Order = 2, Name = "Interní certifikát v souboru", IsValid = true, StarbuildEnumId = 2 },
            new() { Code = "APOC", Order = 3, Name = "Automatizovaný Podpis Osobním Certifikátem", IsValid = true, StarbuildEnumId = 2 },
        });

    public override Task<SmsNotificationTypesResponse> SmsNotificationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<SmsNotificationTypesResponse, SmsNotificationTypesResponse.Types.SmsNotificationTypeItem>(new SmsNotificationTypesResponse(), SqlQueries.SmsNotificationTypes);

    public override Task<StatementFrequenciesResponse> StatementFrequencies(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<StatementFrequenciesResponse, StatementFrequenciesResponse.Types.StatementFrequencyItem>(new StatementFrequenciesResponse(), SqlQueries.StatementFrequencies);

    public override Task<GenericCodebookWithDefaultAndCodeResponse> StatementSubscriptionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItemsWithDefaultAndCode(SqlQueries.StatementSubscriptionTypes);

    public override Task<StatementTypesResponse> StatementTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<StatementTypesResponse, StatementTypesResponse.Types.StatementTypeItem>(new StatementTypesResponse(), SqlQueries.StatementTypes);

    public override Task<TinFormatsByCountryResponse> TinFormatsByCountry(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<TinFormatsByCountryResponse, TinFormatsByCountryResponse.Types.TinFormatsByCountryItem>(new TinFormatsByCountryResponse(), SqlQueries.TinFormatsByCountry);

    public override Task<TinNoFillReasonsByCountryResponse> TinNoFillReasonsByCountry(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<TinNoFillReasonsByCountryResponse, TinNoFillReasonsByCountryResponse.Types.TinNoFillReasonsByCountryItem>(new TinNoFillReasonsByCountryResponse(), SqlQueries.TinNoFillReasonsByCountry);

    public override Task<WorkflowConsultationMatrixResponse> WorkflowConsultationMatrix(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new WorkflowConsultationMatrixResponse(), () =>
        {
            var xxdResult = _xxdhf.ExecuteDapperRawSqlToList<(int Kod, string Text)>(SqlQueries.WorkflowConsultationMatrixResponse1);
            var matrix = _selfDb.ExecuteDapperRawSqlToList<(int TaskSubtypeId, int ProcessTypeId, int ProcessPhaseId, bool IsConsultation)>(SqlQueries.WorkflowConsultationMatrixResponse2);

            return xxdResult.Select(t =>
            {
                var item = new Contracts.v1.WorkflowConsultationMatrixResponse.Types.WorkflowConsultationMatrixItem
                {
                    TaskSubtypeId = t.Kod,
                    TaskSubtypeName = t.Text
                };
                item.IsValidFor.AddRange(matrix
                    .Where(x => x.TaskSubtypeId == t.Kod)
                    .Select(x => new Contracts.v1.WorkflowConsultationMatrixResponse.Types.WorkflowConsultationMatrixItem.Types.WorkflowConsultationMatrixItemValidity
                    {
                        ProcessPhaseId = x.ProcessPhaseId,
                        ProcessTypeId = x.ProcessTypeId
                    })
                    .ToList());
                return item;
            })
            .ToList();
        });

    public override Task<GenericCodebookResponse> WorkflowTaskCategories(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.WorkflowTaskCategory>();

    public override Task<GenericCodebookResponse> WorkflowTaskConsultationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxdhf.GetGenericItems(SqlQueries.WorkflowTaskConsultationTypes);

    public override Task<GenericCodebookResponse> WorkflowTaskSigningResponseTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxdhf.GetGenericItems(SqlQueries.WorkflowTaskSigningResponseTypes);

    public override Task<WorkflowTaskStatesResponse> WorkflowTaskStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new WorkflowTaskStatesResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem>(SqlQueries.WorkflowTaskStates1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.WorkflowTaskStates2);

            items.ForEach(item =>
            {
                byte? flag = extensions.FirstOrDefault(t => t.WorkflowTaskStateId == item.Id)?.Flag;
                item.Flag = flag.HasValue ? (WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag)flag : WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag.None;
            });
            return items;
        });

    public override Task<WorkflowTaskStatesNobyResponse> WorkflowTaskStatesNoby(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _selfDb.GetItems<WorkflowTaskStatesNobyResponse, WorkflowTaskStatesNobyResponse.Types.WorkflowTaskStatesNobyItem>(new WorkflowTaskStatesNobyResponse(), SqlQueries.WorkflowTaskStatesNoby);

    public override Task<WorkflowTaskTypesResponse> WorkflowTaskTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new WorkflowTaskTypesResponse(), () =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<WorkflowTaskTypesResponse.Types.WorkflowTaskTypesItem>(SqlQueries.WorkflowTaskTypes1);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(SqlQueries.WorkflowTaskTypes2);

            items.ForEach(item =>
            {
                item.CategoryId = extensions.FirstOrDefault(t => t.WorkflowTaskTypeId == item.Id)?.CategoryId;
            });
            return items;
        });

    public override Task<GenericCodebookResponse> WorkSectors(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItems(SqlQueries.WorkSectors);

    public override Task<GenericCodebookResponse> CovenantTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxdhf.GetGenericItems(SqlQueries.CovenantTypes);

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