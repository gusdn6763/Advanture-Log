using UnityEngine;
using UnityEngine.Localization;

public abstract class ActionMenuSo : ScriptableObject
{
    [SerializeField] private LocalizedString actionName;

    public LocalizedString ActionName => actionName;

    public abstract bool IsAvailable(BaseEntity target);
    public abstract void Execute(BaseEntity target);
}