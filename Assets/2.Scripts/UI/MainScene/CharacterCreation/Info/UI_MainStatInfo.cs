using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UI_MainStatInfo : MonoBehaviour
{
    [Header("보여줄 메인 스탯 순서")]
    [SerializeField] private List<MainStatType> order = new List<MainStatType>();

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

    public void Refresh(IReadOnlyDictionary<MainStatType, int> totalMainStatDic)
    {
        string result = string.Empty;

        for (int i = 0; i < order.Count; i++)
        {
            MainStatType type = order[i];

            totalMainStatDic.TryGetValue(type, out int value);

            // showZero=false 이고 0이면 스킵
            if (!showZero && value == 0f)
                continue;

            MainStat stat = new MainStat(type, value);

            if (!string.IsNullOrEmpty(result))
                result += "\n";

            result += $"{stat.Name}: {stat}";
        }

        text.text = result;
    }
}