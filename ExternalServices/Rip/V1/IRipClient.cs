using ExternalServices.Rip.V1.RipWrapper;

namespace ExternalServices.Rip.V1
{
    public interface IRipClient
    {
        /// <summary>
        /// Vytvoří Risk Busines Case.
        /// </summary>
        Task<IServiceCallResult> CreateRiskBusinesCase(CreateRequest createRequest);
    }
}
