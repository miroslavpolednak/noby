using Google.Protobuf.WellKnownTypes;

namespace DomainServices.RealEstateValuationService.Contracts;

public partial class CreateRealEstateValuationRequest
    : MediatR.IRequest<CreateRealEstateValuationResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class DeleteRealEstateValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetRealEstateValuationListRequest
    : MediatR.IRequest<GetRealEstateValuationListResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class PatchDeveloperOnRealEstateValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetRealEstateValuationDetailRequest
    : MediatR.IRequest<RealEstateValuationDetail>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetRealEstateValuationDetailByOrderIdRequest
    : MediatR.IRequest<RealEstateValuationDetail>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateRealEstateValuationDetailRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SetACVRealEstateTypeByRealEstateValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateStateByRealEstateValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class AddDeedOfOwnershipDocumentRequest
    : MediatR.IRequest<AddDeedOfOwnershipDocumentResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class DeleteDeedOfOwnershipDocumentRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class CreateRealEstateValuationAttachmentRequest
    : MediatR.IRequest<CreateRealEstateValuationAttachmentResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class DeleteRealEstateValuationAttachmentRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }