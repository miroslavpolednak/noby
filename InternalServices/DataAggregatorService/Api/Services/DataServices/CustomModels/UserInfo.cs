using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;

internal class UserInfo
{
    private readonly User _user;

    public UserInfo(User user)
    {
        _user = user;
    }

    public string CPM => _user.UserInfo.Cpm;

    public string ICP => _user.UserInfo.Icp;

    public string FullName => _user.UserInfo.DisplayName;

    public string FullNameWithDetails => $"{_user.UserInfo.DisplayName} (IČP: {ICP}, ČPM: {CPM})";

    public string Phone => _user.UserInfo.PhoneNumber;

    public string Email => _user.UserInfo.Email;
}