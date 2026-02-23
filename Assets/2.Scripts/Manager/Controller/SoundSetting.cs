using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private int audioSourcePoolingCount = 10;
    // 캐싱된 클립 맵
    [SerializeField] private SerializedDictionary<string, AudioClip> bgmClipDic = new SerializedDictionary<string, AudioClip>();
    [SerializeField] private SerializedDictionary<string, AudioClip> sfxClipDic = new SerializedDictionary<string, AudioClip>();


    private AudioSource[] effectSources;
    private AudioSource bgmSource;

    public bool SoundIsOn { get; private set; }
    public bool BgmIsOn { get; private set; }
    public float BgmVolume { get; private set; }
    public float SoundVolume { get; private set; }

    public void Init()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        effectSources = new AudioSource[audioSourcePoolingCount];
        for (int i = 0; i < audioSourcePoolingCount; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            effectSources[i] = source;
        }
    }

    public void SetBgmOn(bool on)
    {
        BgmIsOn = on;

        if (on)
        {
            if (bgmSource.clip != null && !bgmSource.isPlaying)
                bgmSource.Play();
        }
        else
            bgmSource.Stop();
    }

    public void SetBgmVolume(float volume)
    {
        BgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = BgmVolume;
    }

    public void SetSoundOn(bool on)
    {
        SoundIsOn = on;

        if (!on)
            for (int i = 0; i < effectSources.Length; i++)
                effectSources[i].Stop();
    }

    public void SetSoundVolume(float volume)
    {
        SoundVolume = Mathf.Clamp01(volume);

        for (int i = 0; i < effectSources.Length; i++)
            effectSources[i].volume = SoundVolume;
    }

    public void PlayBgm(string id)
    {
        if (string.IsNullOrEmpty(id))
            return;

        if (!bgmClipDic.TryGetValue(id, out AudioClip clip))
        {
            Debug.LogError($"[SoundSetting] 등록되지 않은 BGM: '{id}'");
            return;
        }

        if (bgmSource.clip == clip)
        {
            if (BgmIsOn && !bgmSource.isPlaying)
                bgmSource.Play();
            return;
        }

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.volume = BgmVolume;

        if (BgmIsOn)
            bgmSource.Play();
    }

    public void PlaySound(string id)
    {
        if (!SoundIsOn)
            return;

        if (string.IsNullOrEmpty(id))
            return;

        if (!sfxClipDic.TryGetValue(id, out AudioClip clip))
        {
            Debug.LogError($"[SoundSetting] 등록되지 않은 SFX: '{id}'");
            return;
        }

        for (int i = 0; i < effectSources.Length; i++)
        {
            if (!effectSources[i].isPlaying)
            {
                effectSources[i].clip = clip;
                effectSources[i].volume = SoundVolume;
                effectSources[i].Play();
                return;
            }
        }

        Debug.Log("모든 가용 AudioSource가 사용중입니다.");
    }

    #region 저장 및 로드 (옵션만)
    public void Load(SettingData data)
    {
        SetSoundOn(data.soundIsOn);
        SetBgmOn(data.bgmIsOn);
        SetBgmVolume(data.bgmVolume);
        SetSoundVolume(data.soundVolume);
    }

    public void SaveTo(SettingData data)
    {
        data.soundIsOn = SoundIsOn;
        data.bgmIsOn = BgmIsOn;
        data.bgmVolume = BgmVolume;
        data.soundVolume = SoundVolume;
    }
    #endregion
}