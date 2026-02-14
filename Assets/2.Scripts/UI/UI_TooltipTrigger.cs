using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject providerObject;

    private ITooltipProvider provider;

    private void Awake()
    {
        provider = providerObject.GetComponent<ITooltipProvider>();

        if (provider == null)
            Debug.LogError($"{gameObject.name}: ITooltipProvider¾øÀ½");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (provider == null) 
            return;

        string message = provider.GetTooltipContent();

        if (string.IsNullOrEmpty(message)) 
            return;

        Managers.UI.ShowTooltip(provider, message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (provider == null)
            return;

        Managers.UI.HideTooltip(provider);
    }

    private void OnDisable()
    {
        if (provider == null)
            return;

        Managers.UI.HideTooltip(provider);
    }

    private void OnApplicationQuit()
    {
        provider = null;
    }
}