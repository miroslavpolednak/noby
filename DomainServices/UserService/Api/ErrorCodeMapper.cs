﻿using CIS.Core.ErrorCodes;

namespace DomainServices.UserService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int UserNotFound = 21000;
    public const int IdentitySchemeNotExist = 21001;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { UserNotFound, "User {PropertyValue} not found" },
            { IdentitySchemeNotExist, "IdentityScheme {PropertyValue} does not exist" },
        });

        return Messages;
    }
}
