namespace ExternalServices.Eas.R21
{
    public class CommonResponse
    {
        public int CommonValue { get; private set; }

        public string CommonText { get; private set; }

        public CommonResponse(EasWrapper.CommonResult commonResult)
        {
            CommonValue = commonResult.return_val;
            CommonText = commonResult.return_text;
        }
    }
}
