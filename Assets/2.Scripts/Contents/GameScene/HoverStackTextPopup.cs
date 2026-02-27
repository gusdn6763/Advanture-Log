using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverStackTextPopup : MonoBehaviour
{
    [SerializeField] private RectTransform root;
    [SerializeField] private Transform rowRoot;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private int initialPoolSize = 6;

    [Header("Position")]
    [SerializeField] private Vector2 screenOffset = new Vector2(24f, -24f);
    [SerializeField] private Camera uiCamera;

    private readonly List<GameObject> pool = new List<GameObject>(16);
    private int activeCount;

    private void Awake()
    {
        if (root == null)
            root = GetComponent<RectTransform>();

        if (rowRoot == null)
            rowRoot = transform;

        WarmPool(initialPoolSize);
        Hide();
    }

    public void Hide()
    {
        for (int i = 0; i < activeCount; i++)
            pool[i].SetActive(false);

        activeCount = 0;

        if (root != null)
            root.gameObject.SetActive(false);
    }

    public void ShowEntities(IReadOnlyList<BaseEntity> entities, Vector2 screenPos)
    {
        if (entities == null || entities.Count == 0)
        {
            Hide();
            return;
        }

        if (root != null && !root.gameObject.activeSelf)
            root.gameObject.SetActive(true);

        SetScreenPosition(screenPos + screenOffset);

        EnsurePool(entities.Count);

        for (int i = 0; i < entities.Count; i++)
        {
            GameObject row = pool[i];
            if (!row.activeSelf)
                row.SetActive(true);

            TMP_Text text = row.GetComponentInChildren<TMP_Text>(true);
            if (text != null)
            {
                BaseEntity e = entities[i];
                // 표시 이름 소스는 프로젝트에 맞게 교체하세요.
                // (예: e.BaseData.LocalizedName / e.BaseData.DisplayName 등)
                string displayName = e != null ? e.gameObject.name : "-";
                text.text = displayName;
            }
        }

        // 남는 줄 끄기
        for (int i = entities.Count; i < activeCount; i++)
            pool[i].SetActive(false);

        activeCount = entities.Count;
    }

    private void SetScreenPosition(Vector2 screenPos)
    {
        if (root == null)
            return;

        Vector3 worldPos;
        bool ok = RectTransformUtility.ScreenPointToWorldPointInRectangle(root, screenPos, uiCamera, out worldPos);
        if (ok)
            root.position = worldPos;
        else
            root.position = screenPos;
    }

    private void WarmPool(int count)
    {
        EnsurePool(count);
    }

    private void EnsurePool(int needed)
    {
        if (rowPrefab == null)
            return;

        while (pool.Count < needed)
        {
            GameObject row = Instantiate(rowPrefab, rowRoot);
            row.SetActive(false);
            pool.Add(row);
        }
    }
}