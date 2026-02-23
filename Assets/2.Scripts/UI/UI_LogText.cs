using TMPro;
using UnityEngine;

public class UI_LogText : MonoBehaviour
{
    private TextMeshProUGUI text;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color previewColor;

    public void Init()
    {
        text = GetComponent<TextMeshProUGUI>();

        text.text = string.Empty;
        text.color = defaultColor;
    }

    public void SetText(string message)
    {
        text.text = message;
    }

    public void ColorChange()
    {
        text.color = previewColor;
    }
}
