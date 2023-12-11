﻿using Google.Protobuf.WellKnownTypes;

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

public partial class SetForeignRealEstateTypesByRealEstateValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateStateByRealEstateValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateValuationTypeByRealEstateValuationRequest
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

public partial class UpdateDeedOfOwnershipDocumentRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class ValidateRealEstateValuationIdRequest
    : MediatR.IRequest<ValidateRealEstateValuationIdResponse>
{ }

public partial class GetDeedOfOwnershipDocumentsRequest
    : MediatR.IRequest<GetDeedOfOwnershipDocumentsResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetDeedOfOwnershipDocumentRequest
    : MediatR.IRequest<DeedOfOwnershipDocument>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetRealEstateValuationTypesRequest
    : MediatR.IRequest<GetRealEstateValuationTypesReponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class PreorderOnlineValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class OrderOnlineValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class OrderStandardValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class OrderDTSValuationRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }