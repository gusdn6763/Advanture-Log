using System;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public event Action OnReset;

    [SerializeField] private DefaultSettingSo defaultSettingSo;

    [SerializeField] private GraphicsSetting graphicsSetting;
    [SerializeField] private SoundSetting soundSetting;
    [SerializeField] private GameplaySetting gamePlaySetting;
    [SerializeField] private KeySetting keySetting;

    private SettingData settingData = new SettingData();
    public GraphicsSetting GraphicsSetting => graphicsSetting;
    public SoundSetting SoundSetting => soundSetting;
    public GameplaySetting GameplaySetting => gamePlaySetting;
    public KeySetting KeySetting => keySetting;

    public void Init()
    {
        SoundSetting.Init();
        KeySetting.Init(defaultSettingSo);

        SettingData settingData = Managers.Save.LoadSettingData(defaultSettingSo);

        // Àû¿ë
        GraphicsSetting.Load(settingData);
        SoundSetting.Load(settingData);
        GameplaySetting.Load(settingData);
        KeySetting.Load(settingData);
        GameplaySetting.OnSavePeriodChaged += Managers.Game.AutoSavePeriodChanged;
    }


    public void SaveSetting()
    {
        GraphicsSetting.SaveTo(settingData);
        SoundSetting.SaveTo(settingData);
        GameplaySetting.SaveTo(settingData);
        KeySetting.SaveTo(settingData);

        Managers.Save.SaveSettingData(settingData);
    }

    public void ResetSettingData()
    {
        settingData.Init(defaultSettingSo);

        GraphicsSetting.Load(settingData);
        SoundSetting.Load(settingData);
        GameplaySetting.Load(settingData);
        KeySetting.Load(settingData);

        Managers.Save.SaveSettingData(settingData);

        OnReset?.Invoke();
    }
}