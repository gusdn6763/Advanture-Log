using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private UI_ActionMenuBar actionMenuBar;
    [SerializeField] private GameObject selectBar;

    public void Init(AreaController areaController, InteractionController interactionController)
    {
        areaController.OnAreaChanged += ChangeBackground;
        interactionController.OnTargetChanged += SetSelected;
        interactionController.OnActionMenuChanged += NavigateMenu;
        interactionController.OnTargetSelect += OpenMenu;
        interactionController.OnCancle += CloseMenu;
    }

    public void ChangeBackground(Area currentArea)
    {
        if (currentArea)
            backgroundImage.sprite = currentArea.BackgroundSprite;
        else
            backgroundImage.sprite = null;

        SetSelected(Managers.Object.Player);
    }

    public void SetSelected(BaseEntity entity)
    {
        selectBar.gameObject.SetActive(true);
        selectBar.transform.position = entity.Position;
    }

    public void OpenMenu(BaseEntity target)
    {
        actionMenuBar.OpenMenu(target);
    }

    public void NavigateMenu(bool up)
    {
        actionMenuBar.NavigateMenu(up);
    }

    public void CloseMenu()
    {
        actionMenuBar.CloseMenu();
    }
}