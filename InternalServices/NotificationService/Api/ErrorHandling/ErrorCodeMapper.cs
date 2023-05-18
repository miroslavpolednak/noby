using CIS.Core.ErrorCodes;
using CIS.InternalServices.NotificationService.Contracts.Common;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;

namespace CIS.InternalServices.NotificationService.Api.ErrorHandling;

internal sealed class ErrorCodeMapper : ErrorCodeMapperBase
{
    // Identifier
    public const int IdentifierInvalid = 300;
    public const int IdentityRequired = 301;
    public const int IdentityInvalid = 302;
    public const int IdentitySchemeRequired = 303;
    public const int IdentitySchemeInvalid = 304;
    
    // DocumentId
    public const int DocumentIdInvalid = 305;

    // CustomId
    public const int CustomIdInvalid = 306;
    
    // Legal person
    public const int NameRequired = 307;
    public const int NameLengthLimitExceeded = 308;
    
    // Natural person
    public const int FirstNameRequired = 309;
    public const int FirstNameLengthLimitExceeded = 310;
    public const int MiddleNameLengthLimitExceeded = 311;
    public const int SurnameRequired = 312;
    public const int SurnameLengthLimitExceeded = 313;
    
    // Party
    public const int EitherLegalOrNaturalPersonRequired = 314;
    public const int LegalPersonInvalid = 315;
    public const int NaturalPersonInvalid = 316;
    
    // Email address
    public const int ValueRequired = 317;
    public const int ValueInvalid = 318;
    public const int PartyRequired = 319;
    public const int PartyInvalid = 320;
    
    // Email content
    public const int FormatRequired = 321;
    public const int FormatInvalid = 322;
    public const int LanguageRequired = 323;
    public const int LanguageInvalid = 324;
    public const int EmailTextRequired = 325;
    
    // Email attachment 
    public const int BinaryRequired = 326;
    public const int BinaryInvalid = 327;
    public const int FilenameRequired = 328;
    public const int FilenameLengthLimitExceeded = 329;
    
    // Phone
    public const int PhoneInvalid = 330;
    public const int CountryCodeRequired = 331;
    public const int CountryCodeInvalid = 332;
    public const int NationalNumberRequired = 333;
    public const int NationalNumberInvalid = 334;
    
    // Get/Search result
    public const int NotificationIdRequired = 340;
    public const int AtLeastOneParameterRequired = 341;
    public const int BothIdentityAndIdentitySchemeRequired = 342;
    
    // Send email
    public const int FromRequired = 350;
    public const int FromInvalid = 351;
    public const int ToRequired = 352;
    public const int ToInvalid = 353;
    public const int BccInvalid = 354;
    public const int CcInvalid = 355;
    public const int ReplyToInvalid = 356;
    public const int SubjectRequired = 357;
    public const int SubjectInvalid = 358;
    public const int ContentRequired = 359;
    public const int ContentInvalid = 360;
    public const int AttachmentsCountLimitExceeded = 361;
    public const int AttachmentsInvalid = 362;
    
    // Send sms
    public const int SmsPhoneNumberRequired = 370;
    public const int SmsPhoneNumberInvalid = 371;
    public const int SmsProcessPriorityInvalid = 372;
    public const int SmsTypeInvalid = 373;
    public const int SmsTextRequired = 374;
    public const int SmsTextLengthLimitExceeded = 375;
    
    // Send sms from template
    public const int SmsTemplatePhoneNumberRequired = 380;
    public const int SmsTemplatePhoneNumberInvalid = 381;
    public const int SmsTemplateProcessPriorityInvalid = 382;
    public const int SmsTemplateTypeInvalid = 383;
    public const int SmsTemplatePlaceholdersRequired = 384;
    public const int SmsTemplatePlaceholdersInvalid = 385;
    
