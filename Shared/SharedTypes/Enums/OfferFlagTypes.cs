﻿namespace SharedTypes.Enums;

[Flags]
public enum OfferFlagTypes : int
{
    None = 0,

    /// <summary>
    /// Aktuální nabídka
    /// </summary>
    Current = 1,

    /// <summary>
    /// Komunikovaná nabídka
    /// </summary>
    Communicated = 2,

    /// <summary>
    /// Zákonné oznámení
    /// </summary>
    LegalNotice = 4,

    /// <summary>
    /// Likovaná nabídka
    /// </summary>
    Liked = 8,

    /// <summary>
    /// Vybraná nabídka
    /// </summary>
    Selected = 16
}
