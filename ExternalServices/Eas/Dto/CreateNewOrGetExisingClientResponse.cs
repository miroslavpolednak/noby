namespace ExternalServices.Eas.Dto;

public class CreateNewOrGetExisingClientResponse
{
    public int Id { get; set; }
    public long? KbId { get; set; }
    public string? BirthNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
