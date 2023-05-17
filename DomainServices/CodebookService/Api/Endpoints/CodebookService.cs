using CIS.Core.Data;
using DomainServices.CodebookService.Api.Database;
using DomainServices.CodebookService.Contracts.v1;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CodebookService.Api.Endpoints;

[Authorize]
internal sealed class CodebookService
    : Contracts.v1.CodebookService.CodebookServiceBase
{
    public override Task<GenericCodebookItemResponse> AcademicDegreesAfter(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _xxd.GetGenericItems("SELECT KOD 'Id', TEXT 'Name', CAST(1 as bit) 'IsValid' FROM [SBR].[CIS_TITULY_ZA] ORDER BY TEXT ASC");

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