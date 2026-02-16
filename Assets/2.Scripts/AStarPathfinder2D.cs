using System.Collections.Generic;
using UnityEngine;

public static class AStarPathfinder2D
{
    private static readonly Vector2Int[] Dir4 =
    {
        new Vector2Int(1,0), new Vector2Int(-1,0),
        new Vector2Int(0,1), new Vector2Int(0,-1),
    };

    public static bool TryFindPath(GridMap2D grid, Vector2Int start, Vector2Int goal, out List<Vector2Int> path)
    {
        path = null;

        if (!grid.InBounds(start) || !grid.InBounds(goal)) return false;
        if (!grid.IsWalkable(start) || !grid.IsWalkable(goal)) return false;
        if (start == goal) { path = new List<Vector2Int>(); return true; }

        var open = new List<Vector2Int>();
        var openSet = new HashSet<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var g = new Dictionary<Vector2Int, int>();

        open.Add(start);
        openSet.Add(start);
        g[start] = 0;

        while (open.Count > 0)
        {
            // 가장 f = g + h 작은 노드 선택 (소규모 그리드면 선형 스캔으로 충분)
            int bestIndex = 0;
            Vector2Int current = open[0];
            int bestF = g[current] + Heuristic(current, goal);

            for (int i = 1; i < open.Count; i++)
            {
                var c = open[i];
                int f = g[c] + Heuristic(c, goal);
                if (f < bestF)
                {
                    bestF = f;
                    bestIndex = i;
                    current = c;
                }
            }

            open.RemoveAt(bestIndex);
            openSet.Remove(current);

            if (current == goal)
            {
                path = Reconstruct(cameFrom, current);
                // path는 start 제외, goal 포함 형태로 반환
                return true;
            }

            int currentG = g[current];

            for (int i = 0; i < Dir4.Length; i++)
            {
                var next = current + Dir4[i];
                if (!grid.IsWalkable(next)) continue;

                int tentativeG = currentG + 1;

                if (!g.TryGetValue(next, out int oldG) || tentativeG < oldG)
                {
                    cameFrom[next] = current;
                    g[next] = tentativeG;

                    if (!openSet.Contains(next))
                    {
                        open.Add(next);
                        openSet.Add(next);
                    }
                }
            }
        }

        return false;
    }

    private static int Heuristic(Vector2Int a, Vector2Int b) =>
        Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

    private static List<Vector2Int> Reconstruct(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        var rev = new List<Vector2Int> { current };
        while (cameFrom.TryGetValue(current, out var prev))
        {
            current = prev;
            rev.Add(current);
        }
        rev.Reverse();         // start ... goal
        rev.RemoveAt(0);       // start 제거
        return rev;            // goal 포함
    }
}