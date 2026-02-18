using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup), typeof(ContentSizeFitter))]
public class UI_Tooltip : MonoBehaviour
{
    [Header("스크린 여백 값")]
    [SerializeField] private Vector2 screenMargin = new Vector2(8f, 8f);

    [Header("타겟으로부터 띄울 거리")]
    [SerializeField] private Vector2 offset = new Vector2(12f, 12f);

    [Header("Setting")]
    [SerializeField] private float minWidth;
    [SerializeField] private float maxWidth;

    [Header("Texts")]
    [SerializeField] protected TextMeshProUGUI headerText;
    [SerializeField] private TooltipLineList headerLineList;
    [SerializeField] protected Image bodyDivisionImage;
    [SerializeField] protected TextMeshProUGUI bodyText;
    [SerializeField] private TooltipLineList bodyLineList;
    [SerializeField] protected Image bottomDivisionImage;
    [SerializeField] protected TextMeshProUGUI bottomText;

    protected RectTransform canvasRect;
    protected RectTransform tooltipRect;
    protected VerticalLayoutGroup verticalLayoutGroup;
    public virtual void Init(RectTransform rectTransform)
    {
        canvasRect = rectTransform;
        tooltipRect = GetComponent<RectTransform>();
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
    }

    public void Show(TooltipData data, RectTransform target)
    {
        gameObject.SetActive(true);

        SettingText(data);

        Canvas.ForceUpdateCanvases();

        ApplySectionSeparators(data);

        SettingTooltip(data);

        Place(target, data.positionType);
    }

    public void SettingText(TooltipData data)
    {
        headerText.text = data.header;
        bodyText.text = data.body;
        headerLineList.SetLines(data.headerLines);
        bodyLineList.SetLines(data.bodyLines);
        bottomText.text = data.bottom;

        if (data.textType == TooltipTextAlignType.Left)
        {
            headerText.alignment = TextAlignmentOptions.Left;
            bodyText.alignment = TextAlignmentOptions.Left;
            bottomText.alignment = TextAlignmentOptions.Left;
        }
        else if (data.textType == TooltipTextAlignType.Right)
        {
            headerText.alignment = TextAlignmentOptions.Right;
            bodyText.alignment = TextAlignmentOptions.Right;
            bottomText.alignment = TextAlignmentOptions.Right;
        }
        else if (data.textType == TooltipTextAlignType.Center)
        {
            headerText.alignment = TextAlignmentOptions.Center;
            bodyText.alignment = TextAlignmentOptions.Center;
            bottomText.alignment = TextAlignmentOptions.Center;
        }
    }

    protected void ApplySectionSeparators(TooltipData data)
    {
        bool hasHeaderBlock = !string.IsNullOrWhiteSpace(data.header) || data.headerLines.Count > 0;
        bool hasBodyBlock = !string.IsNullOrWhiteSpace(data.body) || data.bodyLines.Count > 0;
        bool hasBottomBlock = !string.IsNullOrWhiteSpace(data.bottom);

        if (bodyDivisionImage != null)
            bodyDivisionImage.gameObject.SetActive(hasHeaderBlock && hasBodyBlock);

        if (bottomDivisionImage != null)
            bottomDivisionImage.gameObject.SetActive(hasBottomBlock && hasBodyBlock);
    }

    public virtual void SettingTooltip(TooltipData data)
    {
        float width = minWidth;
        if (data.tooltipWidthType == TooltipWidthType.Auto)
        {
            width = Mathf.Max(width, headerText.preferredWidth);
            width = Mathf.Max(width, bodyText.preferredWidth);
            width = Mathf.Max(width, bottomText.preferredWidth);

            width = Mathf.Clamp(width, minWidth, maxWidth);

            width += (verticalLayoutGroup.padding.left + verticalLayoutGroup.padding.right);
        }
        else if (data.tooltipWidthType == TooltipWidthType.Fixed)
            width = maxWidth;

        tooltipRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    public void Place(RectTransform target, TooltipPositionType type)
    {
        // 1) 레이아웃 갱신 → 실제 툴팁 크기 확보
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);

        // 2) 실제 크기(피벗 0.5,0.5 가정)
        Vector2 tooltipHalf = tooltipRect.rect.size * 0.5f;
        Vector2 targetHalf = target.rect.size * 0.5f;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, target.transform.position, null, out Vector2 pos);

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
        headerLineList.Clear();

        bodyText.text = string.Empty;
        bodyLineList.Clear();

        bottomText.text = string.Empty;

        gameObject.SetActive(false);
    }
}