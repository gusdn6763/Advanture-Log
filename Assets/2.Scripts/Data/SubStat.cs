using UnityEngine;
using UnityEngine.Localization;

public class SubStat
{
    public SubStatType Type { get; }
    public float Value { get; private set; }

    private readonly LocalizedString name;
    private readonly StatDisplayType displayType;
    private readonly float maxValue;
    private readonly bool hasRule;

    public SubStat(SubStatType type, float value)
    {
        Type = type;

        StatRuleSo statRuleSo = Managers.Data.StatRule;

        if (!statRuleSo.TryGet(type, out SubStatRule rule))
        {
            Debug.LogError($"[SubStat] StatRule 없음: {type}");
            name = default;
            displayType = default;
            maxValue = float.MaxValue;
            hasRule = false;
            Value = value;
            return;
        }

        name = rule.StatName;
        displayType = rule.DisplayType;
        maxValue = rule.MaxValue;
        hasRule = true;

        SetValue(value);
    }

    public void SetValue(float value)
    {
        if (hasRule && maxValue > 0f && maxValue < float.MaxValue)
        {
            // 음수는 보통 허용하지 않는 방향으로 고정 (필요하면 정책 바꿔도 됨)
            Value = Mathf.Clamp(value, 0f, maxValue);
        }
        else
        {
            Value = value;
        }
    }

    public string GetName()
    {
        try
        {
            return name.GetLocalizedString();
        }
        catch
        {
            return string.Empty;
        }
    }

    public string GetValueString()
    {
        // 프로젝트 기존 유틸 사용 (퍼센트/정수 표시는 여기서 통일)
        return StringUtil.FormatValueForDisplay(Value, displayType);
    }
}
