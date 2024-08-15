using DomainServices.UserService.Contracts;

namespace DomainServices.UserService.Api.Endpoints.v1.GetUserRIPAttributes;

internal sealed class GetUserRIPAttributesHandler(IConnectionProvider _db)
    : IRequestHandler<GetUserRIPAttributesRequest, UserRIPAttributes>
{
    public async Task<UserRIPAttributes> Handle(GetUserRIPAttributesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // vytahnout info o uzivateli z DB
            var dbIdentity = await _db.ExecuteDapperStoredProcedureFirstOrDefaultAsync<dynamic>(
                "[dbo].[p_GetPersonHF_RIP]",
                new { identity = request.Identity, identityScheme = request.IdentityScheme },
                cancellationToken)
                ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.UserNotFound, $"{request.IdentityScheme}={request.Identity}");

            return new UserRIPAttributes()
            {
                PersonId = dbIdentity.PersonId,
                DealerCompanyId = dbIdentity.DealerCompanyId,
                PersonJobPostId = dbIdentity.PersonJobPostId ?? "",
                PersonOrgUnitId = dbIdentity.PersonOrgUnitId ?? "",
                PersonOrgUnitName = dbIdentity.PersonOrgUnitName ?? "",
                PersonSurname = dbIdentity.PersonSurname ?? "",
                Company = dbIdentity.Company ?? "",
                BrokerId = dbIdentity.BrokerId
            };
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 50000)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.IdentitySchemeNotExist, request.IdentityScheme);
        }
    }
}
