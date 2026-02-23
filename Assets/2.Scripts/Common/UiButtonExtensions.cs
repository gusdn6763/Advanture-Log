using System;
using UnityEngine;
using UnityEngine.UI;

public static class UiButtonExtensions
{
    /// <summary>
    /// 클릭 리스너를 "하나"로 교체.
    /// </summary>
    public static void SetClick(this Button button, Action onClick)
    {
        if (!button)
        {
            Debug.LogWarning("SetClick called with null Button.");
            return;
        }

        button.onClick.RemoveAllListeners();

        if (onClick == null)
            return;

        button.onClick.AddListener(() => onClick());
    }

    /// <summary>
    /// 기존 리스너를 유지한 채로 추가.
    /// </summary>
    public static void AddClick(this Button button, Action onClick)
    {
        if (!button || onClick == null) 
            return;

        button.onClick.AddListener(() => onClick());
    }

    public static void ClearClick(this Button button)
    {
        if (!button) 
            return;

        button.onClick.RemoveAllListeners();
    }
}