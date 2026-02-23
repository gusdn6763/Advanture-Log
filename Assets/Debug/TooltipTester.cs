using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipTester : MonoBehaviour, ITooltipProvider
{
    [SerializeField] TooltipData tooltipData;

    public bool TryGetTooltipData(TooltipData data)
    {
        data.tooltipWidthType = tooltipData.tooltipWidthType;
        data.textType = tooltipData.textType;
        data.positionType = tooltipData.positionType;

        data.header = tooltipData.header;
        data.body = tooltipData.body;
        data.bottom = tooltipData.bottom;

        data.headerLines = tooltipData.headerLines;
        data.bodyLines = tooltipData.bodyLines;

        return true;
    }
}
