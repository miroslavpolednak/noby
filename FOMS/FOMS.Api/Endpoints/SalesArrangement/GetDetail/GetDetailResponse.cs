﻿namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail;

public sealed class GetDetailResponse
{
    /// <summary>
    /// ID zadosti.
    /// </summary>
    public int SalesArrangementId { get; set; }
    
    /// <summary>
    /// Druh zadosti. Ciselnik SalesArrangementTypes.
    /// </summary>
    public int SalesArrangementTypeId { get; set; }
    
    /// <summary>
    /// Data o zadosti - bude se jednat o ruzne objekty podle typu zadosti.
    /// </summary>
    public object Data { get; set; }
}