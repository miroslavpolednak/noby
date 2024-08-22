namespace DomainServices.UserService.Clients;

internal static class Mapper
{
    public static Dto.UserDto MapToDto(this Contracts.User user)
    {
        return new Dto.UserDto
        {
            UserId = user.UserId,
            UserIdentifiers = user.UserIdentifiers.ToList(),
            UserInfo = new Contracts.UserInfoObject
            {
                ChannelId = user.UserInfo.ChannelId,
                Cin = user.UserInfo.Cin,
                Cpm = user.UserInfo.Cpm,
                DealerCompanyName = user.UserInfo.DealerCompanyName,
                DisplayName = user.UserInfo.DisplayName,
                Email = user.UserInfo.Email,
                FirstName = user.UserInfo.FirstName,
                Icp = user.UserInfo.Icp,
                IsInternal = user.UserInfo.IsInternal,
                IsUserVIP = user.UserInfo.IsUserVIP,
                LastName = user.UserInfo.LastName,
                PersonOrgUnitName = user.UserInfo.PersonOrgUnitName,
                PhoneNumber = user.UserInfo.PhoneNumber
            },
            UserPermissions = user.UserPermissions.ToList()
        };
    }
}
