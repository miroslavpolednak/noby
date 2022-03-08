using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api;

internal static class EasExtensions
{
    public static ExternalServices.Eas.Dto.ClientDataModel ToEasKlientData(this CreateRequest model)
        => new ExternalServices.Eas.Dto.ClientDataModel
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            BirthNumber = model.BirthNumber,
            Cin = model.Ico,
            DateOfBirth = model.DateOfBirth,
            Gender = (CIS.Foms.Enums.Genders)(int)model.Gender
        };
}
