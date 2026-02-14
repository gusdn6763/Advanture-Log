using UnityEngine;

public static class StringUtil
{
    public static string FormatValueForDisplay(float value, StatDisplayType displayType)
    {
        bool isPercent = displayType == StatDisplayType.Percent;

        // 정수처럼 보이면 정수로
        float r = Mathf.Round(value);
        bool intLike = Mathf.Abs(value - r) < 0.0001f;

        string number = intLike ? ((int)r).ToString() : value.ToString("0.#");
        return isPercent ? $"{number}%" : number;
    }
}