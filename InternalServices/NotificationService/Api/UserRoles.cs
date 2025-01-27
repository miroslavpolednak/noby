﻿namespace CIS.InternalServices.NotificationService.Api;

public readonly struct UserRoles
{
    public const string ReadResult = "ReadResult";
    public const string SendEmail = "SendEmail";
    public const string SendSms = "SendSms";
    public const string ReceiveStatistics = "ReceiveStatistics";
    public const string ResendNotifications = "ResendNotifications";
}
