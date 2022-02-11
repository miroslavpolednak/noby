namespace FOMS.Api.SharedHandlers.Requests
{
    internal class SharedCreateCaseRequest
        : IRequest<long>
    {
        public int OfferId { get; set; }
        public int ProductTypeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal TargetAmount { get; set; }
        public CIS.Core.Types.CustomerIdentity? Customer { get; set; }
    }
}
