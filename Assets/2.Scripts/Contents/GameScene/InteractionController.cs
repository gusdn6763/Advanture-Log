using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private GameObject selectBar;

    [Header("Path Preview")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PathPreviewRenderer pathPreview;

    [Header("Hover Stack Popup")]
    [SerializeField] private HoverStackTextPopup hoverStackPopup;

    private readonly AStarGridPathfinder pathfinder = new AStarGridPathfinder();
    private readonly List<Vector2Int> pathBuffer = new List<Vector2Int>(128);

    private BaseEntity currentSelectEntity;

    private Vector2Int lastHoverCell = new Vector2Int(int.MinValue, int.MinValue);
    private Vector2Int lastPlayerCell = new Vector2Int(int.MinValue, int.MinValue);

    private Player player;
    public void Init(Player player)
    {
        this.player = player;
    }

    private void Update()
    {
        if (player == null)
            return;

        UpdateSelectBar();
        HandleInput();
    }

    private void OnEnable()
    {
        Managers.Area.OnAreaChanged += SelectBarReset;
    }

    private void OnDisable()
    {
        Managers.Area.OnAreaChanged -= SelectBarReset;
    }

    private void UpdateSelectBar()
    {
        Area currentArea = Managers.Area.CurrentArea;
        if (currentArea == null)
            return;

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int hoverCell;
        if (!currentArea.TryWorldToNearestCell(worldPoint, out hoverCell))
        {
            selectBar.gameObject.SetActive(false);

            if (pathPreview != null)
                pathPreview.Clear();

            if (hoverStackPopup != null)
                hoverStackPopup.Hide();

            return;
        }

        // 플레이어 셀 계산
        Vector2Int playerCell = lastPlayerCell;
        bool hasPlayerCell = TryGetPlayerCell(currentArea, out playerCell);

        bool hoverChanged = hoverCell != lastHoverCell;
        bool playerChanged = hasPlayerCell && playerCell != lastPlayerCell;

        if (!hoverChanged && !playerChanged)
            return;

        lastHoverCell = hoverCell;
        if (hasPlayerCell)
            lastPlayerCell = playerCell;

        // ---- (1) 해당 위치에 최단 경로 표시 ----
        UpdatePathPreview(currentArea, hasPlayerCell, playerCell, hoverCell);

        // selectBar는 이동 불가 장애물이어도 표시
        selectBar.gameObject.SetActive(true);

        // ---- (2) 순간적으로 오브젝트들 보여주기 ----
        if (hoverChanged)
        {
            IReadOnlyList<BaseEntity> list;
            currentArea.TryGetEntitiesAtCell(hoverCell, out list);

            if (hoverStackPopup != null)
            {
                if (list == null || list.Count == 0)
                    hoverStackPopup.Hide();
                else
                    hoverStackPopup.ShowEntities(list, Input.mousePosition);
            }
        }
    }

    private bool TryGetPlayerCell(Area area, out Vector2Int cell)
    {
        cell = default;

        if (playerTransform == null)
            return false;

        return area.TryWorldToNearestCell(playerTransform.position, out cell);
    }

    private void UpdatePathPreview(Area area, bool hasPlayerCell, Vector2Int playerCell, Vector2Int hoverCell)
    {
        if (pathPreview == null)
            return;

        if (!hasPlayerCell)
        {
            pathPreview.Clear();
            return;
        }

        // 목표 셀이 막혀있으면(바위/고블린) 이동 불가 -> 경로 숨김
        if (!IsWalkableCell(area, hoverCell))
        {
            pathPreview.Clear();
            return;
        }

        pathfinder.Resize(area.Size);

        bool found = pathfinder.TryFindPath(playerCell, hoverCell, (Vector2Int c) => IsWalkableCell(area, c), pathBuffer);
        if (!found)
        {
            pathPreview.Clear();
            return;
        }

        pathPreview.SetPath(area, pathBuffer);
    }

    private bool IsWalkableCell(Area area, Vector2Int cell)
    {
        IReadOnlyList<BaseEntity> list;
        bool ok = area.TryGetEntitiesAtCell(cell, out list);
        if (!ok)
            return false;

        if (list == null || list.Count == 0)
            return true;

        for (int i = 0; i < list.Count; i++)
        {
            BaseEntity e = list[i];
            if (e == null)
                continue;

            if (e.Block)
                return false;
        }

        return true;
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
            currentSelectEntity = FindEntityUnderMouse();
        else if (Input.GetMouseButtonDown(1))
        {
            BaseEntity hit = FindEntityUnderMouse();
            if (hit != null && hit == currentSelectEntity)
                Managers.UI.OpenUi(UiType.ActionMenu, hit);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Managers.UI.ClosePopupUI();
    }

    private BaseEntity FindEntityUnderMouse()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && hit.collider.TryGetComponent(out BaseEntity target))
            return target;

        return null;
    }

    public void SelectBarReset(Area area)
    {
        currentSelectEntity = null;
        selectBar.SetActive(false);

        lastHoverCell = new Vector2Int(int.MinValue, int.MinValue);
        lastPlayerCell = new Vector2Int(int.MinValue, int.MinValue);

        if (pathPreview != null)
            pathPreview.Clear();

        if (hoverStackPopup != null)
            hoverStackPopup.Hide();
    }
}