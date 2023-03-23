namespace DomainServices.HouseholdService.Clients;

public static class Helpers
{
    /// <summary>
    /// Nevim kam jinam to dat, je to nejaka jednoducha logika, ktera se bude pouzivat na vice mistech mimo sluzbu jako takovou
    /// </summary>
    public static bool AreCustomersPartners(int? maritalStatusId1, int? maritalStatusId2)
    {
        if (maritalStatusId1.GetValueOrDefault() == 0 || maritalStatusId2.GetValueOrDefault() == 0) return false;

        return !((maritalStatusId1.GetValueOrDefault() == 2 && maritalStatusId2.GetValueOrDefault() == 2)
            || (maritalStatusId1.GetValueOrDefault() == 6 && maritalStatusId2.GetValueOrDefault() == 6));
    }
}