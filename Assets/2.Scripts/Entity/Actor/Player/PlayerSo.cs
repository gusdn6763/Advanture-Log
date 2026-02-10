using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSo : ActorEntitySo
{
    public override ObjectType ObjectType { get; protected set; } = ObjectType.Player;

    [SerializeField] private SerializedDictionary<MainStatType, int> baseMainStat;
    [SerializeField] private SerializedDictionary<SubStatType, float> baseSubStat;

    public IReadOnlyDictionary<MainStatType, int> BaseMainStat { get => baseMainStat; }
}