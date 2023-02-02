using Google.Protobuf.WellKnownTypes;

namespace DomainServices.CaseService.Contracts;

public partial class CreateCaseRequest
    : MediatR.IRequest<CreateCaseResponse>, CIS.Core.Validation.IValidatableRequest, CIS.Infrastructure.CisMediatR.Rollback.IRollbackCapable
{ }

public partial class GetCaseDetailRequest
    : MediatR.IRequest<Case>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class SearchCasesRequest
    : MediatR.IRequest<SearchCasesResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class LinkOwnerToCaseRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateCaseDataRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateCaseStateRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateCustomerDataRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetCaseCountsRequest
    : MediatR.IRequest<GetCaseCountsResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class DeleteCaseRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class GetTaskListRequest
    : MediatR.IRequest<GetTaskListResponse>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateActiveTasksRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }

public partial class UpdateOfferContactsRequest
    : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest
{ }