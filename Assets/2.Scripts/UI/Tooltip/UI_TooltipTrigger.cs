using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject provierObject;

    private ITooltipProvider provider;
    private TooltipData tooltipData;
    private RectTransform target;

    private void Awake()
    {
        provider = provierObject.GetComponent<ITooltipProvider>();
        target = GetComponent<RectTransform>();
        tooltipData = new TooltipData();

        if (provider == null)
            Debug.LogError($"{gameObject.name}: ITooltipProvider¾øÀ½");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (provider == null || tooltipData == null)
            return;

        if (provider.TryTooltipData(tooltipData))
            Managers.UI.ShowTooltip(tooltipData, target);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (provider == null)
            return;

        Managers.UI.HideTooltip();
    }

    private void OnDisable()
    {
        if (provider == null)
            return;

        Managers.UI.HideTooltip();
    }

    private void OnApplicationQuit()
    {
        provider = null;
    }
}