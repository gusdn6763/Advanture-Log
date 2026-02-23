using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyData
{
    public InputAction inputAction;
    public KeyCode keyCode;

    public KeyData(InputAction inputAction, KeyCode keyCode)
    {
        this.inputAction = inputAction;
        this.keyCode = keyCode;
    }
}

[Serializable]
public class SettingData
{
    // ---------- Graphics ----------
    public int resolutionW;
    public int resolutionH;
    public bool isFullscreen;
    public bool gridOn;
    public float effectIntensity;
    public float brightness;

    // ---------- Sound ----------
    public string currentBgm;
    public float soundVolume;
    public float bgmVolume;
    public bool soundIsOn;
    public bool bgmIsOn;

    // ---------- GamePlay ----------
    public int localeIndex;
    public int autoSavePeriod;

    // ---------- Control ----------
    public List<KeyData> keyData;

    public void Init(DefaultSettingSo settingData)
    {
        // ---------- Graphics ----------
        resolutionW = settingData.ResolutionW;
        resolutionH = settingData.ResolutionH;
        isFullscreen = settingData.IsFullscreen;
        gridOn = settingData.GridOn;
        effectIntensity = settingData.EffectIntensity;
        brightness = settingData.Brightness;

        // ---------- Sound ----------
        currentBgm = settingData.StartBgm != null ? settingData.StartBgm.name : string.Empty;
        soundVolume = settingData.SoundVolume;
        bgmVolume = settingData.BgmVolume;
        soundIsOn = settingData.SoundIsOn;
        bgmIsOn = settingData.BgmIsOn;

        // ---------- GamePlay ----------
        localeIndex = settingData.LocaleIndex;
        autoSavePeriod = settingData.AutoSavePeriod;

        // ---------- Control ----------
        keyData.Clear();
        foreach (KeyValuePair<InputAction, InputActionData> kv in settingData.InputActionDic)
            keyData.Add(new KeyData(kv.Key, kv.Value.keyCode));
    }
}