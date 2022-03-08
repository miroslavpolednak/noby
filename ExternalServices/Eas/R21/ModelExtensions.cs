using ExternalServices.Eas.R21.EasWrapper;

namespace ExternalServices.Eas.R21;

internal static class ModelExtensions
{
    public static S_KLIENTDATA MapToEas(this Dto.ClientDataModel model)
    {
        if (!string.IsNullOrEmpty(model.BirthNumber))
        {
            return new S_KLIENTDATA
            {
                klient_type = 1,
                rodne_cislo_ico = model.BirthNumber,
                priezvisko = model.LastName,
                meno = model.FirstName
            };
        }
        else if (!string.IsNullOrEmpty(model.Cin))
        {
            return new S_KLIENTDATA
            {
                klient_type = 2,
                rodne_cislo_ico = model.Cin,
                priezvisko = model.LastName
            };
        }
        else
        {
            return new S_KLIENTDATA
            {
                klient_type = 3,
                rodne_cislo_ico = model.BirthNumber,
                priezvisko = model.LastName,
                meno = model.FirstName,
                datum_narodenia = model.DateOfBirth.HasValue ? model.DateOfBirth.Value : default(DateTime),
                pohlavie = model.Gender == CIS.Foms.Enums.Genders.Female ? "Z" : "M"
            };
        }
    }
}
