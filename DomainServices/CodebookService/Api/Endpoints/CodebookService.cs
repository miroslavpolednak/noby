using CIS.Core.Data;
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

            var developersAndProjects = _xxd.ExecuteDapperRawSqlToList<Contracts.v1.DeveloperSearchResponse.Types.DeveloperSearchItem>(developersAndProjectsQuery);
            var developers = _xxd.ExecuteDapperRawSqlToList<Contracts.v1.DeveloperSearchResponse.Types.DeveloperSearchItem>(developersQuery);

            return developersAndProjects.Concat(developers).ToList();
        });
    }


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