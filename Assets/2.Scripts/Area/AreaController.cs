using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    [SerializeField] private List<AreaSo> areaSoList;
    [SerializeField] private Area areaPrefab;

    private readonly Dictionary<string, Area> areaMap = new();
    private Area currentArea;

    public IEnumerator CreateAreas(ILoadProgress progress)
    {
        areaMap.Clear();

        int areaCount = areaSoList.Count;
        for (int i = 0; i < areaCount; i++)
        {
            AreaSo areaData = areaSoList[i];

            progress.UpdateMessage($"Loading Area {i + 1}/{areaCount}");

            Area area = Instantiate(areaPrefab, transform);
            area.Init(areaData);
            area.name = $"Area_{areaData.AreaId}";
            area.gameObject.SetActive(false);

            yield return area.CreateDefaultObjects();

            areaMap[areaData.AreaId] = area;

            progress.UpdateProgress((i + 1) / (float)areaCount);
        }

        progress?.UpdateProgress(1f);

        yield break;
    }

    public void EnterArea(string areaId)
    {
        if (currentArea != null)
            currentArea.gameObject.SetActive(false);

        if (!areaMap.TryGetValue(areaId, out Area nextArea))
        {
            Debug.LogError($"[Area] Not found: {areaId}");
            return;
        }

        currentArea = nextArea;
        currentArea.gameObject.SetActive(true);
    }

    public void ReleaseAreas()
    {
        for (int i = 0; i < areaMap.Count; i++)
        {
        }
    }
}