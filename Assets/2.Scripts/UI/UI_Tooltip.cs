using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour
{
    [SerializeField] private RectTransform root;     // TooltipImage
    [SerializeField] private TextMeshProUGUI text;   // child TMP

    [Header("Layout")]
    [SerializeField] private Vector2 padding = new Vector2(50f, 50f);
    [SerializeField] private float maxWidth = 360f;
    [SerializeField] private Vector2 screenMargin = new Vector2(8f, 8f);

    [Header("Mouse Offset (down-right default)")]
    [SerializeField] private Vector2 mouseOffset = new Vector2(12f, -12f);

    private Canvas canvas;
    private RectTransform canvasRect;
    private Camera uiCam;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (!root) root = (RectTransform)transform;
        if (!text) text = GetComponentInChildren<TextMeshProUGUI>(true);

        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        uiCam = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera;

        // (1) 툴팁이 포인터를 가로채서 깜빡이는 문제 방지
        // - 그래픽 레이캐스트 타겟 끄기
        var img = root.GetComponent<Image>();
        if (img) img.raycastTarget = false;
        text.raycastTarget = false;

        // - 추가로 안전하게 blocksRaycasts도 차단
        canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.ignoreParentGroups = true;

        // (2) 중앙 기준 방지: pivot을 좌상단(0,1)로 고정
        root.pivot = new Vector2(0f, 1f);

        Hide();
    }

    public void Show(string content)
    {
        SetText(content);
        gameObject.SetActive(true);
        FollowMouse(); // 최초 1회 위치
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    //private void Update()
    //{
    //    if (!gameObject.activeSelf) return;
    //    FollowMouse();
    //}

    private void FollowMouse()
    {
        // 화면좌표 -> 캔버스 로컬좌표 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, Input.mousePosition, uiCam, out Vector2 cursorLocal);

        PlaceNearCursor(cursorLocal);
    }

    public void SetText(string content)
    {
        text.text = content;
        text.ForceMeshUpdate();

        // (3) 텍스트 크기 기반으로 배경 이미지 크기 설정
        // 폭 제한을 걸어서 높이도 올바르게 계산
        float w = Mathf.Min(text.preferredWidth, maxWidth);
        Vector2 preferred = text.GetPreferredValues(content, w, 0f);

        // 텍스트 영역 크기(선택: 유지해도 되고 빼도 됨)
        text.rectTransform.sizeDelta = preferred;

        // 배경 크기 = 텍스트 + 패딩
        root.sizeDelta = preferred + new Vector2(padding.x * 2f, padding.y * 2f);

        LayoutRebuilder.ForceRebuildLayoutImmediate(root);
    }

    private void PlaceNearCursor(Vector2 cursorLocal)
    {
        Rect c = canvasRect.rect;
        Vector2 size = root.sizeDelta;

        float minX = c.xMin + screenMargin.x;
        float maxX = c.xMax - screenMargin.x;
        float minY = c.yMin + screenMargin.y;
        float maxY = c.yMax - screenMargin.y;

        // 기본: 커서의 오른쪽 아래
        float offX = mouseOffset.x;          // + (right)
        float offY = mouseOffset.y;          // - (down)

        // 먼저 “오른쪽 아래”로 놓았을 때의 top-left 좌표 (pivot = (0,1))
        Vector2 pos = cursorLocal + new Vector2(offX, offY);

        // (4) 화면 밖으로 나가면 방향을 뒤집어서 배치 (밀어넣기 X)
        // 오른쪽 초과면 -> 왼쪽에 표시: top-left.x = cursor.x - offX - size.x
        if (pos.x + size.x > maxX)
            pos.x = cursorLocal.x - offX - size.x;

        // 왼쪽 초과면 -> 오른쪽에 강제
        if (pos.x < minX)
            pos.x = cursorLocal.x + offX;

        // 아래로 초과(바닥 아래)면 -> 위에 표시: top-left.y = cursor.y - offY + size.y
        // (offY는 음수이므로 -offY는 양수)
        if (pos.y - size.y < minY)
            pos.y = cursorLocal.y - offY + size.y;

        // 위로 초과면 -> 아래에 강제
        if (pos.y > maxY)
            pos.y = cursorLocal.y + offY;

        // 최종 안전 클램프(마지막 보험)
        pos.x = Mathf.Clamp(pos.x, minX, maxX - size.x);
        pos.y = Mathf.Clamp(pos.y, minY + size.y, maxY);

        root.anchoredPosition = pos;
    }
}