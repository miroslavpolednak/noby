namespace ExternalServices.Eas.R21.CheckFormV2
{
    public class Error
    {
        public string Value { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string AdditionalInformation { get; set; }

        public string ErrorQueue { get; set; }

        public bool Severity { get; set; }
    }
}
