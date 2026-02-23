using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

public static class LocalizeUtil
{
    public static string Get(string table, string key, params object[] args)
    {
        try
        {
            if (args == null || args.Length == 0)
                return LocalizationSettings.StringDatabase.GetLocalizedString(table, key);
            else
                return LocalizationSettings.StringDatabase.GetLocalizedString(table, key, args);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[Loc] Missing or error: {table}:{key} - {e.Message}");
            return $"#{table}:{key}#";
        }
    }

    public static string UI(string key, params object[] args)
    {
        return Get("UI", key, args);
    }
}