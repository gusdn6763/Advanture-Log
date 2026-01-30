using UnityEngine;
using UnityEngine.UIElements;

public class UI_PopupBase : InitBase
{
    private Canvas canvas;
    public override bool Init()
    {
        if (!base.Init())
            return false;

        canvas = GetComponentInParent<Canvas>();

        return true;
    }

    protected virtual void OnEnable()
    {
        Managers.UI.OpenPopupUI(this);
    }

    protected virtual void OnDisable()
    {
        Managers.UI.ClosePopupUI(this);
    }

    public void SetOrder(int order)
    {
        canvas.sortingOrder = order;
    }
}