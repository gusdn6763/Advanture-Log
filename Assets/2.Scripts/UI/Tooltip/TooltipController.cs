using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    [SerializeField] private UI_Tooltip tooltip;

    private RectTransform currentTarget;
    private TooltipData tooltipData = new TooltipData();

    public void Init()
    {
        RectTransform rect = GetComponent<RectTransform>();

        tooltip.Init(rect);
        tooltip.Hide();
    }

    public void ShowTooltip(ITooltipProvider provider, RectTransform target)
    {
        tooltipData.Clear();

        if (!provider.TryGetTooltipData(tooltipData))
            return;

        currentTarget = target;

        tooltip.Show(tooltipData, currentTarget);
    }

    public void HideTooltip(RectTransform target)
    {
        if (currentTarget != target)
            return;

        tooltip.Hide();
        tooltipData.Clear();
    }
}