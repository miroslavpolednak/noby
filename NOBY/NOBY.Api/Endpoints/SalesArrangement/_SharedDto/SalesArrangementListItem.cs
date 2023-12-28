﻿namespace NOBY.Api.Endpoints.SalesArrangement.SharedDto;

public class SalesArrangementListItem
{
    public int SalesArrangementId { get; set; }
    
    public int? OfferId { get; set; }
    
    public int SalesArrangementTypeId { get; set; }
    
    public string? SalesArrangementTypeText { get; set; }
    
    public SharedTypes.Enums.SalesArrangementStates State { get; set; }
    
    public string? StateText { get; set; }
    
    public DateTime CreatedTime { get; set; }
    
    public string? CreatedBy { get; set; }
}