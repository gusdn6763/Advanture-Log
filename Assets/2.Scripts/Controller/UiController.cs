using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private UI_ActionMenuOverlay actionMenuBar;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject quest;

    public void Init()
    {
        actionMenuBar.Init();
        Managers.UI.BindSceneUi(this);
    }

    public void OpenUi(UiType type, BaseEntity target)
    {
        switch (type)
        {
            case UiType.ActionMenu:
                actionMenuBar.Open(target);
                break;
            case UiType.Inventory:
                inventory.gameObject.SetActive(true);
                break;
            case UiType.Quest:
                quest.gameObject.SetActive(true);
                break;
        }
    }

    public void ChangeBackground(Area currentArea)
    {
        if (currentArea)
            backgroundImage.sprite = currentArea.BackgroundSprite;
        else
            backgroundImage.sprite = null;
    }
}