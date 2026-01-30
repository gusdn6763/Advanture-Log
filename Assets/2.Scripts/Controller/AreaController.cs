using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AreaController : MonoBehaviour
{
    public event Action<Area> OnAreaChanged;

    [SerializeField] private AreaGraphSo areaGraph;
    [SerializeField] private AssetReferenceGameObject areaPrefabRef;

    private Dictionary<string, Area> areaMap = new Dictionary<string, Area>();
    private Area currentArea;

    public IEnumerator CreateAreas(ILoadProgress progress)
    {
        ReleaseAllAreas();

        List<AreaSo> areaSos = areaGraph.GetAllAreaSo();
        int areaCount = areaSos.Count;
        for (int i = 0; i < areaCount; i++)
        {
            AreaSo areaData = areaSos[i];

            progress.UpdateMessage($"Loading Area {i + 1}/{areaCount}");

            AsyncOperationHandle<GameObject> handle = areaPrefabRef.InstantiateAsync(transform);
            yield return handle;

            Area area = handle.Result.GetOrAddComponent<Area>();
            area.Init(areaData);
            area.name = $"Area_{areaData.AreaId}";
            area.gameObject.SetActive(false);
            yield return area.CreateDefaultObjects();

            areaMap[areaData.AreaId] = area;

            progress.UpdateProgress((i + 1) / (float)areaCount);
        }

        progress?.UpdateProgress(1f);
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

        OnAreaChanged?.Invoke(currentArea);
    }

    private void OnDestroy()
    {
        ReleaseAllAreas();
    }

    private void ReleaseAllAreas()
    {
        foreach (KeyValuePair<string, Area> kv in areaMap)
            Addressables.ReleaseInstance(kv.Value.gameObject);

        areaMap.Clear();
        currentArea = null;
        OnAreaChanged?.Invoke(null);
    }
}