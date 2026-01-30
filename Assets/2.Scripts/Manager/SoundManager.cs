using System.Collections;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // 곡의 이름.
    public AudioClip clip; // 곡.
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip startBgm;

    //모든 사운드
    private AudioSource[] audioSourceEffects;
    //현재 실행중인브금
    private AudioSource audioSourceBgm;

    //현재 실행중인 사운드 이름
    private string[] playSoundName;

    //현재 실행중인 브금 이름
    private string currentPlayBgmName;
    public string CurrentPlayBgmName { get => currentPlayBgmName; }

    private Sound[] effectSounds;
    private Sound[] bgmSounds;

    private bool soundIsOn = true;
    public bool SoundIsOn { get => soundIsOn; }
    private bool bgmIsOn = true;
    public bool BgmIsOn { get => bgmIsOn; }

    private float bgmVolume = 1f;
    public float BgmVolume { get => bgmVolume; }

    private float soundVolume = 1f;
    public float SoundVolume { get => soundVolume; }

    public void Init()
    {
        audioSourceEffects = GetComponentsInChildren<AudioSource>();

        audioSourceBgm.clip = startBgm;
        playSoundName = new string[audioSourceEffects.Length];
        currentPlayBgmName = audioSourceBgm.name;

        if (bgmIsOn)
            audioSourceBgm.Play();
    }

    public void ApplyAll(bool bgmOn, float bgmVol, bool sfxOn, float sfxVol)
    {
        SetBgmOn(bgmOn);
        SetBgmVolume(bgmVol);
        SetSoundOn(sfxOn);
        SetSoundVolume(sfxVol);
    }
    #region 음악
    #region BGM
    public void SetBgmOn(bool on)
    {
        bgmIsOn = on;
        if (on)
        {
            if (!audioSourceBgm.isPlaying) 
                audioSourceBgm.Play();
        }
        else
        {
            audioSourceBgm.Stop();
        }
    }
    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        audioSourceBgm.volume = bgmVolume;
    }
    public void PlayBgm(string name = null)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (name == bgmSounds[i].name)
            {
                if (name != null)
                    currentPlayBgmName = name;

                audioSourceBgm.Stop();
                audioSourceBgm.clip = bgmSounds[i].clip;
                audioSourceBgm.Play();
                return;
            }
        }
    }
    #endregion

    #region Sound
    public void SetSoundOn(bool on)
    {
        soundIsOn = on;
        if (!on)
        {
            for (int i = 0; i < audioSourceEffects.Length; i++)
                audioSourceEffects[i].Stop();
        }
    }
    public void SetSoundVolume(float volume)
    {
        soundVolume = Mathf.Clamp01(volume);
        for (int i = 0; i < audioSourceEffects.Length; i++)
            audioSourceEffects[i].volume = soundVolume;
    }
    public void PlaySound(string _name)
    {
        if (soundIsOn)
        {
            for (int i = 0; i < effectSounds.Length; i++)
            {
                if (_name == effectSounds[i].name)
                {
                    for (int j = 0; j < audioSourceEffects.Length; j++)
                    {
                        if (!audioSourceEffects[j].isPlaying)
                        {
                            playSoundName[j] = effectSounds[i].name;
                            audioSourceEffects[j].clip = effectSounds[i].clip;
                            audioSourceEffects[j].Play();
                            return;
                        }
                    }
                    Debug.Log("모든 가용 AudioSource가 사용중입니다.");
                    return;
                }
            }
            Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");
        }
    }
    public void StopSound(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다.");
    }
    #endregion
    #endregion
}
