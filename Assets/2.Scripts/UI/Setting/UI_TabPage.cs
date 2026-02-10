using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class UI_TabPage : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;

    public Button Button => button;
    public Image ButtonImage => buttonImage;
}