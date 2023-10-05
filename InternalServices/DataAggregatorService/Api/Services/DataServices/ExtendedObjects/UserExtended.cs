using DomainServices.UserService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ExtendedObjects;

internal class UserExtended : ExtendedObject<User>
{
    public UserInfoObject Info => Source.UserInfo;

    public string FullNameWithDetails => $"{Source.UserInfo.DisplayName} (IČP: {Info.Icp}, ČPM: {Info.Cpm})";

    public bool IsExternal => !Source.UserInfo.IsInternal;
}