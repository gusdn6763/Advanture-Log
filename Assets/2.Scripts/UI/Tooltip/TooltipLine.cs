using TMPro;
using UnityEngine;

public class TooltipLine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leftText;
    [SerializeField] private TextMeshProUGUI rightText;

    public void SetText(string left, string right)
    {
        leftText.text = left;
        rightText.text = right;
    }
}