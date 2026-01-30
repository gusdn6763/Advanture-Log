using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using System;
using UnityEngine.Localization.Settings;

public class UiManager : MonoBehaviour
{
    private List<UI_PopupBase> popupList = new List<UI_PopupBase>();
    private int sortingOrder = 10;

    public bool IsOpenUi { get { return popupList.Count > 0; } }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ClosePopupUI();
    }

    #region ÆË¾÷
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

}