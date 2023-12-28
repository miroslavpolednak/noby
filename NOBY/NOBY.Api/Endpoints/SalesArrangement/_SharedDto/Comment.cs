﻿namespace NOBY.Api.Endpoints.SalesArrangement.SharedDto;

public class Comment
{
    /// <summary>
    /// Komentář k žádosti
    /// </summary>
    /// <example>Tvrdí, že není politicky exponovaná osoba, ale já myslím, že je.</example>
    public string? Text { get; set; }
}