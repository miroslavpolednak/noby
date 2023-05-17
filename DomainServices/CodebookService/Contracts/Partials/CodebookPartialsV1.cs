using Google.Protobuf.WellKnownTypes;

namespace DomainServices.CodebookService.Contracts.v1;

public partial class GenericCodebookResponse : IItemsResponse<GenericCodebookResponse.Types.GenericCodebookItem> { }
public partial class GenericCodebookWithCodeResponse : IItemsResponse<GenericCodebookWithCodeResponse.Types.GenericCodebookWithCodeItem> { }
public partial class BankCodesResponse : IItemsResponse<BankCodesResponse.Types.BankCodeItem> { }
public partial class CaseStatesResponse : IItemsResponse<CaseStatesResponse.Types.CaseStateItem> { }
