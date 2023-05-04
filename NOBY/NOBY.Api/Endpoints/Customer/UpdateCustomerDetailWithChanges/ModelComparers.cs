using KellermanSoftware.CompareNetObjects;
using NOBY.Api.Endpoints.Customer.Shared;

namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

internal static class ModelComparers
{
    private static void writeDifferencesToDelta(ComparisonResult? result, IDictionary<string, Object> delta)
    {
        if (!result?.AreEqual ?? true)
        {
            foreach (var diff in result!.Differences)
            {
                delta.Add(diff.PropertyName, diff.Object2);
            }
        }
    }

    public static void ComparePerson(NaturalPerson? request, NaturalPerson? original, dynamic delta)
    {
        dynamic deltaPerson = new System.Dynamic.ExpandoObject();
        
        writeDifferencesToDelta(_rootCompareLogic.Compare(original, request), deltaPerson!);

        CompareObjects(request?.CitizenshipCountriesId, original?.CitizenshipCountriesId, "CitizenshipCountriesId", deltaPerson);
        CompareObjects(request?.LegalCapacity, original?.LegalCapacity, "LegalCapacity", deltaPerson);
        CompareObjects(request?.TaxResidences, original?.TaxResidences, "TaxResidences", deltaPerson);

        delta.NaturalPerson = deltaPerson;
    }

    public static void CompareRoot(UpdateCustomerDetailWithChangesRequest request, UpdateCustomerDetailWithChangesRequest original, dynamic delta)
    {
        writeDifferencesToDelta(_rootCompareLogic.Compare(original, request), delta);
    }

    public static void CompareObjects<T>(T? request, T? original, string propertyName, IDictionary<string, Object> delta)
    {
        if (!_basicCompareLogic.Compare(original, request).AreEqual)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            delta[propertyName] = request;
#pragma warning restore CS8601 // Possible null reference assignment.
        }
    }

    private static CompareLogic _basicCompareLogic = new CompareLogic();
    private static CompareLogic _rootCompareLogic = new CompareLogic();

    static ModelComparers()
    {
        // basic
        _basicCompareLogic.Config.MaxDifferences = 1;
        _basicCompareLogic.Config.IgnoreCollectionOrder = true;
        _basicCompareLogic.Config.CompareChildren = true;
        _basicCompareLogic.Config.IgnoreProperty<CIS.Foms.Types.Address>(x => x.IsPrimary);

        var spec = new Dictionary<Type, IEnumerable<string>>();
        spec.Add(typeof(CIS.Foms.Types.Address), new string[] { "AddressTypeId" });
        _basicCompareLogic.Config.CollectionMatchingSpec = spec;

        // root
        _rootCompareLogic.Config.IgnoreProperty<UpdateCustomerDetailWithChangesRequest>(x => x.CustomerOnSAId);
        _rootCompareLogic.Config.MaxDifferences = 20;
        _rootCompareLogic.Config.CompareChildren = false;
    }
}
