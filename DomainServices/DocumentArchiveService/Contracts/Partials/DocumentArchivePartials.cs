using MediatR;

namespace DomainServices.DocumentArchiveService.Contracts;

public partial class GetDocumentListRequest : IRequest<GetDocumentListResponse>, CIS.Core.Validation.IValidatableRequest
{
}

public partial class GetDocumentRequest : IRequest<GetDocumentResponse>
{
}

public partial class GenerateDocumentIdRequest : IRequest<Contracts.GenerateDocumentIdResponse>, CIS.Core.Validation.IValidatableRequest
{
}

public partial class UploadDocumentRequest : IRequest, CIS.Core.Validation.IValidatableRequest
{
}