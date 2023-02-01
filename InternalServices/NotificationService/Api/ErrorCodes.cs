namespace CIS.InternalServices.NotificationService.Api;

public static class ErrorCodes
{
    public static class Validation
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
            public const string EitherLegalOrNaturalPersonRequired = "308";
            public const string LegalPersonInvalid = "309";
            public const string NaturalPersonInvalid = "310";
        }

        public static class EmailAddress
        {
            public const string ValueRequired = "311";
            public const string ValueInvalid = "312";
            public const string PartyRequired = "313";
            public const string PartyInvalid = "314";
        }

        public static class EmailContent
        {
            public const string FormatRequired = "315";
            public const string LanguageRequired = "316";
            public const string TextRequired = "317";
        }

        public static class EmailAttachment
        {
            public const string BinaryRequired = "318";
            public const string BinaryInvalid = "319";
            public const string FilenameRequired = "320";
            public const string FilenameLengthLimitExceeded = "321";
        }

        public static class Phone
        {
            public const string CountryCodeRequired = "322";
            public const string CountryCodeInvalid = "323";
            public const string NationalNumberRequired = "324";
            public const string NationalNumberInvalid = "325";
        }

        public static class SearchResult
        {
            public const string AtLeastOneParameterRequired = "330";
            public const string IdentityInvalid = "331";
        }

        public static class SendEmail
        {
            public const string FromRequired = "340";
            public const string FromInvalid = "341";
            public const string ToNotEmpty = "342";
            public const string ToInvalid = "343";
            public const string BccInvalid = "344";
            public const string CcInvalid = "345";
            public const string ReplyToInvalid = "346";
            public const string SubjectRequired = "347";
            public const string SubjectInvalid = "348";
            public const string ContentRequired = "349";
            public const string ContentInvalid = "350";
            public const string AttachmentsInvalid = "351";
        }

        public static class SendSms
        {
            public const string PhoneRequired = "360";
            public const string PhoneInvalid = "361";
            public const string ProcessPriorityInvalid = "362";
            public const string TypeInvalid = "363";
            public const string TextRequired = "364";
            public const string TextLengthLimitExceeded = "365";
        }

        public static class SendSmsFromTemplate
        {
            public const string PhoneRequired = "370";
            public const string PhoneInvalid = "371";
            public const string ProcessPriorityInvalid = "372";
            public const string TypeInvalid = "373";
            public const string TextRequired = "374";
            public const string TextLengthLimitExceeded = "375";
            public const string PlaceholdersRequired = "376";
            public const string PlaceholdersInvalid = "377";
        }
    }

    public static class Internal
    {
        public const int ResultNotFound = 380;
        public const string CreateEmailResultFailed = "381";
        public const string CreateSmsResultFailed = "382";
        public const string UploadAttachmentFailed = "383";
        public const string ProduceSendEmailError = "384";
        public const string ProduceSendSmsError = "385";
    }
}