using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipTester : MonoBehaviour, ITooltipProvider
{
    [SerializeField] private string str;
    public string GetTooltipContent()
    {
        return str;
    }
}
