using Google.Protobuf.WellKnownTypes;
using MediatR;
namespace DomainServices.DocumentOnSAService.Contracts;

public partial class GenerateFormIdRequest : IRequest<GenerateFormIdResponse> { }

public partial class StartSigningRequest : IRequest<StartSigningResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class StopSigningRequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

public partial class GetDocumentsToSignListRequest : IRequest<GetDocumentsToSignListResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class GetDocumentOnSADataRequest : IRequest<GetDocumentOnSADataResponse>, CIS.Core.Validation.IValidatableRequest { }

public partial class SignDocumentManuallyRequest : IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

