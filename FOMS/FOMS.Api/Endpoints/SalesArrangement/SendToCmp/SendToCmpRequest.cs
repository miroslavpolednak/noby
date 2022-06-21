namespace FOMS.Api.Endpoints.SalesArrangement.SendToCmp;

public sealed class SendToCmpRequest: IRequest<SendToCmpResponse>
{
     public int SalesArrangementId { get; set; }
}
