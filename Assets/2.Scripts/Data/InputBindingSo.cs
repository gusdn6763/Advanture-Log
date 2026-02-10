using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Rule/InputBinding", fileName = "InputBindingSo")]
public class InputBindingSo : ScriptableObject
{
    [SerializeField] private SerializedDictionary<InputAction, KeyCode> keyDic;

    public SerializedDictionary<InputAction, KeyCode> KeyDic => keyDic;
}