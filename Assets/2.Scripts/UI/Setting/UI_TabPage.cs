using System;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button), typeof(Image))]
public class UI_TabPage : MonoBehaviour
{
    private Button button;
    private Image buttonImage;

    public void Init()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
    }

    public void SetSprite(Sprite sprite)
    {
        buttonImage.sprite = sprite;
    }

    public void BindClick(Action onClick)
    {
        button.SetClick(onClick);
    }
}