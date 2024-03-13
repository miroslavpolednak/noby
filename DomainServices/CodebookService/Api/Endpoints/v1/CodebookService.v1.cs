using CIS.Infrastructure.Data;
using DomainServices.CodebookService.Api.Database;
using DomainServices.CodebookService.Contracts.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.FeatureManagement;
using SharedTypes;

namespace DomainServices.CodebookService.Api.Endpoints.v1;

[Authorize]
internal partial class CodebookService
    : Contracts.v1.CodebookService.CodebookServiceBase
{
    public override Task<SignatureTypeDetailResponse> SignatureTypeDetails(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<SignatureTypeDetailResponse, SignatureTypeDetailResponse.Types.SignatureTypeDetailItem>();

    public override Task<GenericCodebookResponse> RefinancingStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<SharedTypes.Enums.RefinancingStates>(true);

    public override Task<GenericCodebookResponse> RefixationOfferTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<SharedTypes.Enums.RefixationOfferTypes>();

    public override Task<GenericCodebookResponse> RefinancingTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<SharedTypes.Enums.RefinancingTypes>(true);

    public override Task<GenericCodebookResponse> AcademicDegreesAfter(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> AcademicDegreesBefore(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<AddressTypesResponse> AddressTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
    {
        var items = FastEnum.GetValues<SharedTypes.Enums.AddressTypes>()
           .Where(t => t != SharedTypes.Enums.AddressTypes.Unknown)
           .Select(t => new AddressTypesResponse.Types.AddressTypeItem
           {
               Id = (int)t,
               Code = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? "",
               Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
               IsValid = true,
               IsValidNoby = (int)t is 1 or 2,
               SbJsonValue = (int)t
           })
           .ToList()!;

        return Task.FromResult((new AddressTypesResponse()).AddItems(items));
    }

    public override Task<GenericCodebookResponse> AcvAttachmentCategories(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<BankCodesResponse> BankCodes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<BankCodesResponse, BankCodesResponse.Types.BankCodeItem>();

    public override Task<GenericCodebookResponse> CaseStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<SharedTypes.Enums.CaseStates>(true);

    public override Task<GenericCodebookResponse> ClassificationOfEconomicActivities(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<CollateralTypesResponse> CollateralTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<CollateralTypesResponse, CollateralTypesResponse.Types.CollateralTypeItem>();

    public override Task<ContactTypesResponse> ContactTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetList<ContactTypesResponse.Types.ContactTypeItem>(nameof(ContactTypes), 1);
            var extensions = _db.GetDynamicList(nameof(ContactTypes), 2);
            items.ForEach(item =>
            {
                item.MpDigiApiCode = extensions.FirstOrDefault(t => t.ContactTypeId == item.Id)?.MpDigiApiCode;
            });
            return (new ContactTypesResponse()).AddItems(items);
        });

    public override Task<CountriesResponse> Countries(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<CountriesResponse, CountriesResponse.Types.CountryItem>();

    public override Task<CountryCodePhoneIdcResponse> CountryCodePhoneIdc(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<CountryCodePhoneIdcResponse, CountryCodePhoneIdcResponse.Types.CountryCodePhoneIdcItem>();

    public override Task<CurrenciesResponse> Currencies(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<CurrenciesResponse, CurrenciesResponse.Types.CurrencyItem>();

    public override Task<GenericCodebookResponse> CustomerProfiles(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<SharedTypes.Enums.CustomerProfiles>(true);

    public override Task<CustomerRolesResponse> CustomerRoles(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new CustomerRolesResponse()).AddItems(
            FastEnum
                .GetValues<SharedTypes.Enums.CustomerRoles>()
                .Select(t => new Contracts.v1.CustomerRolesResponse.Types.CustomerRoleItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    RdmCode = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? "",
                    NameNoby = t switch
                    {
                        SharedTypes.Enums.CustomerRoles.Debtor => "Hlavní žadatel",
                        SharedTypes.Enums.CustomerRoles.Codebtor => "Spoludlužník",
                        SharedTypes.Enums.CustomerRoles.Garantor => "Ručitel",
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

        var developersAndProjectsQuery = _db.Sql["DeveloperSearchWithProjects"].Query.Replace("<terms>", termsValues);
        var developersQuery = _db.Sql[nameof(DeveloperSearch)].Query.Replace("<terms>", termsValues);

        var developersAndProjects = await _db.Xxdhf.ExecuteDapperRawSqlToListAsync<DeveloperSearchResponse.Types.DeveloperSearchItem>(developersAndProjectsQuery);
        var developers = await _db.Xxdhf.ExecuteDapperRawSqlToListAsync<DeveloperSearchResponse.Types.DeveloperSearchItem>(developersQuery);

        return (new DeveloperSearchResponse()).AddItems(developersAndProjects.Concat(developers));
    }

    public override Task<DocumentFileTypesResponse> DocumentFileTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new DocumentFileTypesResponse()).AddItems(
            FastEnum
                .GetValues<SharedTypes.Enums.DocumentFileTypes>()
                .Select(t => new DocumentFileTypesResponse.Types.DocumentFileTypeItem()
                {
                    Id = (int)t,
                    DocumenFileType = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    IsPrintingSupported = t == SharedTypes.Enums.DocumentFileTypes.PdfA || t == SharedTypes.Enums.DocumentFileTypes.OpenForm
                })
            )
        );

    public override Task<DocumentOnSATypesResponse> DocumentOnSATypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<DocumentOnSATypesResponse, DocumentOnSATypesResponse.Types.DocumentOnSATypeItem>();

    public override Task<DocumentTemplateTypesResponse> DocumentTemplateTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new DocumentTemplateTypesResponse()).AddItems(
            FastEnum
                .GetValues<SharedTypes.Enums.DocumentFileTypes>()
                .Select(t => new DocumentTemplateTypesResponse.Types.DocumentTemplateTypeItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    ShortName = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? ""
                })
            )
        );

    public override Task<DocumentTemplateVariantsResponse> DocumentTemplateVariants(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<DocumentTemplateVariantsResponse, DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem>();

    public override Task<DocumentTemplateVersionsResponse> DocumentTemplateVersions(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<DocumentTemplateVersionsResponse, DocumentTemplateVersionsResponse.Types.DocumentTemplateVersionItem>();

    public override Task<DocumentTypesResponse> DocumentTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<DocumentTypesResponse, DocumentTypesResponse.Types.DocumentTypeItem>();

    public override Task<DrawingDurationsResponse> DrawingDurations(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<DrawingDurationsResponse, DrawingDurationsResponse.Types.DrawingDurationItem>();

    public override Task<DrawingTypesResponse> DrawingTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new DrawingTypesResponse()).AddItems(
            FastEnum
                .GetValues<SharedTypes.Enums.DrawingTypes>()
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
            var items = _db.GetList<EaCodesMainResponse.Types.EaCodesMainItem>(nameof(EaCodesMain), 1);
            var extensions = _db.GetDynamicList(nameof(EaCodesMain), 2);
            items.ForEach(item =>
            {
                item.IsFormIdRequested = extensions.FirstOrDefault(t => t.EaCodesMainId == item.Id)?.IsFormIdRequested ?? false;
            });
            return (new EaCodesMainResponse()).AddItems(items);
        });

    public override Task<EducationLevelsResponse> EducationLevels(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<EducationLevelsResponse, EducationLevelsResponse.Types.EducationLevelItem>();

    public override Task<GenericCodebookResponse> EmploymentTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<FeesResponse> Fees(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<FeesResponse, FeesResponse.Types.FeeItem>();

    public override Task<FixedRatePeriodsResponse> FixedRatePeriods(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<FixedRatePeriodsResponse, FixedRatePeriodsResponse.Types.FixedRatePeriodItem>();

    public override Task<FormTypesResponse> FormTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<FormTypesResponse, FormTypesResponse.Types.FormTypeItem>();

    public override Task<GendersResponse> Genders(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new GendersResponse()).AddItems(
            FastEnum
                .GetValues<SharedTypes.Enums.Genders>()
                .Where(t => t != SharedTypes.Enums.Genders.Unknown)
                .Select(t => new GendersResponse.Types.GenderItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    KonsDBCode = (int)t,
                    KbCmCode = t == SharedTypes.Enums.Genders.Male ? "M" : "F",
                    StarBuildJsonCode = t == SharedTypes.Enums.Genders.Male ? "M" : "Z"
                })
            )
        );

    public override Task<GetGeneralDocumentListResponse> GetGeneralDocumentList(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<GetGeneralDocumentListResponse, GetGeneralDocumentListResponse.Types.GetGeneralDocumentListItem>();

    public override async Task<GetOperatorResponse> GetOperator(GetOperatorRequest request, ServerCallContext context)
    { 
        return await _db.GetFirstOrDefault<GetOperatorResponse>(new { request.PerformerLogin })
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OperatorNotFound, request.PerformerLogin);
    }

    public override Task<HashAlgorithmsResponse> HashAlgorithms(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new HashAlgorithmsResponse()).AddItems(
            new List<HashAlgorithmsResponse.Types.HashAlgorithmItem>
            {
                new() { Code = "SHA-256", Description = "Secure Hash Algorithm 256-bit" },
                new() { Code = "SHA-384", Description = "Secure Hash Algorithm 384-bit" },
                new() { Code = "SHA-512", Description = "Secure Hash Algorithm 512-bit" },
                new() { Code = "SHA-3", Description = "Secure Hash Algorithm 3" },
            })
        );

    public override Task<HouseholdTypesResponse> HouseholdTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new HouseholdTypesResponse()).AddItems(
            FastEnum
                .GetValues<SharedTypes.Enums.HouseholdTypes>()
                .Select(t => new HouseholdTypesResponse.Types.HouseholdTypeItem()
                {
                    Id = (int)t,
                    Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                    RdmCode = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? "",
                    MaxHouseholdsForSA = t switch
                    {
                        SharedTypes.Enums.HouseholdTypes.Main => 1,
                        SharedTypes.Enums.HouseholdTypes.Codebtor => 1,
                        _ => 0
                    },
                    DocumentTypeId = t switch
                    {
                        SharedTypes.Enums.HouseholdTypes.Main => 4,
                        SharedTypes.Enums.HouseholdTypes.Codebtor => 5,
                        _ => null
                    }
                })
            )
        );

    public override Task<HousingConditionsResponse> HousingConditions(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<HousingConditionsResponse, HousingConditionsResponse.Types.HousingConditionItem>();

    public override Task<ChannelsResponse> Channels(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetList<ChannelsResponse.Types.ChannelItem>(nameof(Channels), 1);
            var extensions = _db.GetDynamicList(nameof(Channels), 2);
            items.ForEach(item =>
            {
                item.RdmCbChannelCode = extensions.FirstOrDefault(t => t.ChannelId == item.Id)?.RdmCbChannelCode ?? "CH0001";
            });
            return (new ChannelsResponse()).AddItems(items);
        });

    public override Task<IdentificationDocumentTypesResponse> IdentificationDocumentTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetList<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem>(nameof(IdentificationDocumentTypes), 1);
            var extensions = _db.GetDynamicList(nameof(IdentificationDocumentTypes), 2);
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
                new() { Id = 1, Code = "KBID", Name = "Identifikátor KB klienta v Customer managementu", MandantId = 2, Category = "Customer" },
                new() { Id = 2, Code = "MPSBID", Name = "PartnerId ve Starbuildu", MandantId = 1, Category = "Customer" },
                new() { Id = 3, Code = "MPEKID", Name = "KlientId v eKmenu", MandantId = 1, Category = "Customer" },
                new() { Id = 4, Code = "KBUID", MandantId = 2, Category = "User" },
                new() { Id = 5, Code = "M04ID", MandantId = 1, Category = "User" },
                new() { Id = 6, Code = "M17ID", MandantId = 1, Category = "User" },
                new() { Id = 7, Code = "BrokerId", MandantId = 2, Category = "User" },
                new() { Id = 8, Code = "MPAD", MandantId = 1, Category = "User" }
            })
        );

    public override Task<GenericCodebookResponse> IncomeForeignTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> IncomeMainTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> IncomeMainTypesAML(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetList<GenericCodebookResponse.Types.GenericCodebookItem>(nameof(IncomeMainTypesAML), 1);
            var extensions = _db.GetDynamicList(nameof(IncomeMainTypesAML), 2);
            items.ForEach(item =>
            {
                item.RdmCode = extensions.FirstOrDefault(t => t.Id == item.Id)?.RdmCode;
            });
            return (new GenericCodebookResponse()).AddItems(items);
        });

    public override Task<GenericCodebookResponse> IncomeOtherTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> JobTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> LegalCapacityRestrictionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() => (new GenericCodebookResponse()).AddItems(
            FastEnum
                .GetValues<SharedTypes.Enums.LegalCapacityRestrictions>()
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
        => Helpers.GetGenericItems<SharedTypes.Enums.LoanInterestRateAnnouncedTypes>(true);

    public override Task<GenericCodebookResponse> LoanKinds(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<LoanPurposesResponse> LoanPurposes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetDynamicList($"{nameof(LoanPurposes)}1");
            var orderExtension = _db.GetDynamicList($"{nameof(LoanPurposes)}2");
            return (new LoanPurposesResponse()).AddItems(items.Select(t =>
            {
                var item = new LoanPurposesResponse.Types.LoanPurposeItem
                {
                    Id = t.Id,
                    C4MId = t.C4MId,
                    IsValid = t.IsValid,
                    MandantId = t.MandantId,
                    Name = t.Name,
                    Order = t.Order,
                    AcvId = t.AcvId,
                    AcvIdPriority = orderExtension.FirstOrDefault(o => o.AcvId == t.AcvId)?.AcvIdPriority
                };
                if (!string.IsNullOrEmpty(t.ProductTypeIds))
                {
                    item.ProductTypeIds.AddRange(((string)t.ProductTypeIds).ParseIDs());
                }
                return item;
            }));
        });

    public override Task<GenericCodebookResponse> Mandants(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<SharedTypes.Enums.Mandants>(true);

    public override Task<GenericCodebookResponse> MaritalStatuses(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetList<GenericCodebookResponse.Types.GenericCodebookItem>(nameof(MaritalStatuses) , 1);
            var extensions = _db.GetDynamicList(nameof(MaritalStatuses), 2);
            items.ForEach(item =>
            {
                item.RdmCode = extensions.FirstOrDefault(t => t.MaritalStatusId == item.Id)?.RDMCode;
            });
            return (new GenericCodebookResponse()).AddItems(items);
        });

    public override Task<GenericCodebookResponse> MarketingActions(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> NetMonthEarnings(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetList<GenericCodebookResponse.Types.GenericCodebookItem>(nameof(NetMonthEarnings), 1);
            var extensions = _db.GetDynamicList(nameof(NetMonthEarnings), 2);
            items.ForEach(item =>
            {
                item.RdmCode = extensions.FirstOrDefault(t => t.NetMonthEarningId == item.Id)?.RdmCode;
            });
            return (new GenericCodebookResponse()).AddItems(items);
        });

    public override Task<GenericCodebookResponse> ObligationCorrectionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<ObligationLaExposuresResponse> ObligationLaExposures(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<ObligationLaExposuresResponse, ObligationLaExposuresResponse.Types.ObligationLaExposureItem>();

    public override Task<ObligationTypesResponse> ObligationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetList<ObligationTypesResponse.Types.ObligationTypeItem>(nameof(ObligationTypes), 1);
            var extensions = _db.GetDynamicList(nameof(ObligationTypes), 2);
            items.ForEach(item =>
            {
                item.ObligationProperty = extensions.FirstOrDefault(t => t.ObligationTypeId == item.Id)?.ObligationProperty;
            });
            return (new ObligationTypesResponse()).AddItems(items);
        });

    public override Task<PaymentDaysResponse> PaymentDays(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<PaymentDaysResponse, PaymentDaysResponse.Types.PaymentDayItem>();

    public override Task<GenericCodebookResponse> PayoutTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItems<SharedTypes.Enums.PayoutTypes>();

    public override Task<PostCodesResponse> PostCodes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<PostCodesResponse, PostCodesResponse.Types.PostCodeItem>();

    public override async Task<ProductTypesResponse> ProductTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
    {
        bool blueBang = await _featureManager.IsEnabledAsync(FeatureFlagsConstants.BlueBang);

        return CodebookMemoryCache.GetOrCreate("ProductTypes", () =>
        {
            var items = _db.GetList<ProductTypesResponse.Types.ProductTypeItem>(nameof(ProductTypes), 1);
            var extensions = _db.GetDynamicList(nameof(ProductTypes), 2);
            var loanKinds = _db.Xxdhf.GetOrCreateCachedResponse<GenericCodebookResponse, GenericCodebookResponse.Types.GenericCodebookItem>(_db.Sql[nameof(LoanKinds)].Query, nameof(LoanKinds))
                .Items
                .Select(t => t.Id)
                .ToArray();

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

            // pridat natvrdo stavebni sporeni
            if (blueBang)
            {
                items.Add(new ProductTypesResponse.Types.ProductTypeItem
                {
                    KonsDbLoanType = 1,
                    Id = 20000,
                    PcpProductId = "220614142026340857",
                    Name = "Stavební spoření",
                    IsValid = true,
                    Order = 6,
                    MandantId = 1
                });
            }

            return (new ProductTypesResponse()).AddItems(items);
        });
    }
    
    public override Task<GenericCodebookResponse> ProfessionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<ProofTypesResponse> ProofTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<ProofTypesResponse, ProofTypesResponse.Types.ProofTypeItem>();

    public override Task<PropertySettlementsResponse> PropertySettlements(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetDynamicList(nameof(PropertySettlements));
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

    
    public override Task<RelationshipCustomerProductTypesResponse> RelationshipCustomerProductTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(() =>
        {
            var items = _db.GetList<RelationshipCustomerProductTypesResponse.Types.RelationshipCustomerProductTypeItem>(nameof(RelationshipCustomerProductTypes), 1);
            var extensions = _db.GetDynamicList(nameof(RelationshipCustomerProductTypes), 2);

            items.ForEach(item =>
            {
                var ext = extensions.FirstOrDefault(x => x.RelationshipCustomerProductTypeId == item.Id);
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
            var items = _db.GetDynamicList(nameof(RiskApplicationTypes));

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
                .GetValues<SharedTypes.Enums.SalesArrangementStates>()
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
        => _db.GetItems<SalesArrangementTypesResponse, SalesArrangementTypesResponse.Types.SalesArrangementTypeItem>();

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

    public override async Task<GenericCodebookResponse> SignatureTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
    {
        var result = await Helpers.GetGenericItems<SharedTypes.Enums.SignatureTypes>(true);

        // zmena pro flag
        int defaultType = await _featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ElectronicSigning)
            ? (int)SharedTypes.Enums.SignatureTypes.Electronic
            : (int)SharedTypes.Enums.SignatureTypes.Paper;

        result.Items.First(t => t.Id == defaultType).IsDefault = true;

        return result;
    }
    
    public override Task<SmsNotificationTypesResponse> SmsNotificationTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<SmsNotificationTypesResponse, SmsNotificationTypesResponse.Types.SmsNotificationTypeItem>();

    public override Task<StatementFrequenciesResponse> StatementFrequencies(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<StatementFrequenciesResponse, StatementFrequenciesResponse.Types.StatementFrequencyItem>();

    public override Task<GenericCodebookResponse> StatementSubscriptionTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<StatementTypesResponse> StatementTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<StatementTypesResponse, StatementTypesResponse.Types.StatementTypeItem>();

    public override Task<TinFormatsByCountryResponse> TinFormatsByCountry(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<TinFormatsByCountryResponse, TinFormatsByCountryResponse.Types.TinFormatsByCountryItem>();

    public override Task<TinNoFillReasonsByCountryResponse> TinNoFillReasonsByCountry(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<TinNoFillReasonsByCountryResponse, TinNoFillReasonsByCountryResponse.Types.TinNoFillReasonsByCountryItem>();

    public override Task<GenericCodebookResponse> WorkSectors(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> CovenantTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<SigningMethodsResponse> SigningMethods(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<SigningMethodsResponse, SigningMethodsResponse.Types.SigningMethodsItem>();

    private readonly Database.DatabaseAggregate _db;
    private readonly ExternalServices.AcvEnumService.V1.IAcvEnumServiceClient _acvEnumService;
    private readonly ExternalServices.RDM.V1.IRDMClient _rdmClient;
    private readonly IFeatureManager _featureManager;

    public CodebookService(
        IFeatureManager featureManager,
        Database.DatabaseAggregate db,
        ExternalServices.RDM.V1.IRDMClient rdmClient,
        ExternalServices.AcvEnumService.V1.IAcvEnumServiceClient acvEnumService)
    {
        _featureManager = featureManager;
        _db = db;
        _acvEnumService = acvEnumService;
        _rdmClient = rdmClient;
    }
}