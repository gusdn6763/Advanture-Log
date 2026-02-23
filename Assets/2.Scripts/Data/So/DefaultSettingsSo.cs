using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class InputActionData
{    
    public KeyCode keyCode;

    [field: SerializeField] public LocalizedString DisplayName { get; private set; }
    [field: SerializeField] public InputCategory Category { get; private set; }

    public InputActionData() { }

    public InputActionData(InputActionData other)
    {
        Category = other.Category;
        DisplayName = other.DisplayName;    //클래스라 얕은 복사
        keyCode = other.keyCode;
    }
}

[CreateAssetMenu(menuName = "Game/Rule/DefaultSetting", fileName = "DefaultSettingSo")]
public class DefaultSettingSo : ScriptableObject
{
    [field: Header("그래픽")]
    [field: SerializeField, Min(640)] public int ResolutionW { get; private set; }
    [field: SerializeField, Min(480)] public int ResolutionH { get; private set; }
    [field: SerializeField] public bool IsFullscreen { get; private set; }
    [field: SerializeField] public bool GridOn { get; private set; }
    [field: SerializeField, Range(0, 1)] public float EffectIntensity { get; private set; }
    [field: SerializeField, Range(0, 1)] public float Brightness { get; private set; }

    //-------------------------------------------

    [field: Header("소리")]
    [field: SerializeField] public AudioClip StartBgm { get; private set; }
    [field: SerializeField] public float SoundVolume { get; private set; }
    [field: SerializeField] public float BgmVolume { get; private set; }
    [field: SerializeField] public bool SoundIsOn { get; private set; }
    [field: SerializeField] public bool BgmIsOn { get; private set; }

    //-------------------------------------------

    [field: Header("게임플레이")]
    [field: SerializeField] public int LocaleIndex { get; private set; }
    [field: SerializeField] public int AutoSavePeriod { get; private set; }

    //-------------------------------------------

    [Header("조작")]
    [SerializeField] private SerializedDictionary<InputAction, InputActionData> inputActionDic;
    public IReadOnlyDictionary<InputAction, InputActionData> InputActionDic => inputActionDic;
}