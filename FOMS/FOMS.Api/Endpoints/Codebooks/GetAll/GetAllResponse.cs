using System.ComponentModel.DataAnnotations;

namespace FOMS.Api.Endpoints.Codebooks.GetAll;

public class GetAllResponseItem
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public GetAllResponseItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public GetAllResponseItem(string code, IEnumerable<object> codebook)
    {
        Code = code;
        Codebook = codebook;
    }

    [Display(Name = "Codebook code")]
    public string Code { get; set; }

    [Display(Name = "Codebook data")]
    public IEnumerable<object> Codebook { get; set; }
}
