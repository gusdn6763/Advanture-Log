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
    private SpriteRenderer gridImage;

    public Area CurrentArea { get; private set; } = null;
    public bool IsMoving { get; private set; } = false;

    public void Init()
    {
        foreach (KeyValuePair<string, AreaSo> area in areas)
            area.Value.SetId(area.Key);

        gridImage = GetComponent<SpriteRenderer>();
        gridImage.drawMode = SpriteDrawMode.Tiled;
        gridImage.size = Vector2.zero;
    }

    public IEnumerator CreateAreas(AreaGraphSo areaGraph, Action<string, int, int> callback)
    {
        List<AreaSo> areaSos = areaGraph.GetAllAreaSo();
        int areaCount = areaSos.Count;
        for (int i = 0; i < areaCount; i++)
        {
            AreaSo areaData = areaSos[i];

            callback?.Invoke("Loading Area", i + 1, areaCount);

            Area area = Instantiate(areaData.AreaPrefab, transform);
            area.Init(areaData);
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

        //특정 효과
        

        if (CurrentArea != null)
            CurrentArea.ExitArea();

        if (!areaMap.TryGetValue(areaId, out Area nextArea))
        {
            IsMoving = false;
            yield break;
        }

        CurrentArea = nextArea;
        CurrentArea.gameObject.SetActive(true);

        OnAreaChanged?.Invoke(nextArea);

        IsMoving = false;
    }
}

