using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 간선(Edge)
/// </summary>
[Serializable]
public class AreaNeighbor
{
    public AreaSo to;
    public int cost = 1;
}

/// <summary>
/// 정점(Vertex)
/// </summary>
[Serializable]
public class AreaVertex
{
    public AreaSo area;
    public List<AreaNeighbor> neighbors = new List<AreaNeighbor>();
}

[CreateAssetMenu(menuName = "Game/Area/AreaGraph", fileName = "AreaGraph")]
public class AreaGraphSo : ScriptableObject
{
    [SerializeField] private List<AreaVertex> vertices = new List<AreaVertex>();

    private Dictionary<AreaSo, List<AreaNeighbor>> areaGraph = new Dictionary<AreaSo, List<AreaNeighbor>>();  //vertices를 쉽게 풀어쓴 변수

    private void OnEnable()
    {
        CreateAreaGraph();
    }

    private void CreateAreaGraph()
    {
        areaGraph.Clear();

        //정점 등록, new List<AreaNeighbor>()를 넣는 이유는 원본 데이터와 분리하기 위함
        int vCount = vertices.Count;
        for (int i = 0; i < vCount; i++)
        {
            AreaSo area = vertices[i].area;
            if (!areaGraph.ContainsKey(area))
                areaGraph.Add(area, new List<AreaNeighbor>());
        }

        //간선 등록
        for (int i = 0; i < vCount; i++)
        {
            AreaVertex vertex = vertices[i];

            int nCount = vertex.neighbors.Count;
            for (int j = 0; j < nCount; j++)
            {
                AreaNeighbor n = vertex.neighbors[j];

                areaGraph[vertex.area].Add(new AreaNeighbor { to = n.to, cost = n.cost });
            }
        }
    }

    /// <summary>
    /// BFS
    /// </summary>
    /// <returns></returns>
    public List<AreaSo> GetAllAreaSo()
    {
        List<AreaSo> result = new List<AreaSo>();

        HashSet<AreaSo> visited = new HashSet<AreaSo>();
        Queue<AreaSo> q = new Queue<AreaSo>();

        visited.Add(vertices[0].area);
        q.Enqueue(vertices[0].area);

        while (q.Count > 0)
        {
            AreaSo cur = q.Dequeue();
            result.Add(cur);

            List<AreaNeighbor> neighbors = areaGraph[cur];
            for (int i = 0; i < neighbors.Count; i++)
            {
                AreaSo next = neighbors[i].to;
                if (visited.Add(next))
                    q.Enqueue(next);
            }
        }

        return result;
    }

    /// 시작 지역에서 "각 지역까지의 최소 비용" 반환 (도달 불가 지역은 제외)
    public Dictionary<AreaSo, int> GetMinCostsFrom(AreaSo start)
    {
        Dictionary<AreaSo, int> dist = new Dictionary<AreaSo, int>();
        foreach (KeyValuePair<AreaSo, List<AreaNeighbor>> kv in areaGraph)
            dist[kv.Key] = int.MaxValue;

        dist[start] = 0;

        HashSet<AreaSo> done = new HashSet<AreaSo>();

        while (true)
        {
            AreaSo current = null;
            int currentDist = int.MaxValue;

            foreach (KeyValuePair<AreaSo, int> kv in dist)
            {
                if (done.Contains(kv.Key))
                    continue;

                if (kv.Value < currentDist)
                {
                    current = kv.Key;
                    currentDist = kv.Value;
                }
            }

            if (current == null || currentDist == int.MaxValue)
                break;

            done.Add(current);

            List<AreaNeighbor> neighbors = areaGraph[current];
            for (int i = 0; i < neighbors.Count; i++)
            {
                AreaNeighbor edge = neighbors[i];

                int alt = currentDist + edge.cost;
                int prev = dist[edge.to];

                if (alt < prev)
                    dist[edge.to] = alt;
            }
        }

        // 도달 가능한 것만 반환
        Dictionary<AreaSo, int> result = new Dictionary<AreaSo, int>();
        foreach (KeyValuePair<AreaSo, int> kv in dist)
        {
            if (kv.Value != int.MaxValue)
                result.Add(kv.Key, kv.Value);
        }

        return result;
    }
}