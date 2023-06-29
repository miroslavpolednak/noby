using MediatR;

namespace CIS.InternalServices.DataAggregatorService.Contracts;

public partial class GetDocumentDataRequest : IRequest<GetDocumentDataResponse> { }

public partial class GetEasFormRequest : IRequest<GetEasFormResponse> { }

public partial class GetRiskLoanApplicationDataRequest : IRequest<GetRiskLoanApplicationDataResponse> { }