namespace ExternalServices.Eas.V1.CheckFormV2
{
    public class Response
    {
        public int CommonValue { get; private set; }

        public string CommonText { get; private set; }

        public Errors? Detail { get; private set; }

        public Response(EasWrapper.CommonResult commonResult, EasWrapper.CheckFormData formData)
        {
            CommonValue = commonResult.return_val;
            CommonText = commonResult.return_text;

            if (!String.IsNullOrEmpty(formData.data))
            {
                Detail = Errors.Parse(formData.data);
            }
        }

    }
}
