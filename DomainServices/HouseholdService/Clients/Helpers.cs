namespace DomainServices.HouseholdService.Clients;

public static class Helpers
{
    public static bool AreCustomersPartners(int? meritalStatusId1, int? meritalStatusId2)
    {
        return meritalStatusId1.GetValueOrDefault() == 2 && meritalStatusId2.GetValueOrDefault() == 2;
    }
}
