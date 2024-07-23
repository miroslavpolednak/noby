using DomainServices.HouseholdService.Contracts.Model;
using FastMember;
using KellermanSoftware.CompareNetObjects;

namespace NOBY.Api.Endpoints.Customer.UpdateCustomerDetailWithChanges;

internal static class ModelComparers
{
    private static readonly CompareLogic _basicCompareLogic = new();
    private static readonly CompareLogic _rootCompareLogic = new();

    static ModelComparers()
    {
        // basic
        _basicCompareLogic.Config.MaxDifferences = 1;
        _basicCompareLogic.Config.IgnoreCollectionOrder = true;
        _basicCompareLogic.Config.CompareChildren = true;
        _basicCompareLogic.Config.TreatStringEmptyAndNullTheSame = true;
        _basicCompareLogic.Config.IgnoreProperty<SharedTypes.Types.Address>(x => x.IsPrimary);
        _basicCompareLogic.Config.IgnoreProperty<SharedTypes.Types.Address>(x => x.SingleLineAddressPoint);

        var spec = new Dictionary<Type, IEnumerable<string>> { { typeof(SharedTypes.Types.Address), ["AddressTypeId"] } };
        _basicCompareLogic.Config.CollectionMatchingSpec = spec;

        // root
        _rootCompareLogic.Config.MaxDifferences = 20;
        _rootCompareLogic.Config.CompareChildren = false;
    }

    private static int WriteDifferencesToDelta(ComparisonResult? result, NaturalPersonDelta naturalPersonDelta)
    {
        if (!result?.AreEqual ?? true)
        {
            var accessor = TypeAccessor.Create(typeof(NaturalPersonDelta));

            foreach (var diff in result!.Differences)
            {
                accessor[naturalPersonDelta, diff.PropertyName] = diff.Object2;
            }

            return result.Differences.Count;
        }

        return 0;
    }

    public static DeltaComparerResult ComparePerson(NaturalPersonDelta? request, NaturalPersonDelta? original, CustomerChangeData delta)
    {
        var deltaPerson = new NaturalPersonDelta();
        var hasDifferences = false;
        var crsIsDifferent = false;

        var differencesCount = WriteDifferencesToDelta(_rootCompareLogic.Compare(original, request), deltaPerson!);

        CompareObjects(request?.CitizenshipCountriesId, original?.CitizenshipCountriesId, ref hasDifferences, obj => deltaPerson.CitizenshipCountriesId = obj);
        CompareObjects(request?.LegalCapacity, original?.LegalCapacity, ref hasDifferences, obj => deltaPerson.LegalCapacity = obj);
        CompareObjects(request?.TaxResidences, original?.TaxResidences, ref crsIsDifferent, obj => deltaPerson.TaxResidences = obj);

        if (differencesCount > 0 || hasDifferences || crsIsDifferent)
        {
            delta.NaturalPerson = deltaPerson;

            return new DeltaComparerResult
            {
                ClientDataWereChanged = differencesCount > 0 || hasDifferences,
                CrsWasChanged = crsIsDifferent
            };
        }

        return new DeltaComparerResult();
    }

    public static bool AreObjectsEqual<T>(T? request, T? original)
    {
        return _basicCompareLogic.Compare(original, request).AreEqual;
    }

    public static bool CompareObjects<TProperty>(TProperty? request, TProperty? original, ref bool hasDifferences, Action<TProperty?> setter)
    {
        if (_basicCompareLogic.Compare(original, request).AreEqual) 
            return false;

        hasDifferences = true;
        setter(request);
        return true;

    }
}
