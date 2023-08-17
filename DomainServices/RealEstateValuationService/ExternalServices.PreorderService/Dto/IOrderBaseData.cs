namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.Dto;

public interface IOrderBaseData
{
    string DealNumber { get; set; }
    ICollection<long> CremRealEstateIds { get; set; }
    ICollection<long> AttachmentIds { get; set; }
    long Cpm { get; set; }
    long Icp { get; set; }
    string CompanyCode { get; set; }
    string ProductCode { get; set; }
    long EFormId { get; set; }
}
