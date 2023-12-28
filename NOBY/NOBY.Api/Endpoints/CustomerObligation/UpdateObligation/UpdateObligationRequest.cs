﻿using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.CustomerObligation.UpdateObligation;

public sealed class UpdateObligationRequest
    : SharedDto.ObligationDto, IRequest
{
    [JsonIgnore]
    public int ObligationId { get; set; }

    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    internal UpdateObligationRequest InfuseId(int customerOnSAId, int incomeId)
    {
        this.CustomerOnSAId = customerOnSAId;
        this.ObligationId = incomeId;
        return this;
    }
}
