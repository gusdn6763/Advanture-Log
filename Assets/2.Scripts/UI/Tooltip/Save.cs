using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Save : MonoBehaviour
{
    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI headerLeftText;
    [SerializeField] private TextMeshProUGUI headerRightText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private TextMeshProUGUI bottomText;

    [Header("Setting")]
    [SerializeField] private float minWidth = 120f;
    [SerializeField] private float maxWidth = 500f;

    [Header("스크린 여백 값")]
    [SerializeField] private Vector2 screenMargin = new Vector2(8f, 8f);

    [Header("타겟으로부터 띄울 거리")]
    [SerializeField] private Vector2 offset = new Vector2(12f, 12f);

    private Canvas canvas;
    private RectTransform canvasRect;

    public void Init()
    {
        canvas = GetComponent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();

        gameObject.SetActive(false);
    }

    public void ShowTooltip(TooltipData tooltipData, RectTransform target)
    {
        gameObject.SetActive(true);

        // 1) 텍스트 세팅
        headerText.text = tooltipData.header;
        bodyText.text = tooltipData.body;
        bottomText.text = tooltipData.bottom;

        SettingTooltipWidth();

        //화면 좌표를 Canvas의 내부 로컬 좌표로 변경
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, target.transform.position, null, out Vector2 localPoint);

        PlaceTooltip(localPoint, target, tooltipData.positionType);
    }

    private void SettingTooltipWidth()
    {
        float width = minWidth;

        width = Mathf.Max(width, headerText.preferredWidth);
        width = Mathf.Max(width, headerLeftText.preferredWidth + headerRightText.preferredWidth);
        width = Mathf.Max(width, bodyText.preferredWidth);
        width = Mathf.Max(width, bottomText.preferredWidth);

        width = Mathf.Clamp(width, minWidth, maxWidth);

        tooltipRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    private void PlaceTooltip(Vector2 cursorLocal, RectTransform target, TooltipPositionType type)
    {
        // 1) 레이아웃 갱신 → 실제 툴팁 크기 확보
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);

        // 2) 실제 크기(피벗 0.5,0.5 가정)
        Vector2 tooltipHalf = tooltipRect.rect.size * 0.5f;
        Vector2 targetHalf = target.rect.size * 0.5f;

        Vector2 pos = cursorLocal;

        // 3) 타겟 반크기 + 툴팁 반크기 + offset 만큼 이동
        switch (type)
        {
            case TooltipPositionType.Left:
                pos += new Vector2(-(targetHalf.x + tooltipHalf.x + offset.x), 0f);
                break;

            case TooltipPositionType.Right:
                pos += new Vector2((targetHalf.x + tooltipHalf.x + offset.x), 0f);
                break;

            case TooltipPositionType.Up:
                pos += new Vector2(0f, (targetHalf.y + tooltipHalf.y + offset.y));
                break;

            case TooltipPositionType.Down:
                pos += new Vector2(0f, -(targetHalf.y + tooltipHalf.y + offset.y));
                break;
        }

        // 4) 클램프
        Vector2 canvasHalf = canvasRect.rect.size * 0.5f;

        float minX = -canvasHalf.x + tooltipHalf.x + screenMargin.x;
        float maxX = canvasHalf.x - tooltipHalf.x - screenMargin.x;
        float minY = -canvasHalf.y + tooltipHalf.y + screenMargin.y;
        float maxY = canvasHalf.y - tooltipHalf.y - screenMargin.y;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        tooltipRect.anchoredPosition = pos;
    }

    public void Hide()
    {
        headerText.text = string.Empty;
        headerLeftText.text = string.Empty;
        headerRightText.text = string.Empty;
        bodyText.text = string.Empty;
        bottomText.text = string.Empty;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);

        gameObject.SetActive(false);
    }
}