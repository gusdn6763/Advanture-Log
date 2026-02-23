using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.UI;

public class UI_KeyButton : MonoBehaviour
{
    public event Action<UI_KeyButton> OnClicked;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private Button button;
    [SerializeField] private Image buttonBackground;          // 버튼 배경 이미지
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;

    public InputAction Action { get; private set; }
    public InputActionData ActionData { get; private set; }

    public void Init(InputAction action, InputActionData inputActionData)
    {
        Action = action;
        ActionData = inputActionData;

        button.SetClick(OnClickRebind);
    }

    public void Refresh()
    {
        actionNameText.text = ActionData.DisplayName.GetLocalizedString();

        keyText.text = ActionData.keyCode.ToString();
    }

    private void OnClickRebind()
    {
        OnClicked?.Invoke(this);
    }

    public void ChangeKey(KeyCode keyCode)
    {
        ActionData.keyCode = keyCode;
    }

    public void SetSelected(bool selected)
    {
        buttonBackground.color = selected ? selectedColor : normalColor;
    }
}