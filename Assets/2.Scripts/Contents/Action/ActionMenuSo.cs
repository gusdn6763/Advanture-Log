using UnityEngine;

public abstract class ActionMenuSo : ScriptableObject
{
    [field: SerializeField] public InputAction InputAction { get; protected set; }

    public abstract bool IsAvailable(BaseEntity target);
    public abstract void Execute(BaseEntity target);
}