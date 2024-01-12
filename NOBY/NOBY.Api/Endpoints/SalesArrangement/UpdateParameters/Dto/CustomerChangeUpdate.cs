﻿using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters.Dto;

public sealed class CustomerChangeUpdate
{
    [Required]
    public GetSalesArrangement.Dto.CustomerChangeDetailRelease? Release { get; set; }

    [Required]
    public GetSalesArrangement.Dto.CustomerChangeDetailAdd? Add { get; set; }

    [Required]
    public CustomerChangeUpdateAgent? Agent { get; set; }

    [Required]
    public PaymentAccount? RepaymentAccount { get; set; }

    /// <summary>
    /// Komentář k žádosti o změnu
    /// </summary>
    [Required]
    public SalesArrangement.SharedDto.CommentToChangeRequest? CommentToChangeRequest { get; set; }
}

public sealed class CustomerChangeUpdateAgent
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Nový zmocněnec pro doručování
    /// </summary>
    public string? NewAgent { get; set; }
}