namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

public class SalesArrangementListItem
{
    public string? ProductName { get; set; }

    public int SalesArrangementId { get; set; }
    
    public int? OfferId { get; set; }
    
    public int SalesArrangementTypeId { get; set; }
    
    public string? SalesArrangementTypeText { get; set; }
    
    public CIS.Foms.Enums.SalesArrangementStates State { get; set; }
    
    public string? StateText { get; set; }
    
    public DateTime CreatedTime { get; set; }
    
    public string? CreatedBy { get; set; }
}