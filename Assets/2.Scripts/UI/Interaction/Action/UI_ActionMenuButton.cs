using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ActionMenuButton : MonoBehaviour, IPointerEnterHandler
{
    private TextMeshProUGUI text;
    private RectTransform rect;
    private Button button;
    private Graphic targetGraphic;

    private Color normalColor;
    private Color highlightColor;

    private ActionMenuSo action;
    private BaseEntity target;

    private UI_ActionMenuBar owner;
    private int myIndex;

    public float Height => rect.sizeDelta.y;
    public Button Button => button;

    public void Init()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        owner = GetComponentInParent<UI_ActionMenuBar>(true);
        text = GetComponentInChildren<TextMeshProUGUI>(true);

        targetGraphic = button.targetGraphic; // 보통 Image
        if (targetGraphic != null)
        {
            normalColor = targetGraphic.color;
            // Button ColorBlock의 highlightedColor를 활용
            highlightColor = button.colors.highlightedColor;
        }

        gameObject.SetActive(false);
    }

    public void Setup(ActionMenuSo action, BaseEntity target, int index)
    {
        this.action = action;
        this.target = target;
        this.myIndex = index;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(InvokeAction);
    }

    private void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveAllListeners();
    }

    private void InvokeAction()
    {
        action.Execute(target);
    }

    public void SetText(string str)
    {
        if (text != null) text.text = str;
    }

    public void SetHighlighted(bool value)
    {
        if (targetGraphic == null) return;
        targetGraphic.color = value ? highlightColor : normalColor;
    }

    // 마우스 올리면 해당 버튼을 "선택" 상태로
    public void OnPointerEnter(PointerEventData eventData)
    {
        owner.SetIndex(myIndex);
    }
}