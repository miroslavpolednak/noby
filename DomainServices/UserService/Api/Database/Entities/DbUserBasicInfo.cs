using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api.Database.Entities;

[Keyless]
internal sealed class DbUserBasicInfo
{
    public string? DisplayName { get; set; }
}
