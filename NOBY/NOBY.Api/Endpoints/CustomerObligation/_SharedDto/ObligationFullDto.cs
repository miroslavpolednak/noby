namespace NOBY.Api.Endpoints.CustomerObligation.SharedDto;

public class ObligationFullDto
    : ObligationDto
{
    public int ObligationId { get; set; }

    public int CustomerOnSAId { get; set; }
}
