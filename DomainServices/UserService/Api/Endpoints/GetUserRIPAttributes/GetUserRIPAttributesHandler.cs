using DomainServices.UserService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api.Endpoints.GetUserRIPAttributes;

internal class GetUserRIPAttributesHandler
    : IRequestHandler<Contracts.GetUserRIPAttributesRequest, Contracts.UserRIPAttributes>
{
    public async Task<UserRIPAttributes> Handle(GetUserRIPAttributesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // vytahnout info o uzivateli z DB
            var dbIdentity = (await _dbContext.DbUserRIPAttributes
                .FromSqlInterpolated($"EXECUTE [dbo].[p_GetPersonHF_RIP] @identity={request.Identity}, @identityScheme={request.IdentityScheme}")
                .ToListAsync(cancellationToken)
                ).FirstOrDefault();

            // TODO: docasne muzeme vracet null
#pragma warning disable CS8603 // Possible null reference return.
            return dbIdentity != null ? new UserRIPAttributes()
            {
                PersonId = dbIdentity.PersonId,
                DealerCompanyId = dbIdentity.DealerCompanyId,
                PersonJobPostId = dbIdentity.PersonJobPostId ?? "",
                PersonOrgUnitId = dbIdentity.PersonOrgUnitId,
                PersonOrgUnitName = dbIdentity.PersonOrgUnitName ?? "",
                PersonSurname = dbIdentity.PersonSurname ?? "",
                Company = dbIdentity.Company ?? "",
                BrokerId = dbIdentity.BrokerId
            } : null;
#pragma warning restore CS8603 // Possible null reference return.
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 50000)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.IdentitySchemeNotExist, request.IdentityScheme);
        }
    }

    private readonly Database.UserServiceDbContext _dbContext;

    public GetUserRIPAttributesHandler(Database.UserServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
