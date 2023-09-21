﻿using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Household.CreateHousehold;

public class CreateHouseholdRequest
    : IRequest<Dto.HouseholdInList>
{
    [JsonIgnore]
    internal int SalesArrangementId { get; set; }

    /// <summary>
    /// ID typu domacnosti. Ciselnik HouseholdTypes
    /// </summary>
    /// <example>1</example>
    public int HouseholdTypeId { get; set; }

    /// <summary>
    /// Pouze pokud se vola handler z jineho handleru
    /// </summary>
    internal bool HardCreate { get; set; }

    internal CreateHouseholdRequest InfuseId(int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
        return this;
    }
}
