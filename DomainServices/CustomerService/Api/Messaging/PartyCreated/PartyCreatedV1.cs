//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v10.7.2.0 (Newtonsoft.Json v9.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------


namespace DomainServices.CustomerService.Api.Messaging.PartyCreated
{
    #pragma warning disable // Disable all warnings

    /// <summary>
    /// Party object, only one of the atributes (naturalPersonAttributes, entrepreneurAttributes, legalPersonAttributes) will be filled according to legalStatusCode
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class Party
    {
        /// <summary>
        /// Legal status code, P - FOO (Natural Person), E - FOP (Entrepreneur), B - PO (LegalPerson)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("legalStatusCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public PartyLegalStatusCode LegalStatusCode { get; set; }

        /// <summary>
        /// List of customer identifiers
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("customerIdentifiers")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<CustomerIdentifier> CustomerIdentifiers { get; set; } = new System.Collections.ObjectModel.Collection<CustomerIdentifier>();

        /// <summary>
        /// When party is of type Naturel Person (legalStatusCode = P) then this attribute contains data about Natural Person
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("naturalPersonAttributes")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public NaturalPersonAttributes NaturalPersonAttributes { get; set; }

        /// <summary>
        /// When party is of type Entrepreneur (legalStatusCode = E) then this attribute contains data about Entrepreneur
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("entrepreneurAttributes")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public EntrepreneurAttributes EntrepreneurAttributes { get; set; }

        /// <summary>
        /// When party is of type LegalPerson (legalStatusCode = B) then this attribute contains data about LegalPerson
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("legalPersonAttributes")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public LegalPersonAttributes LegalPersonAttributes { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// Attributes of Natural Person
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class NaturalPersonAttributes
    {
        /// <summary>
        /// Title
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("title")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(15, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>
        /// First name
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("firstName")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }

        /// <summary>
        /// Surname
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("surname")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 1)]
        public string Surname { get; set; }

        /// <summary>
        /// Gender (M - male, F - female)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("genderCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public NaturalPersonAttributesGenderCode GenderCode { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("birthDate")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset BirthDate { get; set; }

        /// <summary>
        /// czech birth number
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("czechBirthNumber")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(30, MinimumLength = 1)]
        public string CzechBirthNumber { get; set; }

        /// <summary>
        /// citizenship code, RDM codebook CB_Country
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("citizenshipCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(2, MinimumLength = 2)]
        public string CitizenshipCode { get; set; }

        /// <summary>
        /// List of customer citizenships
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("citizenshipCodes")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public System.Collections.Generic.ICollection<string> CitizenshipCodes { get; set; }

        /// <summary>
        /// Birth country code, RDM codebook CB_Country
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("birthCountryCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(2, MinimumLength = 2)]
        public string BirthCountryCode { get; set; }

        /// <summary>
        /// Marital status, RDM codebook CB_MaritalStatus
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("maritalStatusCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(1, MinimumLength = 1)]
        public string MaritalStatusCode { get; set; }

        /// <summary>
        /// Birth place
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("birthPlace")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 1)]
        public string BirthPlace { get; set; }

        /// <summary>
        /// Birth last name
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("birthName")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 1)]
        public string BirthName { get; set; }

        /// <summary>
        /// Date of death
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("deathDate")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset DeathDate { get; set; }

        /// <summary>
        /// Flag indicating that customer is legally incapable
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("isLegallyIncapable")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public bool IsLegallyIncapable { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// Attributes of  Entrepreneur
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class EntrepreneurAttributes
    {
        /// <summary>
        /// Entrepreneur name. Is filled with name from business register. When name from business register does not exists, KB custom search name is used instead.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("name")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 1)]
        public string Name { get; set; }

        /// <summary>
        /// Entrepreneur name. Name from business register.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("businessRegisterName")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 1)]
        public string BusinessRegisterName { get; set; }

        /// <summary>
        /// Custom KB search name.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("nameKb")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 1)]
        public string NameKb { get; set; }

        /// <summary>
        /// Entrepreneur established date
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("establishedOn")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset EstablishedOn { get; set; }

        /// <summary>
        /// Czech identification number
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("czechIdentificationNumber")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(30, MinimumLength = 1)]
        public string CzechIdentificationNumber { get; set; }

        /// <summary>
        /// Legal form
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("legalFormCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(4, MinimumLength = 3)]
        public string LegalFormCode { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// Attributes of LegalPerson
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class LegalPersonAttributes
    {
        /// <summary>
        /// Company name. Is filled with name from business register. When name from business register does not exists, KB custom search name is used instead.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("name")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 1)]
        public string Name { get; set; }

        /// <summary>
        /// Company name. Name from business register.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("businessRegisterName")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 1)]
        public string BusinessRegisterName { get; set; }

        /// <summary>
        /// Custom KB search name.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("nameKb")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 1)]
        public string NameKb { get; set; }

        /// <summary>
        /// Company established date
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("establishedOn")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset EstablishedOn { get; set; }

        /// <summary>
        /// Czech identification number
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("czechIdentificationNumber")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(30, MinimumLength = 1)]
        public string CzechIdentificationNumber { get; set; }

        /// <summary>
        /// Legal form
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("legalFormCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(4, MinimumLength = 3)]
        public string LegalFormCode { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class CustomerIdentifier
    {
        /// <summary>
        /// Identifier context code
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("contextCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string ContextCode { get; set; }

        /// <summary>
        /// Identifier value
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("identifierValue")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string IdentifierValue { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// PartyCreated event message. Event is triggered when new subject in bank was created.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class PartyCreatedV1
    {
        /// <summary>
        /// Customer ID, KBID
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("customerId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public long CustomerId { get; set; }

        /// <summary>
        /// Party value
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("party")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public Party Party { get; set; } = new Party();

        /// <summary>
        /// Lifecycle status of created subject
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("lifecycleStatusCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public LifecycleStatusCode LifecycleStatusCode { get; set; }

        /// <summary>
        /// Source of change
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("sourceOfChange")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public SourceOfChange SourceOfChange { get; set; }

        /// <summary>
        /// Timestamp when party was created
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("modificationTimestamp")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset ModificationTimestamp { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum PartyLegalStatusCode
    {

        [System.Runtime.Serialization.EnumMember(Value = @"P")]
        P = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"E")]
        E = 1,


        [System.Runtime.Serialization.EnumMember(Value = @"B")]
        B = 2,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum NaturalPersonAttributesGenderCode
    {

        [System.Runtime.Serialization.EnumMember(Value = @"M")]
        M = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"F")]
        F = 1,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum LifecycleStatusCode
    {

        [System.Runtime.Serialization.EnumMember(Value = @"ACTIVE")]
        ACTIVE = 0,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum SourceOfChange
    {

        [System.Runtime.Serialization.EnumMember(Value = @"BASIC_REGISTRY")]
        BASIC_REGISTRY = 0,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.7.2.0 (Newtonsoft.Json v9.0.0.0)")]
    internal class DateFormatConverter : System.Text.Json.Serialization.JsonConverter<System.DateTimeOffset>
    {
        public override System.DateTimeOffset Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            var dateTime = reader.GetString();
            if (dateTime == null)
            {
                throw new System.Text.Json.JsonException("Unexpected JsonTokenType.Null");
            }

            return System.DateTimeOffset.Parse(dateTime);
        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, System.DateTimeOffset value, System.Text.Json.JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("O"));
        }
    }
}