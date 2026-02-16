using UnityEngine;

public class GridMap2D : MonoBehaviour
{
    [Header("Grid")]
    [Min(1)] public int width = 20;
    [Min(1)] public int height = 12;
    [Min(0.1f)] public float cellSize = 1f;

    [Tooltip("그리드 좌하단(0,0) 월드 기준점")]
    public Transform origin; // 비워두면 this.transform

    [Header("Walkable Check")]
    public LayerMask obstacleMask;
    [Tooltip("셀 중앙에서 이 크기로 OverlapBox 검사")]
    public Vector2 obstacleCheckSize = new Vector2(0.9f, 0.9f);

    private bool[,] _walkable;

    public Vector3 OriginWorld => (origin ? origin.position : transform.position);

    public void Build()
    {
        _walkable = new bool[width, height];

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                Vector3 center = CellCenterWorld(new Vector2Int(x, y));
                bool blocked = Physics2D.OverlapBox(center, obstacleCheckSize, 0f, obstacleMask) != null;
                _walkable[x, y] = !blocked;
            }
    }

    public bool InBounds(Vector2Int c) =>
        c.x >= 0 && c.x < width && c.y >= 0 && c.y < height;

    public bool IsWalkable(Vector2Int c) =>
        InBounds(c) && _walkable != null && _walkable[c.x, c.y];

    public Vector2Int WorldToCell(Vector3 world)
    {
        Vector3 o = OriginWorld;
        int x = Mathf.FloorToInt((world.x - o.x) / cellSize);
        int y = Mathf.FloorToInt((world.y - o.y) / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 CellCenterWorld(Vector2Int cell)
    {
        Vector3 o = OriginWorld;
        return new Vector3(
            o.x + (cell.x + 0.5f) * cellSize,
            o.y + (cell.y + 0.5f) * cellSize,
            0f
        );
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 o = OriginWorld;
        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.color = new Color(1, 1, 1, 0.15f);
        for (int x = 0; x <= width; x++)
        {
            Vector3 a = new Vector3(o.x + x * cellSize, o.y, 0);
            Vector3 b = new Vector3(o.x + x * cellSize, o.y + height * cellSize, 0);
            Gizmos.DrawLine(a, b);
        }
        for (int y = 0; y <= height; y++)
        {
            Vector3 a = new Vector3(o.x, o.y + y * cellSize, 0);
            Vector3 b = new Vector3(o.x + width * cellSize, o.y + y * cellSize, 0);
            Gizmos.DrawLine(a, b);
        }
    }
#endif
}