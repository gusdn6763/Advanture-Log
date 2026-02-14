using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UI_SubStatIntroduce : MonoBehaviour
{
    [Tooltip("보여줄 서브 스탯 순서")]
    [SerializeField] private List<SubStatType> order = new List<SubStatType>();

    private TextMeshProUGUI text;

    public void Init()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Refresh(string str)
    {
        text.text = str;
    }

    public void Refresh(IReadOnlyDictionary<SubStatType, SubStatRule> subStatRuleDic, IReadOnlyDictionary<SubStatType, float> totalSubStatDic, bool showZero = true)
    {
        string result = string.Empty;

        for (int i = 0; i < order.Count; i++)
        {
            SubStatType type = order[i];

            if (!subStatRuleDic.TryGetValue(type, out SubStatRule rule))
            {
                Debug.LogError($"존재하지 않는 서브 데이터 규칙: {type}");
                continue;
            }

            totalSubStatDic.TryGetValue(type, out float value);

            // showZero=false 이고 0이면 스킵
            if (!showZero && value == 0f)
                continue;

            string name = rule.StatName.GetLocalizedString();
            string valStr = StringUtil.FormatValueForDisplay(value, rule.DisplayType);

            if (!string.IsNullOrEmpty(result))
                    result += "\n";

            result += $"{name}: {valStr}";
        }

        text.text = result;
    }
}