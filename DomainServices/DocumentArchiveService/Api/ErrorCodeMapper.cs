﻿using CIS.Core.ErrorCodes;

namespace DomainServices.DocumentArchiveService.Api;

internal sealed class ErrorCodeMapper : ErrorCodeMapperBase
{
    public const int FileWithDocumentidExist = 14015;
    public const int ServiceUser2LoginBindingConfigurationNotSet = 14012;
    public const int ServiceUserNotFoundInServiceUser2LoginBinding = 14013;
    public const int UnknownEnvironmentName = 14009;
    public const int OneOfMainParametersFillIn = 14017;
    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { FileWithDocumentidExist, "File with documentid already exist in database" },
            { ServiceUser2LoginBindingConfigurationNotSet, "ServiceUser2LoginBinding configuration is not set" },
            { ServiceUserNotFoundInServiceUser2LoginBinding, "ServiceUser {PropertyValue} not found in ServiceUser2LoginBinding configuration and no _default has been set"},
            { UnknownEnvironmentName, "Unknown EnvironmentName."},
            { OneOfMainParametersFillIn, "One of main parameters have to be fill in (CaseId, PledgeAgreementNumber, ContractNumber, OrderId, AuthorUserLogin)"}
        });

        return Messages;
    }
}