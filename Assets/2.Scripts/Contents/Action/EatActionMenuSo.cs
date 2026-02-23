using UnityEngine;

[CreateAssetMenu(menuName = "Game/Action/EatActionMenuSo", fileName = "EatActionMenuSo")]
public class EatActionMenuSo : ActionMenuSo
{
    public override bool IsAvailable(BaseEntity target)
    {
        return true;
    }
    public override void Execute(BaseEntity target)
    {
        Managers.UI.OpenUi(UiType.Inventory, target);
        Debug.Log("정보 보기 완료");
    }
}