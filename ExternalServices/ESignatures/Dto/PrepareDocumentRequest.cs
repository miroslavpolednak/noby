namespace ExternalServices.ESignatures.Dto;

public sealed class PrepareDocumentRequest
{
    public UserInfo CurrentUserInfo { get; set; } = null!;
    
    public UserInfo CreatorInfo { get; set; } = null!;

    public DocumentInfo DocumentData { get; set; } = null!;

    public ClientInfo ClientData { get; set; } = null!;

    public List<OtherClient>? OtherClients { get; set; } = null!;

    public sealed class DocumentInfo
    {
        /// <summary>
        /// Slouzi pro ziskani "EaCodeMain" z ePodpis kontraktu
        /// </summary>
        public int DocumentTypeId { get; set; }

        /// <summary>
        /// Slouzi pro ziskani "TypeCode" a "TemplateVersion" z ePodpis kontraktu
        /// </summary>
        public int DocumentTemplateVersionId { get; set; }

        public string FormId { get; set; } = string.Empty;

        /// <summary>
        /// "Name" z ePodpis kontraktu
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        public string ContractNumber { get; set; } = string.Empty;
    }

    public sealed class ClientInfo : BaseClient
    {
        public string? BirthNumber { get; set; }   
    }

    public sealed class OtherClient : BaseClient
    {
    }

    public abstract class BaseClient
    {
        /// <summary>
        /// Cislo kotvy podpisu - X_SIG_{CodeIndex}
        /// </summary>
        public int CodeIndex { get; set; } = 1;

        /// <summary>
        /// Slouzi k naplneni "ExternalId" a "UniversalID" z ePodpis kontraktu
        /// </summary>
        public IEnumerable<CIS.Foms.Types.CustomerIdentity>? Identities { get; set; }

        /// <summary>
        /// "Name" z ePodpis kontraktu
        /// </summary>
        public string? FullName { get; set; } = string.Empty;

        /// <summary>
        /// "PhoneNumber" z ePodpis kontraktu
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// EmailAddress z ePodpis kontraktu
        /// </summary>
        public string? Email { get; set; }
    }
}