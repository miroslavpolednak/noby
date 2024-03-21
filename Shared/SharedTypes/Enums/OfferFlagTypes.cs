namespace SharedTypes.Enums;

[Flags]
public enum OfferFlagTypes : int
{
    None = 0,
    Current = 1,
    Communicated = 2,
    LegalNotice = 4,
    Liked = 8,
    Selected = 16
}
