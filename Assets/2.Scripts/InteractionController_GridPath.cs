using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController_GridPath : MonoBehaviour
{
    public Camera cam;
    public GridMap2D grid;
    public PlayerGridMover playerMover;
    public PathParticleRenderer pathVfx;

    [Header("Input")]
    public int mouseButtonMove = 0; // 좌클릭
    public LayerMask groundMask;    // 바닥(레이캐스트용). 없으면 ScreenToWorldPoint로 처리

    private Vector2Int _lastHoverCell = new Vector2Int(int.MinValue, int.MinValue);
    private List<Vector2Int> _lastPathCells;

    private void Start()
    {
        if (grid != null) grid.Build();
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (cam == null || grid == null || playerMover == null || pathVfx == null) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (!TryGetMouseWorld(out Vector3 mouseWorld))
        {
            pathVfx.Clear();
            _lastPathCells = null;
            return;
        }

        Vector2Int hoverCell = grid.WorldToCell(mouseWorld);
        if (!grid.InBounds(hoverCell) || !grid.IsWalkable(hoverCell))
        {
            pathVfx.Clear();
            _lastPathCells = null;
            return;
        }

        // 셀이 바뀔 때만 경로 재계산(매 프레임 A* 방지)
        if (hoverCell != _lastHoverCell)
        {
            _lastHoverCell = hoverCell;

            Vector2Int start = playerMover.CurrentCell;
            if (AStarPathfinder2D.TryFindPath(grid, start, hoverCell, out var pathCells))
            {
                _lastPathCells = pathCells;

                // 파티클은 월드 포인트로 변환해서 표시
                var points = new List<Vector3>(pathCells.Count);
                for (int i = 0; i < pathCells.Count; i++)
                    points.Add(grid.CellCenterWorld(pathCells[i]));

                pathVfx.RenderPath(points);
            }
            else
            {
                pathVfx.Clear();
                _lastPathCells = null;
            }
        }

        if (Input.GetMouseButtonDown(mouseButtonMove))
        {
            if (_lastPathCells != null && _lastPathCells.Count > 0)
                playerMover.MoveAlong(_lastPathCells);
        }
    }

    private bool TryGetMouseWorld(out Vector3 world)
    {
        // 1) 바닥 콜라이더가 있으면 Raycast로 정확히 찍기
        if (groundMask.value != 0)
        {
            Vector3 mp = Input.mousePosition;
            Vector3 wp = cam.ScreenToWorldPoint(mp);
            var hit = Physics2D.Raycast(wp, Vector2.zero, 0f, groundMask);
            if (hit.collider != null)
            {
                world = hit.point;
                return true;
            }
        }

        // 2) 없으면 ScreenToWorldPoint(2D)
        Vector3 p = Input.mousePosition;
        p.z = -cam.transform.position.z;
        world = cam.ScreenToWorldPoint(p);
        return true;
    }
}