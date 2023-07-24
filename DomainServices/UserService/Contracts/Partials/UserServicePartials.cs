﻿namespace DomainServices.UserService.Contracts;

public partial class GetUserRequest
    : MediatR.IRequest<User>
{ }

public partial class GetUserPermissionsRequest
    : MediatR.IRequest<GetUserPermissionsResponse>
{ }

public partial class GetUserRIPAttributesRequest
    : MediatR.IRequest<UserRIPAttributes>
{ }