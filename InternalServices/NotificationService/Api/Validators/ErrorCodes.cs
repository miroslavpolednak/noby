namespace CIS.InternalServices.NotificationService.Api.Validators;

public static class ErrorCodes
{
    public static class Result
    {
        public const string NotificationIdNotEmpty = "300";
    }
    
    public static class LegalPerson
    {
        public const string NameRequired = "301";
        public const string NameLengthLimitExceeded = "302";
    }
    
    public static class NaturalPerson
    {
        public const string FirstNameRequired = "303";
        public const string FirstNameLengthLimitExceeded = "304";
        public const string MiddleNameLengthLimitExceeded = "305";
        public const string SurnameRequired = "306";
        public const string SurnameLengthLimitExceeded = "307";
    }
    
    public static class EmailParty
    {
        public const string LegalPersonInvalid = "308";
        public const string NaturalPersonInvalid = "309";
    }
    
    public static class EmailAddress
    {
        public const string ValueRequired = "310";
        public const string ValueInvalid = "311";
        public const string PartyRequired = "312";
        public const string PartyInvalid = "313";
    }

    public static class EmailContent
    {
        public const string FormatRequired = "314";
        public const string LanguageRequired = "315";
        public const string TextRequired = "316";
    }
    
    public static class EmailAttachment
    {
        public const string BinaryRequired = "317";
        public const string FilenameRequired = "318";
        public const string FilenameLengthLimitExceeded = "319";
    }
 
    public static class SendEmail
    {
        public const string FromRequired = "320";
        public const string FromInvalid = "321";
        public const string ToNotEmpty = "322";
        public const string ToInvalid = "323";
        public const string BccInvalid = "324";
        public const string CcInvalid = "326";
        public const string ReplyToInvalid = "326";
        public const string SubjectRequired = "327";
        public const string SubjectInvalid = "328";
        public const string ContentRequired = "329";
        public const string ContentInvalid = "330";
        public const string AttachmentsInvalid = "331";
    }

    public static class Phone
    {
        public const string CountryCodeRequired = "332";
        public const string CountryCodeInvalid = "333";
        public const string NationalNumberRequired = "334";
        public const string NationalNumberInvalid = "335";
    }
    
}