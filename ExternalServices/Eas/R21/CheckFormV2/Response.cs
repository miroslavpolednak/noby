namespace ExternalServices.Eas.R21.CheckFormV2
{
    public class Response
    {
        public int CommonValue { get; private set; }

        public string CommonText { get; private set; }

        public Errors Detail { get; private set; }

        public Response(EasWrapper.CommonResult commonResult, EasWrapper.CheckFormData formData)
        {
            CommonValue = commonResult.return_val;
            CommonText = commonResult.return_text;
            Detail = Errors.Parse(formData.data);
        }

    }
}
