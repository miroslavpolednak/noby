namespace ExternalServices.Sulm.V1;

public interface ISulmClientHelper
{
    Task StartUse(long kbCustomerId, string purposeCode, CancellationToken cancellationToken = default);

    Task StopUse(long kbCustomerId, string purposeCode, CancellationToken cancellationToken = default);
}
