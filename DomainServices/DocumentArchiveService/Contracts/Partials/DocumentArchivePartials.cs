using Google.Protobuf.WellKnownTypes;
using MediatR;

namespace DomainServices.DocumentArchiveService.Contracts;

public partial class GetDocumentListRequest : IRequest<GetDocumentListResponse>, CIS.Core.Validation.IValidatableRequest{ }

public partial class GetDocumentRequest : IRequest<GetDocumentResponse>{ }

public partial class GenerateDocumentIdRequest : IRequest<Contracts.GenerateDocumentIdResponse>, CIS.Core.Validation.IValidatableRequest{ }

public partial class UploadDocumentRequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest{ }

public partial class GetDocumentsInQueueRequest : IRequest<GetDocumentsInQueueResponse>{ }