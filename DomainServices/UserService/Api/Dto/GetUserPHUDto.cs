namespace DomainServices.UserService.Api.Dto;

internal sealed record GetUserPHUDto(
    string? firstname, 
    string? surname, 
    string? email, 
    string? phone, 
    string? mortgageCenterId,
    string? mortgageCenterAdditionalId)
{
}
