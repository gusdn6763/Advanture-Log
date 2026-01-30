using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/KeyBinding")]
public class InputBindingSo : ScriptableObject
{
    [SerializeField] private SerializedDictionary<InputAction, KeyCode> keyDic;

    public SerializedDictionary<InputAction, KeyCode> KeyDic => keyDic;
}