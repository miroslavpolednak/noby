using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api;

internal static class EasExtensions
{
    public static Eas.EasWrapper.S_KLIENTDATA ToEasKlientData(this CreateRequest model) 
    {
        var typKlienta = model.EasTypKlienta();

        var partner = new Eas.EasWrapper.S_KLIENTDATA
        {
            klient_type = (int)typKlienta,
            rodne_cislo_ico = model.BirthNumber,
            priezvisko = model.LastName
        };

        // mimo PO
        if (typKlienta != EasKlientTypes.PO)
        {
            partner.meno = model.FirstName;
            // cizinec
            if (typKlienta == EasKlientTypes.CizinecBezRc && model.DateOfBirth != null)
            {
                partner.datum_narodenia = model.DateOfBirth;
                partner.pohlavie = model.Gender switch
                {
                    Genders.Female => "Z",
                    Genders.Male => "M",
                    _ => "N"
                };
            }
        }

        return partner;
    }

    public static EasKlientTypes EasTypKlienta(this CreateRequest model)
    {
        if (!string.IsNullOrEmpty(model.BirthNumber))
            return EasKlientTypes.FO;
        if (!string.IsNullOrEmpty(model.Ico))
            return EasKlientTypes.PO;
        return EasKlientTypes.CizinecBezRc;
    }
}
