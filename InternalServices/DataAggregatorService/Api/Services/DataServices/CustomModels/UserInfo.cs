using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;

internal class UserInfo
{
    private readonly User _user;

    public UserInfo(User user)
    {
        _user = user;
    }

    public string CPM => _user.CPM;

    public string ICP => _user.ICP;

    public string FullName => _user.FullName;

    public string FullNameWithDetails => $"{_user.FullName} ({CPM}, {ICP})";

    public string Phone => _user.Phone;

    public string Email => _user.Email;
}