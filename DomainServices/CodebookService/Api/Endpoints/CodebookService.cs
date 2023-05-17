using CIS.Core.Data;
using DomainServices.CodebookService.Api.Database;
using DomainServices.CodebookService.Contracts.v1;

using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CodebookService.Api.Endpoints;

[Authorize]
internal sealed class CodebookService
    : Contracts.v1.CodebookService.CodebookServiceBase
{
    public override Task<GenericCodebookResponse> AcademicDegreesAfter(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItems("SELECT KOD 'Id', TEXT 'Name', CAST(1 as bit) 'IsValid' FROM [SBR].[CIS_TITULY_ZA] ORDER BY TEXT ASC");

    public override Task<GenericCodebookResponse> AcademicDegreesBefore(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
    {
        GenericCodebookResponse response = new();
        response.Items.AddRange(new GenericCodebookResponse.Types.GenericCodebookItem[]
        {
            new() { Id = 0, Name = "Neuvedeno", IsValid = true},
            new() { Id = 3, Name = "DR.", IsValid = true},
            new() { Id = 7, Name = "JUDR.", IsValid = true},
            new() { Id = 10, Name = "RNDR.", IsValid = true},
            new() { Id = 20, Name = "MGR.", IsValid = true},
            new() { Id = 47, Name = "ING.", IsValid = true},
            new() { Id = 66, Name = "PROF.", IsValid = true},
            new() { Id = 79, Name = "THMGR", IsValid = true},
            new() { Id = 84, Name = "MVDR.", IsValid = true},
            new() { Id = 90, Name = "PHMR.", IsValid = true},
            new() { Id = 91, Name = "BC.", IsValid = true},
            new() { Id = 92, Name = "MGA.", IsValid = true},
            new() { Id = 94, Name = "PAEDDR", IsValid = true},
            new() { Id = 96, Name = "THDR.", IsValid = true},
            new() { Id = 97, Name = "THLIC.", IsValid = true},
            new() { Id = 100, Name = "DOC.", IsValid = true},
            new() { Id = 148, Name = "MDDr.", IsValid = true},
            new() { Id = 5001, Name = "AKAD.", IsValid = true},
            new() { Id = 5002, Name = "BCA.", IsValid = true},
            new() { Id = 5003, Name = "RSDR.", IsValid = true},
            new() { Id = 5004, Name = "PHDR.", IsValid = true},
            new() { Id = 5005, Name = "ARCH.", IsValid = true},
            new() { Id = 5006, Name = "MUDR.", IsValid = true},
        });
        return Task.FromResult(response);
    }

    public override Task<GenericCodebookWithCodeResponse> AddressTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetGenericItemsWithCode<CIS.Foms.Enums.AddressTypes>();

    public override Task<BankCodesResponse> BankCodes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetItems<BankCodesResponse, BankCodesResponse.Types.BankCodeItem>(new BankCodesResponse(), "SELECT KOD_BANKY 'BankCode', NAZOV_BANKY 'Name', SKRAT_NAZOV_BANKY 'ShortName', SKRATKA_STATU_PRE_IBAN 'State', CASE WHEN SYSDATETIME() <= ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM SBR.CIS_KODY_BANK ORDER BY KOD_BANKY ASC");

    public override Task<CaseStatesResponse> CaseStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Helpers.GetItems(new CaseStatesResponse(), () => FastEnum.GetValues<CIS.Foms.Enums.CaseStates>()
            .Select(t => new CaseStatesResponse.Types.CaseStateItem
            {
                Id = (int)t,
                Code = t.ToString(),
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                IsDefault = t.HasAttribute<CIS.Core.Attributes.CisDefaultValueAttribute>()
            }));

    private readonly IConnectionProvider<IKonsdbDapperConnectionProvider> _konsdb;
    private readonly IConnectionProvider<IXxdHfDapperConnectionProvider> _xxdhf;
    private readonly IConnectionProvider<IXxdDapperConnectionProvider> _xxd;

    public CodebookService(
        IConnectionProvider<IKonsdbDapperConnectionProvider> konsdb, 
        IConnectionProvider<IXxdHfDapperConnectionProvider> xxdhf, 
        IConnectionProvider<IXxdDapperConnectionProvider> xxd)
    {
        _konsdb = konsdb;
        _xxdhf = xxdhf;
        _xxd = xxd;
    }
}