    // TODO Internal
    public const int ResultNotFound = 390;
    public const string CreateEmailResultFailed = "391";
    public const string CreateSmsResultFailed = "392";
    public const string UploadAttachmentFailed = "393";
    public const string ProduceSendEmailError = "394";
    public const string ProduceSendSmsError = "395";

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>
        {
            { IdentifierInvalid, $"{nameof(Identifier)} must contain either both {nameof(Identifier.Identity)} and {nameof(Identifier.IdentityScheme)} or none." },
            { IdentityRequired, $"{nameof(Identifier.Identity)} required."},
            { IdentityInvalid, $"Invalid {nameof(Identifier.Identity)}."},
            { IdentitySchemeRequired, $"{nameof(Identifier.IdentityScheme)} required."},
            { IdentitySchemeInvalid, $"Invalid {nameof(Identifier.IdentityScheme)}."},
            
            { DocumentIdInvalid, $"Invalid DocumentId."},
            { CustomIdInvalid, $"Invalid CustomId."},

            { NameRequired, $"{nameof(LegalPerson.Name)} required." },
            { NameLengthLimitExceeded, $"Maximum length of {nameof(LegalPerson.Name)} is 255." },
            
            { FirstNameRequired, $"{nameof(NaturalPerson.FirstName)} required." },
            { FirstNameLengthLimitExceeded, $"Maximum length of {nameof(NaturalPerson.FirstName)} is 40." },
            { MiddleNameLengthLimitExceeded, $"Maximum length of {nameof(NaturalPerson.MiddleName)} is 40." },
            { SurnameRequired, $"{nameof(NaturalPerson.Surname)} required." },
            { SurnameLengthLimitExceeded, $"Maximum length of {nameof(NaturalPerson.Surname)} is 80." },
            
            { EitherLegalOrNaturalPersonRequired, $"{nameof(Party)} must contain either {nameof(LegalPerson)} or {nameof(NaturalPerson)}." },
            { LegalPersonInvalid, $"Invalid {nameof(Party.LegalPerson)}." },
            { NaturalPersonInvalid, $"Invalid {nameof(Party.NaturalPerson)}." },
            
            { ValueRequired, $"{nameof(EmailAddress.Value)} required." },
            { ValueInvalid, $"Invalid {nameof(EmailAddress.Value)}." },
            { PartyRequired, $"{nameof(EmailAddress.Party)} required." },
            { PartyInvalid, $"Invalid {nameof(EmailAddress.Party)}." },
            
            { FormatRequired, $"{nameof(EmailContent.Format)} required." },
            // { FormatInvalid, "TODO" },
            { LanguageRequired, $"{nameof(EmailContent.Language)} required." },
            // { LanguageInvalid, "TODO" },
            { EmailTextRequired, $"{nameof(EmailContent.Text)} required." },
            
            { BinaryRequired, $"{nameof(EmailAttachment.Binary)} required." },
            { BinaryInvalid, $"{nameof(EmailAttachment.Binary)} must be encoded in Base64." },
            { FilenameRequired, $"{nameof(EmailAttachment.Filename)} required." },
            { FilenameLengthLimitExceeded, $"Maximum length of {nameof(EmailAttachment.Filename)} is 255." },
            
            { PhoneInvalid, $"{nameof(Phone)} not in standard E. 164" },
            { CountryCodeRequired, $"{nameof(Phone.CountryCode)} required." },
            { CountryCodeInvalid, $"Invalid {nameof(Phone.CountryCode)}." },
            { NationalNumberRequired, $"{nameof(Phone.NationalNumber)} required." },
            { NationalNumberInvalid, $"Invalid {nameof(Phone.NationalNumber)}." },
            
            { NotificationIdRequired, $"{nameof(GetResultRequest.NotificationId)} must be not empty." },
            { AtLeastOneParameterRequired, $"{nameof(SearchResultsRequest)} must contain at least 1 non-empty search parameter." },
            { BothIdentityAndIdentitySchemeRequired, $"{nameof(SearchResultsRequest)} must contain either both {nameof(SearchResultsRequest.Identity)} and {nameof(SearchResultsRequest.IdentityScheme)} or none." },
            
            { FromRequired, $"{nameof(SendEmailRequest.From)} required." },
            { FromInvalid, $"Invalid {nameof(SendEmailRequest.From)}." },
            { ToRequired, $"{nameof(SendEmailRequest.To)} must be not empty." },
            { ToInvalid, $"Invalid {nameof(SendEmailRequest.To)}." },
            { BccInvalid, $"Invalid {nameof(SendEmailRequest.Bcc)}." },
            { CcInvalid, $"Invalid {nameof(SendEmailRequest.Cc)}." },
            { ReplyToInvalid, $"Invalid {nameof(SendEmailRequest.ReplyTo)}." },
            { SubjectRequired, $"{nameof(SendEmailRequest.Subject)} required." },
            { SubjectInvalid, $"Invalid {nameof(SendEmailRequest.Subject)}." },
            { ContentRequired, $"{nameof(SendEmailRequest.Content)} required." },
            { ContentInvalid, $"Invalid {nameof(SendEmailRequest.Content)}." },
            { AttachmentsCountLimitExceeded, $"Maximum count of {nameof(SendEmailRequest.Attachments)} is 10." },
            { AttachmentsInvalid, $"Invalid {nameof(SendEmailRequest.Attachments)}." },

            { SmsPhoneNumberRequired, $"{nameof(SendSmsRequest.PhoneNumber)} required." },
            { SmsPhoneNumberInvalid, $"Invalid {nameof(SendSmsRequest.PhoneNumber)}." },
            { SmsProcessPriorityInvalid, $"Invalid {nameof(SendSmsRequest.ProcessingPriority)}." },
            { SmsTypeInvalid, $"Invalid {nameof(SendSmsRequest.Type)}." },
            { SmsTextRequired, $"{nameof(SendSmsRequest.Text)} required."},
            { SmsTextLengthLimitExceeded, $"Maximum length of {nameof(SendSmsRequest.Text)} is 480." },
            
            { SmsTemplatePhoneNumberRequired, $"{nameof(SendSmsFromTemplateRequest.PhoneNumber)} required."},
            { SmsTemplatePhoneNumberInvalid, $"Invalid {nameof(SendSmsFromTemplateRequest.PhoneNumber)}."},
            { SmsTemplateProcessPriorityInvalid, $"Invalid {nameof(SendSmsFromTemplateRequest.ProcessingPriority)}."},
            { SmsTemplateTypeInvalid, $"Invalid {nameof(SendSmsFromTemplateRequest.Type)}."},
            
            { SmsTemplatePlaceholdersRequired, $"{nameof(SendSmsFromTemplateRequest.Placeholders)} required." },
            { SmsTemplatePlaceholdersInvalid, $"{nameof(SendSmsFromTemplateRequest.Placeholders)} must contain non-empty values." },
            
            // { ResultNotFound, "TODO" },
            // { CreateEmailResultFailed, "TODO" },
            // { CreateSmsResultFailed, "TODO" },
            // { UploadAttachmentFailed, "TODO" },
            // { ProduceSendEmailError, "TODO" },
            // { ProduceSendSmsError, "TODO" },
        });

        return Messages;
    }
}