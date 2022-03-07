namespace FOMS.Core;

public struct ErrorCodes
{
    public const int OfferIdAlreadyLinkedToSalesArrangement = 1;
    public const int OfferDefaultSalesArrangementTypeIdNotFound = 2;
    public const int CodebookDuplicatedInQueryParam = 3;
    public const int SalesArrangementNotLinkedToOffer = 4;
    public const int SalesArrangementProductCategoryNotFound = 5;
    public const int SalesArrangementOfferIdIsNull = 6;
    public const int HouseholdTypeNotFound = 8;

    public const int CaseNotFoundInKonsDb = 7;
}