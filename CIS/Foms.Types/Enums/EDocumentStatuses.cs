namespace CIS.Foms.Types.Enums;

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1720 // Identifier contains type name
public enum EDocumentStatuses : int
{
    UNKNOWN = 0,

    SIGNED = 1,
    VERIFIED = 2,
    SENT = 3,
    DELETED = 4,
    NEW = 5,
    IN_PROGRESS = 6,
    APPROVED = 7
}
