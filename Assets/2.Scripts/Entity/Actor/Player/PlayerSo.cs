using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Entity/Player", fileName = "Player")]
public class PlayerSo : ActorEntitySo
{
    [Header("기본 메인 스탯")]
    [SerializeField] private SerializedDictionary<MainStatType, int> baseMainStat;

    [Header("기본 서브 스탯")]
    [SerializeField] private SerializedDictionary<SubStatType, float> baseSubStat;

    public IReadOnlyDictionary<MainStatType, int> BaseMainStat { get => baseMainStat; }
}