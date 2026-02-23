using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ActionMenuButton : MonoBehaviour
{
    private TextMeshProUGUI text;
    private RectTransform rect;
    private Button button;

    private ActionMenuSo action;
    private BaseEntity target;

    public float Height => rect.sizeDelta.y;
    public Button Button => button;

    public void Init()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        text = GetComponentInChildren<TextMeshProUGUI>(true);

        gameObject.SetActive(false);
    }

    public void Setup(ActionMenuSo action, BaseEntity target)
    {
        this.action = action;
        this.target = target;

        button.SetClick(InvokeAction);
    }

    private void OnDisable()
    {
        button.ClearClick();
    }

    private void InvokeAction()
    {
        action.Execute(target);
    }

    public void SetText(string str)
    {
        if (text != null)
            text.text = str;
    }
}