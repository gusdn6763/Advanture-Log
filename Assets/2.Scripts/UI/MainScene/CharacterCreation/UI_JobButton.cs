using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class UI_JobButton : MonoBehaviour, IPointerEnterHandler
{
    public event Action<string> OnHovered;
    public event Action<string> OnClicked;

    [SerializeField] private TextMeshProUGUI jobNameText;

    private Button button;
    private Image image;

    private PlayerSo playerSo;

    public void Init(PlayerSo playerSo)
    {
        this.playerSo = playerSo;

        button = GetComponent<Button>();
        image = GetComponent<Image>();

        image.sprite = playerSo.EntityImage;
        jobNameText.text = playerSo.ObjectName.GetLocalizedString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);

        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale locale)
    {
        jobNameText.text = playerSo.ObjectName.GetLocalizedString(locale);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string desc = playerSo.Description.GetLocalizedString();
        OnHovered?.Invoke(desc);
    }

    private void OnClick()
    {
        OnClicked?.Invoke(playerSo.Id);
    }
}