namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ExtendedObjects;

internal class UserExtended : ExtendedObject<DomainServices.UserService.Clients.Dto.UserDto>
{
    public DomainServices.UserService.Contracts.UserInfoObject Info => Source.UserInfo;

    public string FullNameWithDetails => $"{Source.UserInfo.DisplayName} (IČP: {Info.Icp}, ČPM: {Info.Cpm})";

    public bool IsExternal => !Source.UserInfo.IsInternal;
}