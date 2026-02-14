using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UI_SubStatIntroduce : MonoBehaviour
{
    [Header("보여줄 서브 스탯 순서")]
    [SerializeField] private List<SubStatType> order = new List<SubStatType>();

    [Header("0인 값도 보여줄 것인지")]
    [SerializeField] private bool showZero;

    private TextMeshProUGUI text;

    public void Init()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Refresh(string str)
    {
        text.text = str;
    }

    public void Refresh(IReadOnlyDictionary<SubStatType, float> totalSubStatDic)
    {
        string result = string.Empty;

        for (int i = 0; i < order.Count; i++)
        {
            SubStatType type = order[i];

            totalSubStatDic.TryGetValue(type, out float value);

            // showZero=false 이고 0이면 스킵
            if (showZero && value == 0f)
                continue;

            SubStat stat = new SubStat(type, value);

            if (!string.IsNullOrEmpty(result))
                    result += "\n";

            result += $"{stat.Name}: {stat.DisplayValue}";
        }

        text.text = result;
    }
}