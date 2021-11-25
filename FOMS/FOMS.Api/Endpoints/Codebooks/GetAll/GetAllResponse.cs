namespace FOMS.Api.Endpoints.Codebooks.Dto;

internal class GetAllResponseItem
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public GetAllResponseItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public GetAllResponseItem(string code, object codebook)
    {
        Code = code;
        Codebook = codebook;
    }

    public string Code { get; set; }
    public object Codebook { get; set; }
}
