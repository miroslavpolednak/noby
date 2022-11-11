namespace DomainServices.HouseholdService.Clients;

public static class Helpers
{
    public static bool AreCustomersPartners(int? maritalStatusId1, int? maritalStatusId2)
    {
        if (!maritalStatusId2.HasValue) return false;
        return !(maritalStatusId1.GetValueOrDefault() == 2 && maritalStatusId2.GetValueOrDefault() == 2);
    }
}
