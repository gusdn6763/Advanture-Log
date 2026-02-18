using System;
using System.Collections.Generic;

//구조체는 클래스와 달리 개별 힙으로 저장되지 않음
public struct TooltipLineData
{
    public readonly string Left;
    public readonly string Right;

    public TooltipLineData(string left, string right)
    {
        Left = left;
        Right = right;
    }
}

[Serializable]
public class TooltipData
{
    public TooltipWidthType tooltipWidthType;
    public TooltipTextAlignType textType;
    public TooltipPositionType positionType;

    public string header;
    public List<TooltipLineData> headerLines;
    public string body;
    public string bottom;

    public List<TooltipLineData> bodyLines;

    public TooltipData()
    {
        headerLines = new List<TooltipLineData>(2);
        bodyLines = new List<TooltipLineData>(8);
    }

    public void Clear()
    {
        tooltipWidthType = TooltipWidthType.Auto;
        textType = TooltipTextAlignType.Left;
        positionType = TooltipPositionType.Right;
        header = body = bottom = string.Empty;

        //구조체라 초기화해도 문제 없음
        headerLines.Clear();
        bodyLines.Clear();
    }
}