using CIS.Core.ErrorCodes;

namespace DomainServices.ProductService.Api;

internal sealed class ErrorCodeMapper : ErrorCodeMapperBase
{
    public const int NotFound12000 = 12000; // no reference found.
    public const int NotFound12001 = 12001;
    public const int InvalidArgument12004 = 12004; // no reference found.
    public const int AlreadyExists12005 = 12005;
    public const int InvalidArgument12008 = 12008;
    public const int InvalidArgument12009 = 12009;
    public const int InvalidArgument12010 = 12010;
    public const int AlreadyExists12011 = 12011; // problem with multiple arguments.
    public const int NotFound12012 = 12012;
    public const int NotFound12013 = 12013;
    public const int InvalidArgument12014 = 12014;
    public const int InvalidArgument12015 = 12015;
    public const int InvalidArgument12016 = 12016;
    public const int InvalidArgument12017 = 12017;
    public const int NotFound12018 = 12018; // problem with multiple arguments.
    public const int InvalidArgument12019 = 12019;
    public const int InvalidArgument12020 = 12020;
    public const int InvalidArgument12021 = 12021;
    // missing LoanKindNotSpecified?
    
    
    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { NotFound12000, "CaseId does not exist." },	
            { NotFound12001, "ProductInstanceId {productInstanceId} does not exist in KonsDB." },	
            { InvalidArgument12004, "Product can be created only with client with partnerId linked to case." },	
            { AlreadyExists12005, "Product already exist." },	
            { InvalidArgument12008, "CaseId is not specified." },	
            { InvalidArgument12009, "ProductTypeId is not specified." },	
            { InvalidArgument12010, "PartnerId is not specified." },	
            { AlreadyExists12011, "Relationship with ProductId {productId} and PartnerId {partnerId} already exists." },	
            { NotFound12012, "Partner {partnerId} not found." },	
            { NotFound12013, "RelationshipCustomerProductTypeItem {contractRelationshipTypeId} not found." },	
            { InvalidArgument12014, "ProductId is not specified." },	
            { InvalidArgument12015, "Relationship not provided." },	
            { InvalidArgument12016, "Relationship PartnerId not specified." },	
            { InvalidArgument12017, "Relationship ContractRelationshipTypeId not specified." },	
            { NotFound12018, "Relationship with ProductId {productId} and PartnerId {partnerId} does not exist." },	
            { InvalidArgument12019, "Unsupported product type." },	
            { InvalidArgument12020, "Customers not found for product." },	
            { InvalidArgument12021, "Not all customers does have KB ID." }
        });
        return Messages;
    }
}