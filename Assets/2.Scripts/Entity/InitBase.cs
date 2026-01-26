using UnityEngine;

/// <summary>
/// 초기화용
/// </summary>
public class InitBase : MonoBehaviour
{
    [Header("확인용")]
    public bool _init = false;

    private void Awake()
    {
        Init();
    }

    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;

        return true;
    }
}
