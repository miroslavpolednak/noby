using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api.Database.Entities;

[Keyless]
internal sealed class DbUserPermission
{
    public string PermissionCode { get; set; } = null!;
}
