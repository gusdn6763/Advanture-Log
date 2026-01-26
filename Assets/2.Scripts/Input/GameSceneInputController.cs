using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

public sealed class GameSceneInputController : MonoBehaviour
{
    private BaseEntity currentSelectEntity;

    public GameObject selectBar;
    private InputMode Mode { get; set; }
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Mode = InputMode.Navigate;

        currentSelectEntity = Managers.Object.Player;

        selectBar.SetActive(true);
        selectBar.transform.position = currentSelectEntity.Position;
    }

    private void Update()
    {
        switch (Mode)
        {
            case InputMode.Navigate:
                UpdateNavigate();
                break;

            case InputMode.Menu:
                UpdateMenu();
                break;
        }
    }

    private void UpdateNavigate()
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
            Mode = InputMode.Menu;
    }

    private void UpdateMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Mode = InputMode.Navigate;
    }

    private void TryMove(Vector2 dir)
    {
        if (currentSelectEntity == null) 
            return;

        List<BaseEntity> list = Managers.Object.ActiveEntities;
        if (list == null || list.Count == 0)
            return;

        BaseEntity next = FindEntityWithDirection(dir, list);
        if (next == null)
            next = FindOppositeEntityWithDirection(dir, list);

        if (next == null)
            currentSelectEntity = Managers.Object.Player;
        else
            currentSelectEntity = next;

        selectBar.transform.position = currentSelectEntity.Position;

        //area.SetCurrentEntity(next);
        return;
    }

    private BaseEntity FindEntityWithDirection(Vector2 dir, List<BaseEntity> list)
    {
        Vector2 origin = currentSelectEntity.Position;

        BaseEntity best = null;
        float bestForward = float.PositiveInfinity;
        float bestDist2 = float.PositiveInfinity;
        float bestTie = float.PositiveInfinity;

        const float EPS = 1e-5f;

        for (int i = 0; i < list.Count; i++)
        {
            BaseEntity entity = list[i];
            if (entity == currentSelectEntity)
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
}