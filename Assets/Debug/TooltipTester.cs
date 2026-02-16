using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipTester : MonoBehaviour, ITooltipProvider
{
    [SerializeField] TooltipData tooltipData;

    public bool TryTooltipData(TooltipData data)
    {
        data = tooltipData;
        return true;
    }
}
