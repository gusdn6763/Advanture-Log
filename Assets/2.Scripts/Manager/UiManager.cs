using UnityEngine;
using System.Collections.Generic;

public class UiManager : MonoBehaviour
{
    public UiController SceneUi { get; private set; }

    public void Init()
    {
        tooltipController.Init();
        settingUI.Init();
    }

    public void BindSceneUi(UiController ui)
    {
        SceneUi = ui;
    }

    public void OpenUi(UiType type, BaseEntity target = null)
    {
        if (SceneUi == null)
            return;

        SceneUi.OpenUi(type, target);
    }

    #region ÆË¾÷

    private List<UI_PopupBase> popupList = new List<UI_PopupBase>();
    private int sortingOrder = 10;


    public void OpenPopupUI(UI_PopupBase popup)
    {
        if (popupList.Contains(popup) == false)
        {
            popupList.Add(popup);

            RecomputeOrders();
        }
    }
    public void ClosePopupUI()
    {
        if (popupList.Count == 0)
            return;

        ClosePopupUI(popupList[popupList.Count - 1]);
    }
    public void ClosePopupUI(UI_PopupBase popup)
    {
        if (popupList.Count == 0)
            return;

        sortingOrder--;

        popupList.Remove(popup);

        RecomputeOrders();
    }
    public void CloseAllPopupUI()
    {
        while (popupList.Count > 0)
            ClosePopupUI(popupList[popupList.Count - 1]);
    }
    private void RecomputeOrders()
    {
        for (int i = 0; i < popupList.Count; i++)
        {
            popupList[i].SetOrder(sortingOrder + i);
        }
    }
    #endregion

    #region ·Î±×Ã¢
    public void ShowLog(string message)
    {
        if (SceneUi == null)
            return;

        SceneUi.CreateMessage(message);
    }
    #endregion

    #region ¼³Á¤Ã¢
    [Header("¼³Á¤Ã¢")][SerializeField] private UI_Setting settingUI;
    public void OpenSettingUI()
    {
        settingUI.Open();
    }
    #endregion

    #region ÅøÆÁÃ¢
    [Header("ÅøÆÁ")]
    [SerializeField] private TooltipController tooltipController;

    public void ShowTooltip(ITooltipProvider provider, RectTransform target)
    {
        tooltipController.ShowTooltip(provider, target);
    }

    public void HideTooltip(RectTransform target)
    {
        tooltipController.HideTooltip(target);
    }
    #endregion
}