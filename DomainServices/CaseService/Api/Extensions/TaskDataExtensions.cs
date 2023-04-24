﻿namespace DomainServices.CaseService.Api;

internal static class TaskDataExtensions
{
    public static int GetInteger(this IReadOnlyDictionary<string, string> taskData, string key)
    {
        return int.Parse(taskData[key], CultureInfo.InvariantCulture);
    }

    public static DateTime GetDate(this IReadOnlyDictionary<string, string> taskData, string key)
    {
        return DateTime.Parse(taskData[key], CultureInfo.InvariantCulture);
    }

    public static bool GetBoolean(this IReadOnlyDictionary<string, string> taskData, string key)
    {
        return Convert.ToBoolean(int.Parse(taskData[key], CultureInfo.InvariantCulture));
    }
}