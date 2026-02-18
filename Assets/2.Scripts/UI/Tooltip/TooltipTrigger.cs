using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject provierObject;

    private ITooltipProvider provider;
    private RectTransform target;

    private void Awake()
    {
        provider = provierObject.GetComponent<ITooltipProvider>();
        target = GetComponent<RectTransform>();

        if (provider == null)
            Debug.LogError($"{gameObject.name}: ITooltipProvider¾øÀ½");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (provider == null || target == null)
            return;

        Managers.UI.ShowTooltip(provider, target);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (provider == null || target == null)
            return;

        Managers.UI.HideTooltip(target);
    }

    private void OnDisable()
    {
        if (provider == null || target == null)
            return;

        Managers.UI.HideTooltip(target);
    }

    private void OnApplicationQuit()
    {
        provider = null;
    }
}