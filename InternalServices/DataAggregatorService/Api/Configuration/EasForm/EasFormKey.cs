namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;

public class EasFormKey
{
    private static readonly EasFormType[] _productFormTypes = { EasFormType.F3601, EasFormType.F3602 };

    public EasFormKey(EasFormRequestType requestType, EasFormType[] easFormTypes)
    {
        RequestType = requestType;
        RequestTypeId = (int)requestType;
        EasFormTypes = easFormTypes;
    }

    public static EasFormKey ForProduct() => new(EasFormRequestType.Product, _productFormTypes);

    public static EasFormKey ForService(EasFormType formType) => new(EasFormRequestType.Service, new[] { formType });

    public EasFormRequestType RequestType { get; }

    public int RequestTypeId { get; }

    public EasFormType[] EasFormTypes { get;}

    public override bool Equals(object? obj)
    {
        if (obj is not EasFormKey toCompare)
            return false;

        if (ReferenceEquals(this, toCompare))
            return true;

        return RequestTypeId == toCompare.RequestTypeId && EasFormTypes.SequenceEqual(toCompare.EasFormTypes);
    }

    public override int GetHashCode()
    {
        var collectionHashCode = EasFormTypes.Select(type => (int)type).Aggregate(0, (current, formTypeId) => current ^ formTypeId.GetHashCode());

        return HashCode.Combine(RequestTypeId, collectionHashCode);
    }
}