using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UI_MainStatInfo : MonoBehaviour
{
    [Header("보여줄 메인 스탯 순서")]
    [SerializeField] private List<MainStatType> order = new List<MainStatType>();

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

            MainStat stat = new MainStat(type, value);

            if (!string.IsNullOrEmpty(result))
                result += "\n";

            result += $"{stat.Name}: {value}";
        }

        text.text = result;
    }
}