using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class GameplaySetting : MonoBehaviour
{
    public event Action OnSavePeriodChaged;

    public int LocaleIndex { get; private set; }
    public int AutoSavePeriod { get; private set; }

    public void SetLocaleIndex(int localeIndex)
    {
        LocaleIndex = localeIndex;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales?.Locales[LocaleIndex];
    }

    public void SetAutoSavePeriod(int period)
    {
        AutoSavePeriod = period;
        OnSavePeriodChaged?.Invoke();
    }

    #region 저장 및 로드
    public void SaveTo(SettingData data)
    {
        data.localeIndex = LocaleIndex;
        data.autoSavePeriod = AutoSavePeriod;
    }
    public void Load(SettingData data)
    {
        SetLocaleIndex(data.localeIndex);
        SetAutoSavePeriod(data.autoSavePeriod);
    }
    #endregion
}