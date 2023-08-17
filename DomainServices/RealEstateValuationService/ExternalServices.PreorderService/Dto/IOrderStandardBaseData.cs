namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.Dto;

public interface IOrderStandardBaseData
{
    string ContactPersonEmail { get; set; }
    string ContactPersonName { get; set; }
    string ContactPersonTel { get; set; }
    string BagmanRealEstateTypeId { get; set; }
    double? ActualPurchasePrice { get; set; }
    bool? Leased { get; set; }
    bool? OwnershipLimitations { get; set; }
    string ProductOwner { get; set; }
    double? MaturityLoan { get; set; }
    string PurposeLoan { get; set; }
    double? LoanAmount { get; set; }
    bool? IsCellarFlat { get; set; }
    bool? IsNonApartmentBuildingFlat { get; set; }
    bool? IsNotUsableTechnicalState { get; set; }
    string RealEstateTypeId { get; set; }
}