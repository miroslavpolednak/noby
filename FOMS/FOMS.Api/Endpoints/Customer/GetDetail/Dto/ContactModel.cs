namespace FOMS.Api.Endpoints.Customer.GetDetail.Dto;

public class ContactModel
{
    public bool IsPrimary { get; set; }
    public int? ContactTypeId { get; set; }
    public string? Value { get; set; }
}