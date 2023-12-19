using Google.Protobuf.WellKnownTypes;
using MediatR;
namespace DomainServices.DocumentOnSAService.Contracts;

public partial class GenerateFormIdRequest : IRequest<GenerateFormIdResponse> { }

public partial class StartSigningRequest : IRequest<StartSigningResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class StopSigningRequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

public partial class GetDocumentsToSignListRequest : IRequest<GetDocumentsToSignListResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class GetDocumentOnSADataRequest : IRequest<GetDocumentOnSADataResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class SignDocumentRequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

public partial class GetDocumentsOnSAListRequest :IRequest<GetDocumentsOnSAListResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class CreateDocumentOnSARequest : IRequest<CreateDocumentOnSAResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class LinkEArchivIdToDocumentOnSARequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

public partial class GetElectronicDocumentFromQueueRequest : IRequest<GetElectronicDocumentFromQueueResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class GetElectronicDocumentPreviewRequest : IRequest<GetElectronicDocumentPreviewResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class SetDocumentOnSAArchivedRequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

public partial class SendDocumentPreviewRequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

public partial class RefreshElectronicDocumentRequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

public partial class GetDocumentOnSAByFormIdRequest : IRequest<GetDocumentOnSAByFormIdResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class RefreshSalesArrangementStateRequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

public partial class SetProcessingDateInSbQueuesRequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

public partial class GetDocumentOnSAStatusRequest : IRequest<GetDocumentOnSAStatusResponse>, CIS.Core.Validation.IValidatableRequest { }
