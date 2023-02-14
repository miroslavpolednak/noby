using ExternalServices.Eas.V1.EasWrapper;

namespace ExternalServices.Eas.V1;

internal static class ModelExtensions
{
    public static S_KLIENTDATA MapToEas(this Dto.ClientDataModel model)
    {
        switch (model.ClientType)
        {
            case Dto.ClientDataModel.ClientTypes.PO:
                return new S_KLIENTDATA
                {
                    kb_id = model.KbId,
                    klient_type = 2,
                    rodne_cislo_ico = model.Cin,
                    priezvisko = model.LastName
                };

            case Dto.ClientDataModel.ClientTypes.FO:
                return new S_KLIENTDATA
                {
                    kb_id = model.KbId,
                    klient_type = 1,
                    rodne_cislo_ico = model.BirthNumber,
                    priezvisko = model.LastName,
                    meno = model.FirstName
                };

            default:
                return new S_KLIENTDATA
                {
                    kb_id = model.KbId,
                    klient_type = 3,
                    rodne_cislo_ico = model.BirthNumber,
                    priezvisko = model.LastName,
                    meno = model.FirstName,
                    datum_narodenia = model.DateOfBirth.HasValue ? model.DateOfBirth.Value : default(DateTime),
                    pohlavie = model.Gender == CIS.Foms.Enums.Genders.Female ? "Z" : "M"
                };
        }
    }

    public static string[] FindDifferentProps(S_KLIENTDATA input, S_KLIENTDATA output)
    {
        var values = new List<string>();


        if (input.klient_type != default(int) && input.klient_type != output.klient_type)
        {
            values.Add("klient_type");
        }

        if (input.rodne_cislo_ico != default(string) && input.rodne_cislo_ico != output.rodne_cislo_ico)
        {
            values.Add("rodne_cislo_ico");
        }

        if (input.priezvisko != default(string) && input.priezvisko.Equals(output.priezvisko, StringComparison.InvariantCultureIgnoreCase))
        {
            values.Add("priezvisko");
        }

        if (input.meno != default(string) && input.meno.Equals(output.meno, StringComparison.InvariantCultureIgnoreCase))
        {
            values.Add("meno");
        }

        if (input.datum_narodenia != default(DateTime) && input.datum_narodenia != output.datum_narodenia)
        {
            values.Add("datum_narodenia");
        }

        if (input.pohlavie != default(string) && input.pohlavie != output.pohlavie)
        {
            values.Add("pohlavie");
        }

        return values.ToArray();
    }
}
