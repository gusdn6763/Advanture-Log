using UnityEngine;
using UnityEngine.UI;

public class UI_Audio : MonoBehaviour
{
    [SerializeField] private Slider bgmVolume;
    [SerializeField] private Slider effectSoundVolume;
    [SerializeField] private Toggle bgmOn;
    [SerializeField] private Toggle effectSoundOn;

    private SoundSetting soundSetting;

    public void Init()
    {
        soundSetting = Managers.Setting.SoundSetting;
        Managers.Setting.OnReset += Refresh;

        bgmVolume.onValueChanged.AddListener(OnBgmVolume);
        effectSoundVolume.onValueChanged.AddListener(OnSoundVolume);
        bgmOn.onValueChanged.AddListener(OnBgmOn);
        effectSoundOn.onValueChanged.AddListener(OnSoundOn);
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void OnDestroy()
    {
        Managers.Setting.OnReset -= Refresh;
    }

    private void Refresh()
    {
        if (soundSetting == null)
            return;

        // UI에 값 설정 시 이벤트 발동 방지
        bgmVolume.SetValueWithoutNotify(soundSetting.BgmVolume);
        effectSoundVolume.SetValueWithoutNotify(soundSetting.SoundVolume);
        bgmOn.SetIsOnWithoutNotify(soundSetting.BgmIsOn);
        effectSoundOn.SetIsOnWithoutNotify(soundSetting.SoundIsOn);
    }

    private void OnBgmVolume(float value)
    {
        soundSetting.SetBgmVolume(value);
    }
    private void OnSoundVolume(float value)
    {
        soundSetting.SetSoundVolume(value);
    }
    private void OnBgmOn(bool isOn)
    {
        soundSetting.SetBgmOn(isOn);
    }
    private void OnSoundOn(bool isOn)
    {
        soundSetting.SetSoundOn(isOn);
    }
}