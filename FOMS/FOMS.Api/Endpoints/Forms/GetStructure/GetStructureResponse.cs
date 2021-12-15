namespace FOMS.Api.Endpoints.Forms.Dto;

internal sealed class GetStructureResponse
{
    public List<Step> Steps { get; set; }

    public class Step
    {
        public string Icon { get; set; }
        public string ShortName { get; set; }
    }
}
