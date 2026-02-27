using System.Collections.Generic;
using UnityEngine;

public sealed class PathPreviewRenderer : MonoBehaviour
{
    [SerializeField] private Transform markerRoot;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private int initialPoolSize = 32;
    [SerializeField] private bool includeStartCell = false;

    private readonly List<GameObject> pool = new List<GameObject>(64);
    private int activeCount;

    private void Awake()
    {
        if (markerRoot == null)
            markerRoot = transform;

        WarmPool(initialPoolSize);
        Clear();
    }

    public void Clear()
    {
        for (int i = 0; i < activeCount; i++)
            pool[i].SetActive(false);

        activeCount = 0;
    }

    public void SetPath(Area area, IReadOnlyList<Vector2Int> path)
    {
        Clear();

        if (area == null || path == null)
            return;

        if (path.Count <= 1)
            return;

        int startIndex = includeStartCell ? 0 : 1;
        int needed = path.Count - startIndex;
        EnsurePool(needed);

        for (int i = 0; i < needed; i++)
        {
            Vector2Int cell = path[i + startIndex];
            Vector2 world = area.OriginWorld + (Vector2)cell;

            GameObject go = pool[i];
            go.transform.position = world;
            go.SetActive(true);
        }

        activeCount = needed;
    }

    private void WarmPool(int count)
    {
        EnsurePool(count);
    }

    private void EnsurePool(int needed)
    {
        if (markerPrefab == null)
            return;

        while (pool.Count < needed)
        {
            GameObject go = Instantiate(markerPrefab, markerRoot);
            go.SetActive(false);
            pool.Add(go);
        }
    }
}