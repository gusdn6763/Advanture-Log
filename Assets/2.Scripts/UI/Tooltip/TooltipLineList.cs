using System.Collections.Generic;
using UnityEngine;

public class TooltipLineList : MonoBehaviour
{
    [SerializeField] private TooltipLine linePrefab;
    [SerializeField] private int poolCount;

    private List<TooltipLine> poolLine = new List<TooltipLine>();
    public void Init()
    {
        for (int i = 0; i < poolCount; i++)
            CreateLine();
    }

    private void CreateLine()
    {
        TooltipLine line = Instantiate(linePrefab, transform);

        poolLine.Add(line);
        line.gameObject.SetActive(false);
    }

    public void SetLines(List<TooltipLineData> lines)
    {
        int count = lines.Count;

        // pool »Æ¿Â
        while (poolLine.Count < count)
            CreateLine();

        // ¿˚øÎ
        for (int i = 0; i < count; i++)
        {
            TooltipLine line = poolLine[i];

            line.SetText(lines[i].Left, lines[i].Right);
            line.gameObject.SetActive(true);
        }

        // ≥≤¥¬ row º˚±Ë
        for (int i = count; i < poolLine.Count; i++)
            poolLine[i].gameObject.SetActive(false);
    }

    public void Clear()
    {
        for (int i = 0; i < poolLine.Count; i++)
        {
            TooltipLine line = poolLine[i];

            line.SetText(string.Empty, string.Empty);
            line.gameObject.SetActive(false);
        }
    }
}