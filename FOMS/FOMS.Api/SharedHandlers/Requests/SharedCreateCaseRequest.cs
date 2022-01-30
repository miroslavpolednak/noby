namespace FOMS.Api.SharedHandlers.Requests
{
    internal class SharedCreateCaseRequest
        : IRequest<long>
    {
        public int OfferInstanceId { get; set; }
        public int ProductInstanceTypeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? TargetAmount { get; set; }
        public CIS.Core.Types.CustomerIdentity? Customer { get; set; }
    }
}
