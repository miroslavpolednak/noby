using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.CodebookService.Api.Database;
using DomainServices.CodebookService.Contracts.v1;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CodebookService.Api.Endpoints;

[Authorize]
internal sealed class CodebookService
    : Contracts.v1.CodebookService.CodebookServiceBase
{
    public override Task<GenericCodebookResponse> AcademicDegreesAfter(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<GenericCodebookResponse> AcademicDegreesBefore(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_selfDb);

    public override Task<GenericCodebookResponse> AddressTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.AddressTypes>(true);

    public override Task<BankCodesResponse> BankCodes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<BankCodesResponse, BankCodesResponse.Types.BankCodeItem>(_xxd);

    public override Task<GenericCodebookResponse> CaseStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.CaseStates>(true);

    public override Task<GenericCodebookResponse> ClassificationOfEconomicActivities(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<CollateralTypesResponse> CollateralTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<CollateralTypesResponse, CollateralTypesResponse.Types.CollateralTypeItem>(_xxd);

    public override Task<ContactTypesResponse> ContactTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<ContactTypesResponse.Types.ContactTypeItem>(_sql[nameof(ContactTypes) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(ContactTypes) + "2"]);
            items.ForEach(item =>
            {
                item.MpDigiApiCode = extensions.FirstOrDefault(t => t.ContactTypeId == item.Id)?.MpDigiApiCode;
            });
            return (new ContactTypesResponse()).AddItems(items);
        });

    public override Task<CountriesResponse> Countries(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<CountriesResponse, CountriesResponse.Types.CountryItem>(_xxd);

    public override Task<CountryCodePhoneIdcResponse> CountryCodePhoneIdc(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new CountryCodePhoneIdcResponse()).AddItems(
            new List<Contracts.v1.CountryCodePhoneIdcResponse.Types.CountryCodePhoneIdcItem>
            {
                new() { Id = "AD+376", Name = "AD", Idc = "+376",  IsValid = true, IsPriority = false, IsDefault = false },
                new() { Id = "AE+971", Name = "AE", Idc = "+971",  IsValid = true, IsPriority = false, IsDefault = false },
                new() { Id = "AF+93", Name = "AF", Idc = "+93",  IsValid = true, IsPriority = false, IsDefault = false },
                new() { Id = "AG+1268", Name = "AG", Idc = "+1268",  IsValid = true, IsPriority = false, IsDefault = false },
                new() { Id = "AI+1264", Name = "AI", Idc = "+1264",  IsValid = true, IsPriority = false, IsDefault = false },
                new() { Id = "CZ+420", Name = "CZ", Idc = "+420",  IsValid = true, IsPriority = true, IsDefault = true },
            })
        );

    public override Task<CurrenciesResponse> Currencies(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<CurrenciesResponse, CurrenciesResponse.Types.CurrencyItem>(_xxd);

    public override Task<GenericCodebookResponse> CustomerProfiles(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.CustomerProfiles>(true);

    public override Task<CustomerRolesResponse> CustomerRoles(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new CustomerRolesResponse()).AddItems(
            FastEnum
                .GetValues<CIS.Foms.Enums.CustomerRoles>()
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
                }))
        );

    public override async Task<DeveloperSearchResponse> DeveloperSearch(DeveloperSearchRequest request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Term))
            return new DeveloperSearchResponse();

        var terms = request.Term.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var termsValues = String.Join(",", terms.Select(t => $"('{t}')"));

        var developersAndProjectsQuery = _sql["DeveloperSearchWithProjects"].Replace("<terms>", termsValues);
        var developersQuery = _sql[nameof(DeveloperSearch)].Replace("<terms>", termsValues);

        var developersAndProjects = await _xxd.ExecuteDapperRawSqlToListAsync<DeveloperSearchResponse.Types.DeveloperSearchItem>(developersAndProjectsQuery);
        var developers = await _xxd.ExecuteDapperRawSqlToListAsync<DeveloperSearchResponse.Types.DeveloperSearchItem>(developersQuery);

        return (new DeveloperSearchResponse()).AddItems(developersAndProjects.Concat(developers));
    }

    public override Task<DocumentFileTypesResponse> DocumentFileTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new DocumentFileTypesResponse()).AddItems(
            FastEnum
                .GetValues<CIS.Foms.Enums.DocumentFileType>()
                .Select(t => new DocumentFileTypesResponse.Types.DocumentFileTypeItem()
                {
                    Id = (int)t,
                    DocumenFileType = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    IsPrintingSupported = t == CIS.Foms.Enums.DocumentFileType.PdfA || t == CIS.Foms.Enums.DocumentFileType.OpenForm
                })
            )
        );

    public override Task<DocumentOnSATypesResponse> DocumentOnSATypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<DocumentOnSATypesResponse, DocumentOnSATypesResponse.Types.DocumentOnSATypeItem>(_selfDb);

    public override Task<DocumentTemplateTypesResponse> DocumentTemplateTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new DocumentTemplateTypesResponse()).AddItems(
            FastEnum
                .GetValues<CIS.Foms.Enums.DocumentFileType>()
                .Select(t => new DocumentTemplateTypesResponse.Types.DocumentTemplateTypeItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    ShortName = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? ""
                })
            )
        );

    public override Task<DocumentTemplateVariantsResponse> DocumentTemplateVariants(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<DocumentTemplateVariantsResponse, DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem>(_selfDb);

    public override Task<DocumentTemplateVersionsResponse> DocumentTemplateVersions(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<DocumentTemplateVersionsResponse, DocumentTemplateVersionsResponse.Types.DocumentTemplateVersionItem>(_selfDb);

    public override Task<DocumentTypesResponse> DocumentTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<DocumentTypesResponse, DocumentTypesResponse.Types.DocumentTypeItem>(_selfDb);

    public override Task<DrawingDurationsResponse> DrawingDurations(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<DrawingDurationsResponse, DrawingDurationsResponse.Types.DrawingDurationItem>(_xxd);

    public override Task<DrawingTypesResponse> DrawingTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new DrawingTypesResponse()).AddItems(
            FastEnum
                .GetValues<CIS.Foms.Enums.DrawingTypes>()
                .Select(t => new DrawingTypesResponse.Types.DrawingTypeItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    StarbuildId = t.GetAttribute<CIS.Core.Attributes.CisStarbuildIdAttribute>()?.StarbuildId
                })
            )
        );

    public override Task<EaCodesMainResponse> EaCodesMain(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _selfDb.ExecuteDapperRawSqlToList<EaCodesMainResponse.Types.EaCodesMainItem>(_sql[nameof(EaCodesMain) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(EaCodesMain) + "2"]);
            items.ForEach(item =>
            {
                item.IsFormIdRequested = extensions.FirstOrDefault(t => t.EaCodesMainId == item.Id)?.IsFormIdRequested ?? false;
            });
            return (new EaCodesMainResponse()).AddItems(items);
        });

    public override Task<EducationLevelsResponse> EducationLevels(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<EducationLevelsResponse, EducationLevelsResponse.Types.EducationLevelItem>(_xxd);

    public override Task<GenericCodebookResponse> EmploymentTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<FeesResponse> Fees(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<FeesResponse, FeesResponse.Types.FeeItem>(_xxd);

    public override Task<FixedRatePeriodsResponse> FixedRatePeriods(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<FixedRatePeriodsResponse, FixedRatePeriodsResponse.Types.FixedRatePeriodItem>(_xxd);

    public override Task<FormTypesResponse> FormTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<FormTypesResponse, FormTypesResponse.Types.FormTypeItem>(_xxd);

    public override Task<GendersResponse> Genders(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new GendersResponse()).AddItems(
            FastEnum
                .GetValues<CIS.Foms.Enums.Genders>()
                .Select(t => new GendersResponse.Types.GenderItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    KonsDBCode = (int)t,
                    KbCmCode = t == CIS.Foms.Enums.Genders.Male ? "M" : "F",
                    StarBuildJsonCode = t == CIS.Foms.Enums.Genders.Male ? "M" : "Z"
                })
            )
        );

    public override async Task<GetDeveloperResponse> GetDeveloper(GetDeveloperRequest request, ServerCallContext context)
    {
        return (await _xxd.ExecuteDapperFirstOrDefaultAsync<GetDeveloperResponse>(_sql[nameof(GetDeveloper)], new { request.DeveloperId }))
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DeveloperNotFound, request.DeveloperId);
    }

    public override async Task<GetDeveloperProjectResponse> GetDeveloperProject(GetDeveloperProjectRequest request, ServerCallContext context)
    {
        return (await _xxd.ExecuteDapperFirstOrDefaultAsync<GetDeveloperProjectResponse>(_sql[nameof(GetDeveloperProject)], new { request.DeveloperProjectId, request.DeveloperId }))
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DeveloperProjectNotFound, request.DeveloperProjectId);
    }

    public override Task<GetGeneralDocumentListResponse> GetGeneralDocumentList(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<GetGeneralDocumentListResponse, GetGeneralDocumentListResponse.Types.GetGeneralDocumentListItem>(_selfDb);

    public override async Task<GetOperatorResponse> GetOperator(GetOperatorRequest request, ServerCallContext context)
    {
        using var connection = _xxd.Create();
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<GetOperatorResponse>(_sql[nameof(GetOperator)], new { request.PerformerLogin });
    }
    
    public override Task<HouseholdTypesResponse> HouseholdTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new HouseholdTypesResponse()).AddItems(
            FastEnum
                .GetValues<CIS.Foms.Enums.HouseholdTypes>()
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
            )
        );

    public override Task<HousingConditionsResponse> HousingConditions(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<HousingConditionsResponse, HousingConditionsResponse.Types.HousingConditionItem>(_xxd);

    public override Task<ChannelsResponse> Channels(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<ChannelsResponse.Types.ChannelItem>(_sql[nameof(Channels) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(Channels) + "2"]);
            items.ForEach(item =>
            {
                item.RdmCbChannelCode = extensions.FirstOrDefault(t => t.ChannelId == item.Id)?.RdmCbChannelCode;
            });
            return (new ChannelsResponse()).AddItems(items);
        });

    public override Task<IdentificationDocumentTypesResponse> IdentificationDocumentTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem>(_sql[nameof(IdentificationDocumentTypes) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(IdentificationDocumentTypes) + "2"]);
            items.ForEach(item =>
            {
                item.MpDigiApiCode = extensions.FirstOrDefault(t => t.IdentificationDocumentTypeId == item.Id)?.MpDigiApiCode;
            });
            return (new IdentificationDocumentTypesResponse()).AddItems(items);
        });

    public override Task<GenericCodebookResponse> IdentificationSubjectMethods(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new GenericCodebookResponse()).AddItems(
            new List<GenericCodebookResponse.Types.GenericCodebookItem>
            {
                new() { Id = 1, Name = "za fyzické přítomnosti", IsValid = true },
                new() { Id = 3, Name = "ověření notářem, krajským nebo obecním úřadem", IsValid = true },
                new() { Id = 8, Name = "zástupce MPSS", IsValid = true }
            })
        );

    public override Task<IdentitySchemesResponse> IdentitySchemes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new IdentitySchemesResponse()).AddItems(
            new List<IdentitySchemesResponse.Types.IdentitySchemeItem>
            {
                new() { Id = 1, Code = "KBID", Name = "Identifikátor KB klienta v Customer managementu", MandantId = 2, Category = "Customer", ChannelId = null },
                new() { Id = 2, Code = "MPSBID", Name = "PartnerId ve Starbuildu", MandantId = 1, Category = "Customer", ChannelId = null },
                new() { Id = 3, Code = "MPEKID", Name = "KlientId v eKmenu", MandantId = 1, Category = "Customer", ChannelId = null },
                new() { Id = 4, Code = "KBUID", MandantId = 2, Category = "User", ChannelId = 4 },
                new() { Id = 5, Code = "M04ID", MandantId = 1, Category = "User", ChannelId = 1 },
                new() { Id = 6, Code = "M17ID", MandantId = 1, Category = "User", ChannelId = 1 },
                new() { Id = 7, Code = "BrokerId", MandantId = 2, Category = "User", ChannelId = 6 },
                new() { Id = 8, Code = "MPAD", MandantId = 1, Category = "User", ChannelId = 1 }
            })
        );

    public override Task<GenericCodebookResponse> IncomeForeignTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<GenericCodebookResponse> IncomeMainTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<GenericCodebookResponse> IncomeMainTypesAML(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<GenericCodebookResponse.Types.GenericCodebookItem>(_sql[nameof(IncomeMainTypesAML) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(IncomeMainTypesAML) + "2"]);
            items.ForEach(item =>
            {
                item.RdmCode = extensions.FirstOrDefault(t => t.Id == item.Id)?.RdmCode;
            });
            return (new GenericCodebookResponse()).AddItems(items);
        });

    public override Task<GenericCodebookResponse> IncomeOtherTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<GenericCodebookResponse> JobTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<GenericCodebookResponse> LegalCapacityRestrictionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new GenericCodebookResponse()).AddItems(
            FastEnum
                .GetValues<CIS.Foms.Enums.LegalCapacityRestrictions>()
                .Select(t => new GenericCodebookResponse.Types.GenericCodebookItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    Description = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Description ?? "",
                    RdmCode = t.ToString()
                })
            )
        );

    public override Task<GenericCodebookResponse> LoanInterestRateAnnouncedTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.LoanInterestRateAnnouncedTypes>(true);

    public override Task<GenericCodebookResponse> LoanKinds(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<LoanPurposesResponse> LoanPurposes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToDynamicList(_sql[nameof(LoanPurposes)]);
            return (new LoanPurposesResponse()).AddItems(items.Select(t =>
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
            }));
        });

    public override Task<GenericCodebookResponse> Mandants(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.Mandants>(true);

    public override Task<GenericCodebookResponse> MaritalStatuses(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<GenericCodebookResponse.Types.GenericCodebookItem>(_sql[nameof(MaritalStatuses) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(MaritalStatuses) + "2"]);
            items.ForEach(item =>
            {
                item.RdmCode = extensions.FirstOrDefault(t => t.MaritalStatusId == item.Id)?.RDMCode;
            });
            return (new GenericCodebookResponse()).AddItems(items);
        });

    public override Task<GenericCodebookResponse> MarketingActions(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<GenericCodebookResponse> Nationalities(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_konsdb);

    public override Task<GenericCodebookResponse> NetMonthEarnings(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<GenericCodebookResponse.Types.GenericCodebookItem>(_sql[nameof(NetMonthEarnings) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(NetMonthEarnings) + "2"]);
            items.ForEach(item =>
            {
                item.RdmCode = extensions.FirstOrDefault(t => t.NetMonthEarningId == item.Id)?.RdmCode;
            });
            return (new GenericCodebookResponse()).AddItems(items);
        });

    public override Task<GenericCodebookResponse> ObligationCorrectionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<ObligationLaExposuresResponse> ObligationLaExposures(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<ObligationLaExposuresResponse, ObligationLaExposuresResponse.Types.ObligationLaExposureItem>(_xxd);

    public override Task<ObligationTypesResponse> ObligationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<ObligationTypesResponse.Types.ObligationTypeItem>(_sql[nameof(ObligationTypes) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(ObligationTypes) + "2"]);
            items.ForEach(item =>
            {
                item.ObligationProperty = extensions.FirstOrDefault(t => t.ObligationTypeId == item.Id)?.ObligationProperty;
            });
            return (new ObligationTypesResponse()).AddItems(items);
        });

    public override Task<PaymentDaysResponse> PaymentDays(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<PaymentDaysResponse, PaymentDaysResponse.Types.PaymentDayItem>(_xxd);

    public override Task<GenericCodebookResponse> PayoutTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.PayoutTypes>();

    public override Task<PostCodesResponse> PostCodes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<PostCodesResponse, PostCodesResponse.Types.PostCodeItem>(_xxd);

    public override Task<ProductTypesResponse> ProductTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(, () =>
        {
            var items = _xxdhf.ExecuteDapperRawSqlToList<ProductTypesResponse.Types.ProductTypeItem>(_sql[nameof(ProductTypes) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(ProductTypes) + "2"]);
            var loanKinds = _xxd.GetOrCreateCachedResponse<GenericCodebookResponse.Types.GenericCodebookItem>(_sql[nameof(LoanKinds)], nameof(LoanKinds)).Select(t => t.Id).ToArray();

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

            return (new ProductTypesResponse()).AddItems(items);
        });

    public override Task<ProfessionCategoriesResponse> ProfessionCategories(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(ProfessionCategories)]);

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
            return (new ProfessionCategoriesResponse()).AddItems(items);
        });

    public override Task<GenericCodebookResponse> ProfessionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<ProofTypesResponse> ProofTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<ProofTypesResponse, ProofTypesResponse.Types.ProofTypeItem>(_xxd);

    public override Task<PropertySettlementsResponse> PropertySettlements(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToDynamicList(_sql[nameof(PropertySettlements)]);
            return (new PropertySettlementsResponse()).AddItems(
                items.Select(t =>
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
                })
            );
        });

    public override Task<GenericCodebookResponse> RealEstatePurchaseTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<GenericCodebookResponse> RealEstateTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<RelationshipCustomerProductTypesResponse> RelationshipCustomerProductTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem>(_sql[nameof(RelationshipCustomerProductTypes) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(RelationshipCustomerProductTypes) + "2"]);

            items.ForEach(item =>
            {
                var ext = extensions.FirstOrDefault(x => x.RelationshipCustomerProductTypeId == item.Id);
                item.RdmCode = ext?.RdmCode;
                item.MpDigiApiCode = ext?.MpDigiApiCode;
                item.NameNoby = ext?.NameNoby;
            });
            return (new RelationshipCustomerProductTypesResponse()).AddItems(items);
        });

    public override Task<GenericCodebookResponse> RepaymentScheduleTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new GenericCodebookResponse()).AddItems(
            new List<GenericCodebookResponse.Types.GenericCodebookItem>
            {
                new() { Id = 1, Name = "Anuitní", Code = "A" },
                new() { Id = 2, Name = "Postupné", Code = "P" },
                new() { Id = 3, Name = "Jednorázové - jedna splátka", Code = "OS" },
                new() { Id = 4, Name = "Jednorázové - více splátek", Code = "OM" },
                new() { Id = 5, Name = "Anuitní s mimořádnými splátkami", Code = "AX" },
                new() { Id = 6, Name = "Postupné s mimořádnými splátkami", Code = "PX" },
            }
        ));

    public override Task<RiskApplicationTypesResponse> RiskApplicationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToDynamicList(_sql[nameof(RiskApplicationTypes)]);

            return (new RiskApplicationTypesResponse()).AddItems(items.Select(t =>
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
            }));
        });

    public override Task<SalesArrangementStatesResponse> SalesArrangementStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new SalesArrangementStatesResponse()).AddItems(
            FastEnum
                .GetValues<CIS.Foms.Enums.SalesArrangementStates>()
                .Select(t => new SalesArrangementStatesResponse.Types.SalesArrangementStateItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    StarbuildId = t.GetAttribute<CIS.Core.Attributes.CisStarbuildIdAttribute>()?.StarbuildId,
                    IsDefault = t.HasAttribute<CIS.Core.Attributes.CisDefaultValueAttribute>()
                })
            )
        );

    public override Task<SalesArrangementTypesResponse> SalesArrangementTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<SalesArrangementTypesResponse, SalesArrangementTypesResponse.Types.SalesArrangementTypeItem>(_selfDb);

    public override Task<GenericCodebookResponse> SignatureStatesNoby(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new GenericCodebookResponse()).AddItems(
            new List<Contracts.v1.GenericCodebookResponse.Types.GenericCodebookItem>
            {
                new() { Id = 1, Name ="připraveno", IsValid = true},
                new() { Id = 2, Name ="v procesu", IsValid = true},
                new() { Id = 3, Name ="čeká na sken", IsValid = true},
                new() { Id = 4, Name ="podepsáno", IsValid = true},
                new() { Id = 5, Name ="zrušeno", IsValid = true},
            }
        ));

    public override Task<GenericCodebookResponse> SignatureTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.SignatureTypes>(true);

    public override Task<SigningMethodsForNaturalPersonResponse> SigningMethodsForNaturalPerson(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new SigningMethodsForNaturalPersonResponse()).AddItems(
            new List<SigningMethodsForNaturalPersonResponse.Types.SigningMethodsForNaturalPersonItem>
            {
                new() { Code = "OFFERED", Order = 4, Name = "Delegovaná metoda podpisu", Description = "deprecated", IsValid = true, StarbuildEnumId = 2 },
                new() { Code = "PHYSICAL", Order = 1, Name = "Ruční podpis", Description = "Fyzický/ruční podpis dokumentu.", IsValid = true, StarbuildEnumId = 1 },
                new() { Code = "DELEGATE", Order = 1, Name = "Přímé bankovnictví", Description = "Přímé bankovnictví - Delegovaná metoda podpisu", IsValid = true, StarbuildEnumId = 2 },
                new() { Code = "PAAT", Order = 1, Name = "KB klíč", IsValid = true, StarbuildEnumId = 2 },
                new() { Code = "INT_CERT_FILE", Order = 2, Name = "Interní certifikát v souboru", IsValid = true, StarbuildEnumId = 2 },
                new() { Code = "APOC", Order = 3, Name = "Automatizovaný Podpis Osobním Certifikátem", IsValid = true, StarbuildEnumId = 2 },
            }
        ));

    public override Task<SmsNotificationTypesResponse> SmsNotificationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<SmsNotificationTypesResponse, SmsNotificationTypesResponse.Types.SmsNotificationTypeItem>(_selfDb);

    public override Task<StatementFrequenciesResponse> StatementFrequencies(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<StatementFrequenciesResponse, StatementFrequenciesResponse.Types.StatementFrequencyItem>(_xxd);

    public override Task<GenericCodebookResponse> StatementSubscriptionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<StatementTypesResponse> StatementTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<StatementTypesResponse, StatementTypesResponse.Types.StatementTypeItem>(_xxd);

    public override Task<TinFormatsByCountryResponse> TinFormatsByCountry(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<TinFormatsByCountryResponse, TinFormatsByCountryResponse.Types.TinFormatsByCountryItem>(_selfDb);

    public override Task<TinNoFillReasonsByCountryResponse> TinNoFillReasonsByCountry(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<TinNoFillReasonsByCountryResponse, TinNoFillReasonsByCountryResponse.Types.TinNoFillReasonsByCountryItem>(_selfDb);

    public override Task<WorkflowConsultationMatrixResponse> WorkflowConsultationMatrix(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var xxdResult = _xxdhf.ExecuteDapperRawSqlToList<(int Kod, string Text)>(_sql[nameof(WorkflowConsultationMatrixResponse) + "1"]);
            var matrix = _selfDb.ExecuteDapperRawSqlToList<(int TaskSubtypeId, int ProcessTypeId, int ProcessPhaseId, bool IsConsultation)>(_sql[nameof(WorkflowConsultationMatrixResponse) + "2"]);

            return (new WorkflowConsultationMatrixResponse()).AddItems(
                xxdResult.Select(t =>
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
            );
        });

    public override Task<GenericCodebookResponse> WorkflowTaskCategories(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<CIS.Foms.Enums.WorkflowTaskCategory>();

    public override Task<GenericCodebookResponse> WorkflowTaskConsultationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxdhf);

    public override Task<GenericCodebookResponse> WorkflowTaskSigningResponseTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxdhf);

    public override Task<WorkflowTaskStatesResponse> WorkflowTaskStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem>(_sql[nameof(WorkflowTaskStates) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(WorkflowTaskStates) + "2"]);

            items.ForEach(item =>
            {
                byte? flag = extensions.FirstOrDefault(t => t.WorkflowTaskStateId == item.Id)?.Flag;
                item.Flag = flag.HasValue ? (WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag)flag : WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag.None;
            });
            return (new WorkflowTaskStatesResponse()).AddItems(items);
        });

    public override Task<WorkflowTaskStatesNobyResponse> WorkflowTaskStatesNoby(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetItems<WorkflowTaskStatesNobyResponse, WorkflowTaskStatesNobyResponse.Types.WorkflowTaskStatesNobyItem>(_selfDb);

    public override Task<WorkflowTaskTypesResponse> WorkflowTaskTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _xxd.ExecuteDapperRawSqlToList<WorkflowTaskTypesResponse.Types.WorkflowTaskTypesItem>(_sql[nameof(WorkflowTaskTypes) + "1"]);
            var extensions = _selfDb.ExecuteDapperRawSqlToDynamicList(_sql[nameof(WorkflowTaskTypes) + "2"]);

            items.ForEach(item =>
            {
                item.CategoryId = extensions.FirstOrDefault(t => t.WorkflowTaskTypeId == item.Id)?.CategoryId;
            });
            return (new WorkflowTaskTypesResponse()).AddItems(items);
        });

    public override Task<GenericCodebookResponse> WorkSectors(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxd);

    public override Task<GenericCodebookResponse> CovenantTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _sql.GetGenericItems(_xxdhf);

    private readonly Database.SqlQueryCollection _sql;
    private readonly IConnectionProvider _selfDb;
    private readonly IConnectionProvider<IKonsdbDapperConnectionProvider> _konsdb;
    private readonly IConnectionProvider<IXxdHfDapperConnectionProvider> _xxdhf;
    private readonly IConnectionProvider<IXxdDapperConnectionProvider> _xxd;

    public CodebookService(
        IConnectionProvider selfDb,
        Database.SqlQueryCollection sql,
        IConnectionProvider<IKonsdbDapperConnectionProvider> konsdb, 
        IConnectionProvider<IXxdHfDapperConnectionProvider> xxdhf, 
        IConnectionProvider<IXxdDapperConnectionProvider> xxd)
    {
        _sql = sql;
        _selfDb = selfDb;
        _konsdb = konsdb;
        _xxdhf = xxdhf;
        _xxd = xxd;
    }
}