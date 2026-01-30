using System;
using System.Collections.Generic;
using UnityEngine;


public sealed class InteractionController : MonoBehaviour
{
    public event Action<BaseEntity> OnTargetChanged;
    public event Action<bool> OnActionMenuChanged;
    public event Action<BaseEntity> OnTargetSelect;
    public event Action OnCancle;

    private BaseEntity currentSelectEntity;

    public BaseEntity CurrentSelectEntity
    {
        get => currentSelectEntity ?? Managers.Object.Player;
        set
        {
            currentSelectEntity = value;
            OnTargetChanged?.Invoke(currentSelectEntity);
        }
    }
    public InteractionMode Mode { get; set; } = InteractionMode.Target;

    private void Update()
    {
        if (Managers.UI.IsOpenUi)
            return;

        switch (Mode)
        {
            case InteractionMode.Target:
                TargetSelectKeyboard();
                TargetSelectMouse();
                break;

            case InteractionMode.Menu:
                MenuSelectKeyboard();
                break;
        }
    }

    #region 키보드 - TargetSelectKeyboard
    private void TargetSelectKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            TryMove(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            TryMove(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            TryMove(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            TryMove(Vector2.right);

        if (Input.GetKeyDown(KeyCode.Return))
            OpenMenu(CurrentSelectEntity);
    }
    private void TryMove(Vector2 dir)
    {
        if (CurrentSelectEntity == null)
            return;

        List<BaseEntity> list = Managers.Object.ActiveEntities;
        if (list == null || list.Count == 0)
            return;

        BaseEntity next = FindEntityWithDirection(dir, list);
        if (next == null)
            next = FindOppositeEntityWithDirection(dir, list);

        CurrentSelectEntity = next;

        return;
    }
    private BaseEntity FindEntityWithDirection(Vector2 dir, List<BaseEntity> list)
    {
        Vector2 origin = CurrentSelectEntity.Position;

        BaseEntity best = null;
        float bestForward = float.PositiveInfinity;
        float bestDist2 = float.PositiveInfinity;
        float bestTie = float.PositiveInfinity;

        const float EPS = 1e-5f;

        for (int i = 0; i < list.Count; i++)
        {
            BaseEntity entity = list[i];
            if (entity == CurrentSelectEntity)
                continue;

            Vector2 p = entity.Position;
            Vector2 d = p - origin;

            // 카디널 dir 전제: forward = 축 성분만 보면 됨 (Dot보다 직관적)
            float forward = (dir.x != 0f) ? d.x * dir.x : d.y * dir.y;
            if (forward <= 0f)
                continue;

            float dist2 = d.sqrMagnitude;

            // 타이브레이커: 좌/우면 위->아래(y 큰 것 우선) => -y, 상/하면 좌->우(x 작은 것 우선) => x
            float tie = (dir.x != 0f) ? -p.y : p.x;

            bool better =
                (forward + EPS < bestForward) ||
                (Mathf.Abs(forward - bestForward) <= EPS && dist2 + EPS < bestDist2) ||
                (Mathf.Abs(forward - bestForward) <= EPS && Mathf.Abs(dist2 - bestDist2) <= EPS && tie + EPS < bestTie);

            if (better)
            {
                best = entity;
                bestForward = forward;
                bestDist2 = dist2;
                bestTie = tie;
            }
        }

        return best;
    }
    private BaseEntity FindOppositeEntityWithDirection(Vector2 dir, IReadOnlyList<BaseEntity> list)
    {
        BaseEntity oppositeEntity = null;
        bool hasBest = false;

        float bestValue = 0f;
        bool wrapToMin = (dir == Vector2.right || dir == Vector2.up); // right->minX, up->minY
        bool useX = (dir == Vector2.right || dir == Vector2.left);

        for (int i = 0; i < list.Count; i++)
        {
            BaseEntity e = list[i];

            if (e == null || e == currentSelectEntity)
                continue;

            Vector2 p = e.Position;
            float v = useX ? p.x : p.y;

            if (!hasBest)
            {
                oppositeEntity = e;
                bestValue = v;
                hasBest = true;
                continue;
            }

            if (wrapToMin)
            {
                if (v < bestValue)
                {
                    oppositeEntity = e;
                    bestValue = v;
                }
            }
            else
            {
                if (v > bestValue)
                {
                    oppositeEntity = e;
                    bestValue = v;
                }
            }
        }

        return oppositeEntity;
    }
    #endregion

    #region 마우스 - TargetSelectMouse
    private void TargetSelectMouse()
    {
        // 좌클릭
        if (Input.GetMouseButtonDown(0))
        {
            BaseEntity hit = FindEntityUnderMouse();
            if (hit != null)
                CurrentSelectEntity = hit;
        }

        //우클릭
        if (Input.GetMouseButtonDown(1))
        {
            BaseEntity hit = FindEntityUnderMouse();
            if (hit == CurrentSelectEntity)
                OpenMenu(hit);
        }

    }
    private BaseEntity FindEntityUnderMouse()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && hit.collider.TryGetComponent(out BaseEntity target))
            return target;

        return null;
    }
    #endregion

    #region 키보드 - MenuSelectKeyboard
    private void MenuSelectKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            OnActionMenuChanged?.Invoke(true);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            OnActionMenuChanged?.Invoke(false);
        else if (Input.GetKeyDown(KeyCode.Escape))
            CloseMenu();
    }
    #endregion

    #region 메뉴
    private void OpenMenu(BaseEntity target)
    {
        if (target == null)
            return;

        OnTargetSelect?.Invoke(target);
        Mode = InteractionMode.Menu;
    }

    private void CloseMenu()
    {
        OnCancle?.Invoke();
        Mode = InteractionMode.Target;
    }
    #endregion
}