using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Game/Entity/Player", fileName = "Player")]
public class PlayerSo : ActorEntitySo
{
    [Header("캐릭터 선택창 직업 설명")]
    [SerializeField] private LocalizedString jobDescription;

    [Header("플레이어 규칙")]
    [SerializeField] protected PlayerRuleSo playerRuleSo;

    public LocalizedString JobDescription => jobDescription; 
}