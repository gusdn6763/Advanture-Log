using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public event Action<Area> OnAreaChanged;

    [SerializeField] private SerializedDictionary<string, AreaSo> areas = new SerializedDictionary<string, AreaSo>();

    private Dictionary<string, Area> areaMap = new Dictionary<string, Area>();
    public Area CurrentArea { get; private set; } = null;
    public bool IsMoving { get; private set; } = false;

    public void Init()
    {
        foreach (KeyValuePair<string, AreaSo> area in areas)
            area.Value.SetId(area.Key);
    }

    public bool TryGetArea(string id, out AreaSo so)
    {
        if (areas.TryGetValue(id, out AreaSo baseSo))
        {
            so = baseSo;
            return true;
        }
        so = null;
        return false;
    }

    public Vector2 Cell2World(Vector2Int cellPos) { return Vector2.zero; }

    public IEnumerator CreateAreas(AreaGraphSo areaGraph, Action<string, int, int> callback)
    {
        List<AreaSo> areaSos = areaGraph.GetAllAreaSo();
        int areaCount = areaSos.Count;
        for (int i = 0; i < areaCount; i++)
        {
            AreaSo areaData = areaSos[i];

            callback?.Invoke("Loading Area", i + 1, areaCount);

            Area area = Instantiate(areaData.AreaPrefab, transform);
            area.SetInfo(areaData);
            area.SpawnFixedEntities();
            area.name = $"Area_{areaData.Id}";
            area.gameObject.SetActive(false);

            areaMap[areaData.Id] = area;

            yield return null;
        }
    }

    public IEnumerator EnterArea(string areaId)
    {
        if (IsMoving)
            yield break;

        IsMoving = true;

        if (CurrentArea != null)
            CurrentArea.ExitArea();

        if (!areaMap.TryGetValue(areaId, out Area nextArea))
        {
            Debug.LogError($"[Area] Not found: {areaId}");
            yield break;
        }

        CurrentArea = nextArea;
        CurrentArea.gameObject.SetActive(true);

        OnAreaChanged?.Invoke(nextArea);

        IsMoving = false;
    }

    private void ReleaseAllAreas()
    {
        areaMap.Clear();
        CurrentArea = null;
    }

    #region Load
    public void LoadAreas(string str)
    {
        //지역 데이터를 받은 후 추가 오브젝트 불러오기.
    }
    #endregion
}

