namespace FOMS.Api.Endpoints.CustomerObligation.Dto;

public class ObligationFullDto
    : ObligationDto
{
    public int ObligationId { get; set; }

    public int CustomerOnSAId { get; set; }
}
