namespace DomainServices.UserService.Api.Endpoints.v1.GetUserPermissions;

internal sealed class GetUserPermissionsHandler(IConnectionProvider _db)
        : IRequestHandler<Contracts.GetUserPermissionsRequest, Contracts.GetUserPermissionsResponse>
{
    public async Task<Contracts.GetUserPermissionsResponse> Handle(Contracts.GetUserPermissionsRequest request, CancellationToken cancellationToken)
    {
        var dbPermissions = await _db.ExecuteDapperStoredProcedureSqlToListAsync<dynamic>(
            "[dbo].[getPermissions]",
            new { ApplicationCode = "NOBY", v33id = request.UserId },
            cancellationToken);

        // vytvorit finalni model
        var model = new Contracts.GetUserPermissionsResponse();
        dbPermissions.ForEach(t =>
        {
            if (int.TryParse(t.PermissionCode, out int p))
            {
                model.UserPermissions.Add(p);
            }
        });

        return model;
    }
}
