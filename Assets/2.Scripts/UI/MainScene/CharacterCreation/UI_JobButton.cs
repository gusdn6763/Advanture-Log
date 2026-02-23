using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_JobButton : MonoBehaviour, IPointerEnterHandler
{
    public event Action<PlayerSo> OnHovered;
    public event Action<UI_JobButton> OnClicked;

    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI jobNameText;

    public PlayerSo ButtonPlayerSo { get; set; }

    public void Init(PlayerSo playerSo)
    {
        ButtonPlayerSo = playerSo;

        UpdateName();
        image.sprite = playerSo.EntityImage;

        button.SetClick(OnClick);
    }

    public void UpdateName()
    {
        jobNameText.text = ButtonPlayerSo.ObjectNameLocalized.GetLocalizedString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHovered?.Invoke(ButtonPlayerSo);
    }

    private void OnClick()
    {
        OnClicked?.Invoke(this);
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }
}