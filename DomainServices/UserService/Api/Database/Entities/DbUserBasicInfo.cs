using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api.Database.Entities;

[Keyless]
internal sealed class DbUserBasicInfo
{
    public string v33jmeno { get; set; }
    public string v33prijmeni { get; set; }
}
