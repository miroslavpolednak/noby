using CIS.Core.ErrorCodes;

namespace DomainServices.ProductService.Api;

internal sealed class ErrorCodeMapper : ErrorCodeMapperBase
{
    public const int NotFound12001 = 12001;
    public const int InvalidArgument12004 = 12004;
    public const int AlreadyExists12005 = 12005;
    public const int InvalidArgument12007 = 12007;
    public const int InvalidArgument12008 = 12008;
    public const int InvalidArgument12009 = 12009;
    public const int InvalidArgument12010 = 12010;
    public const int AlreadyExists12011 = 12011;
    public const int NotFound12012 = 12012;
    public const int NotFound12013 = 12013;
    public const int InvalidArgument12014 = 12014;
    public const int InvalidArgument12015 = 12015;
    public const int InvalidArgument12016 = 12016;
    public const int InvalidArgument12017 = 12017;
    public const int NotFound12018 = 12018;
    public const int InvalidArgument12019 = 12019;
    public const int InvalidArgument12020 = 12020;
    public const int InvalidArgument12021 = 12021;
    public const int PaymentAccountNotFound = 12022;
    public const int ContractNumberNotFound = 12023;
    public const int NotFound12024 = 12024;
    
    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>
        {
            { NotFound12001, "ProductInstanceId {PropertyValue} not found in KonsDB." },	
            { InvalidArgument12004, "Product can be created only when client and partnerId are linked to the case." },	
            { AlreadyExists12005, "Product {PropertyValue} already exist." },
            { InvalidArgument12007, "LoanKindId is not provided." },
            { InvalidArgument12008, "CaseId is not provided." },	
            { InvalidArgument12009, "ProductTypeId is not provided." },	
            { InvalidArgument12010, "PartnerId is not provided." },	
            { AlreadyExists12011, "Relationship with ProductId {0} and PartnerId {1} already exists." },	
            { NotFound12012, "Partner {PropertyValue} not found." },	
            { NotFound12013, "Relationship CustomerProductTypeItem {PropertyValue} not found." },	
            { InvalidArgument12014, "ProductId is not provided." },	
            { InvalidArgument12015, "Relationship is not provided." },	
            { InvalidArgument12016, "Relationship PartnerId is not provided." },	
            { InvalidArgument12017, "Relationship ContractRelationshipTypeId is not provided." },	
            { NotFound12018, "Relationship with ProductId {0} and PartnerId {1} not found." },	
            { InvalidArgument12019, "Unsupported product type." },	
            { InvalidArgument12020, "Customers for product not found." },	
            { InvalidArgument12021, "Some customers do not have KB ID." },
            { PaymentAccountNotFound, "Payment account {PropertyValue} not found." },
            { ContractNumberNotFound, "Contract number {PropertyValue} not found." },
            { NotFound12024, "Covenant not found." }
        });
        return Messages;
    }
}