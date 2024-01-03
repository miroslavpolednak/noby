using SharedComponents.DocumentDataStorage;
using SharedTypes.Types;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal sealed class CustomerChangeData : IDocumentData
{
    int IDocumentData.Version => 1;

    public IEnumerable<CustomerChangeApplicantData> Applicants { get; set; } = Enumerable.Empty<CustomerChangeApplicantData>();
    public CustomerChangeReleaseData? Release { get; set; }
    public CustomerChangeAddData? Add { get; set; }
    public CustomerChangeAgentData? Agent { get; set; }
    public CustomerChangePaymentAccountData? RepaymentAccount { get; set; }
    public CustomerChangeCommentToChangeRequestData? CommentToChangeRequest { get; set; }

    public class CustomerChangeApplicantData
    {
        public IEnumerable<CustomerIdentity> Identity { get; set; } = Enumerable.Empty<CustomerIdentity>();
        public CustomerChangeNaturalPersonData? NaturalPerson { get; set; }
        public CustomerChangeIdentificationDocumentData? IdentificationDocument { get; set; }
    }

    public class CustomerChangeNaturalPersonData
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
    }

    public class CustomerChangeIdentificationDocumentData
    {
        public int IdentificationDocumentTypeId { get; set; }
        public string Number { get; set; } = null!;
    }

    public class CustomerChangeReleaseData
    {
        public bool IsActive { get; set; }
        public IEnumerable<CustomerChangeReleaseCustomerData> Customers { get; set; } = Enumerable.Empty<CustomerChangeReleaseCustomerData>();
    }

    public class CustomerChangeReleaseCustomerData
    {
        public CustomerIdentity Identity { get; set; } = null!;
        public CustomerChangeNaturalPersonData? NaturalPerson { get; set; }
    }

    public class CustomerChangeAddData
    {
        public bool IsActive { get; set; }
        public IEnumerable<CustomerChangeAddCustomerData> Customers { get; set; } = Enumerable.Empty<CustomerChangeAddCustomerData>();
    }

    public class CustomerChangeAddCustomerData
    {
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
    }

    public class CustomerChangeAgentData
    {
        public bool IsActive { get; set; }
        public string ActualAgent { get; set; } = null!;
        public string? NewAgent { get; set; }
    }

    public class CustomerChangePaymentAccountData
    {
        public bool IsActive { get; set; }
        public string AgreedPrefix { get; set; } = null!;
        public string AgreedNumber { get; set; } = null!;
        public string AgreedBankCode { get; set; } = null!;
        public string? Prefix { get; set; }
        public string? Number { get; set; }
        public string? BankCode { get; set; }
        public string? OwnerFirstName { get; set; }
        public string? OwnerLastName { get; set; }
        public DateTime? OwnerDateOfBirth { get; set; }
    }

    public class CustomerChangeCommentToChangeRequestData
    {
        public bool IsActive { get; set; }
        public string? GeneralComment { get; set; }
    }

}