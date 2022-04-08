
namespace ExternalServices.CustomerManagement.V1.CMWrapper;

public partial class Client
{
    public Client(string? baseUrl, HttpClient? httpClient)
    {
        BaseUrl = baseUrl;
        _httpClient = httpClient;
        _settings = new Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);

    }

    public Task<CustomerSearchResult> SearchCustomerAsync(SearchCustomerRequest model, string x_B3_TraceId, string x_KB_Orig_System_Identity, string x_KB_Caller_System_Identity, CancellationToken cancellationToken = default)
    {
        return SearchCustomerAsync(model.NumberOfEntries, model.CustomerId, model.Name, model.FirstName, model.BirthEstablishedDate, model.IdentifierValue, model.IdentifierTypeCode, model.IdDocumentTypeCode, model.IdDocumentNumber, model.IdDocumentIssuingCountryCode, model.Email, model.PhoneNumber, model.IsInKbi, model.LegalStatusCode, model.IncludeArchived, model.ShowSegment, x_B3_TraceId, "", x_KB_Orig_System_Identity, x_KB_Caller_System_Identity, cancellationToken: cancellationToken);
    }
}

public class SearchCustomerRequest
{
    /// <summary>
    /// Number of entries to return. When not specified or when is greater than 100 then 100 is applied.
    /// </summary>
    public int? NumberOfEntries { get; set; }

    /// <summary>
    /// Customer's ID (KBID) filter - primary filter
    /// </summary>
    public long? CustomerId { get; set; }

    /// <summary>
    /// Surname/company name filter - primary filter. Surname or company name can be used with mask - at least 3 marks + * (example: Komer* for Komercni Banka)
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// First name filter - can be used only as additional filter to surname for Natural Person. First name can be used with mask - at least 3 marks + *
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Birth date/Established date filter, can be used only as additional filter with other filters
    /// </summary>
    public DateTime? BirthEstablishedDate { get; set; }

    /// <summary>
    /// Identifier value filter - primary filter
    /// </summary>
    public string? IdentifierValue { get; set; }

    /// <summary>
    /// Identifier type filter, additional filter to identifierValue. When not specified then all identifier types are used.
    /// </summary>
    public string? IdentifierTypeCode { get; set; }

    /// <summary>
    /// Type of identification document. Can be used only with idDocumentNumber and idDocumentIssuingCountryCode.
    /// </summary>
    public string? IdDocumentTypeCode { get; set; }

    /// <summary>
    /// Identification document number. Can be used only with idDocumentTypeCode and idDocumentIssuingCountryCode.
    /// </summary>
    public string? IdDocumentNumber { get; set; }

    /// <summary>
    /// Identification document issuing country code. Can be used only with idDocumentTypeCode and idDocumentNumber.
    /// </summary>
    public string? IdDocumentIssuingCountryCode { get; set; }

    /// <summary>
    /// Email filter
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Phone number filter
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Flag indicating whether subject should or should not be in KBI. Possible variants: &lt;ul&gt; &lt;li&gt;parameter is not used - all subject are returned&lt;/li&gt; &lt;li&gt;true - only subjects in KBI are returned&lt;/li&gt; &lt;li&gt;false - only subjects not in KBI are returned&lt;/li&gt; &lt;/ul&gt;
    /// </summary>
    public bool? IsInKbi { get; set; }

    //Legal status filter
    public IEnumerable<Anonymous>? LegalStatusCode { get; set; }

    /// <summary>
    /// Flag indicating whether archived customers will be returned
    /// </summary>
    public bool? IncludeArchived { get; set; }

    /// <summary>
    /// Flag indicating whether segment (fixed and float) should be returnd in response
    /// </summary>
    public bool? ShowSegment { get; set; }
}
