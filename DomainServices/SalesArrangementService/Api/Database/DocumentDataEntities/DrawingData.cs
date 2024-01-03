using SharedComponents.DocumentDataStorage;
using SharedTypes.Types;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal sealed class DrawingData : IDocumentData
{
    int IDocumentData.Version => 1;

    public IEnumerable<CustomerIdentity> Applicant { get; set; } = Enumerable.Empty<CustomerIdentity>();

    public DrawingAgentData? Agent { get; set; }

    public DrawingRepaymentAccount? RepaymentAccount { get; set; }

    public IEnumerable<DrawingPayoutListItem> PayoutList { get; set; } = Enumerable.Empty<DrawingPayoutListItem>();

    public DateTime? DrawingDate { get; set; }

    public bool IsImmediateDrawing { get; set; }

    public class DrawingAgentData
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public bool IsActive { get; set; }

        public DrawingAgentIdentificationDocumentData? IdentificationDocument { get; set; }

        public class DrawingAgentIdentificationDocumentData
        {
            public int IdentificationDocumentTypeId { get; set; }

            public string Number { get; set; } = null!;
        }
    }

    public class DrawingRepaymentAccount
    {
        public bool IsAccountNumberMissing { get; set; }

        public string Prefix { get; set; } = null!;

        public string Number { get; set; } = null!;

        public string BankCode { get; set; } = null!;
    }

    public class DrawingPayoutListItem
    {
        public int? ProductObligationId { get; set; }

        public int Order { get; set; }

        public decimal? DrawingAmount { get; set; }

        public string PrefixAccount { get; set; } = null!;

        public string AccountNumber { get; set; } = null!;

        public string BankCode { get; set; } = null!;

        public string? VariableSymbol { get; set; }

        public string? SpecificSymbol { get; set; }

        public string? ConstantSymbol { get; set; }

        public int PayoutTypeId { get; set; }
    }
}