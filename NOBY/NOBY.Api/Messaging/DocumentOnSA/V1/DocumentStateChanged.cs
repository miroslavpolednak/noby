namespace cz.mpss.api.epodpisy.digitalsigning.documentsigningevents.v1;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public enum DocumentStateEnum
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    NEW,
    IN_PROGRESS,
    APPROVED,
    DELETED,
#pragma warning disable CA1720 // Identifier contains type name
    SIGNED,
#pragma warning restore CA1720 // Identifier contains type name
    VERIFIED,
    SENT
}

/// <summary>Schema belongs to API DocumentSigningEvents.v1. Schema represents an event of changing the state of document within ePodpisy application.</summary>
public class DocumentStateChanged
{
    /// <summary>Unique identifier of event.</summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string eventId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>Date and time when event occurred.</summary>
    public long occurredOn { get; set; }

    /// <summary>Externally defined unique identifier of document within ePodpisy.</summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string documentExternalId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>Current runtime state of the document within ePodpisy.</summary>
    public DocumentStateEnum state { get; set; }

    /// <summary>Previous runtime state of the document within ePodpisy.</summary>
    public global::cz.mpss.api.epodpisy.digitalsigning.documentsigningevents.v1.DocumentStateEnum? previousState { get; set; }
}
