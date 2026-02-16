using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Game/Action/InfoActionMenuSo", fileName = "InfoActionMenuSo")]
public class InfoActionMenuSo : ActionMenuSo
{
    public override bool IsAvailable(BaseEntity target)
    {
        return true;
    }
    public override void Execute(BaseEntity target)
    {
        Managers.UI.OpenUi(UiType.Info, target);
    }
}