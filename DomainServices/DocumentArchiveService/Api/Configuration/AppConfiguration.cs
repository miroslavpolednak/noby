﻿using System.ComponentModel.DataAnnotations;

namespace DomainServices.DocumentArchiveService.Api.Configuration;

internal sealed class AppConfiguration
{
    /// <summary>
    /// Nastaveni mapovani technickych uzivatelu sluzby vs. Login v generovanem ID
    /// [service_user, C4M ItChannel]
    /// </summary>
    [Required]
    public Dictionary<string, string>? ServiceUser2LoginBinding { get; set; }

}
