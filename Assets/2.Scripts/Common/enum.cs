/// <summary>
/// 메인 스탯 타입 
/// </summary>
public enum MainStatType
{
    // 기본 스탯
    Strength,           //힘
    Dexterity,          //민첩
    Constitution,       //건강
    Intelligence,       //지능
    Perception,         //지각
}

/// <summary>
/// 서브 스탯 타입
/// </summary>
public enum SubStatType
{
    MaxHealth = 0,          // 최대 체력
    HealthRegen,            // 체력 회복
    MaxMana,                // 최대 마나
    ManaRegen,              // 마나 회복

    MinPhysicalDamage,      // 최소 물리 피해
    MaxPhysicalDamage,      // 최대 물리 피해
    MinMagicDamage,         // 최소 마법 피해
    MaxMagicDamage,         // 최대 마법 피해

    PhysicalDefense,        // 물리 방어력
    MagicDefense,           // 마법 방어력

    CriticalChance,         // 치명타 확률
    CriticalDamage,         // 치명타 피해

    Accuracy,               // 명중
    Evasion,                // 회피
    AttackSpeed,            // 공격 속도

    // 탐색/유틸
    Detection,              // 감지
    CarryCapacity,          // 소지 한도(최대 무게)
}

/// <summary>
/// 스탯 계산 타입
/// </summary>
public enum StatCalculateType
{
    Flat,           // 고정 증감
    PercentAdd,     // 퍼센트 합산
    PercentMult,    // 퍼센트 곱산
    Absolute,       // 절대값 설정 (ex. HP = 50)
}

/// <summary>
/// 스탯 보여주는 타입
/// </summary>
public enum StatDisplayType
{
    Flat, 
    Percent
}

/// <summary>
/// 툴팁 타입
/// </summary>
public enum TooltipPositionType
{
    Left,
    Right,
    Up,
    Down
}

/// <summary>
/// 플레이어 자원
/// </summary>
public enum NeedType
{
    Hunger,
    Sleep,
}

/// <summary>
/// 플레이어 상태
/// </summary>
public enum NeedTier
{
    Good,
    Normal,
    Tired,
    Exhausted
}

/// <summary>
/// 랭크 타입
/// </summary>
public enum RankType
{
    None,
    One,
    Two,
}

/// <summary>
/// UiControlller에서 보여주는 ui타입
/// </summary>
public enum UiType
{
    None,
    ActionMenu,
    Inventory,
    Quest,
    Info,
}

/// <summary>
/// 입력값
/// </summary>
public enum InputAction
{
    None,
    Select,          //기본값-엔터키, 선택 확정
    Cancel,          //기본값-Esc, 취소
    ItemQuickSlot1,
    ItemQuickSlot2,
    ItemQuickSlot3,
    ItemQuickSlot4,
    ItemQuickSlot5,
    ItemQuickSlot6,
    ItemQuickSlot7,
    ItemQuickSlot8,
    ItemQuickSlot9
}

/// <summary>
/// 다국어 표현용
/// </summary>
public enum LocalizeKey
{
    UI_NameEmpty,
    UI_InvalidCharacters,
    UI_NameTooShort,
    MissionFailed,          // 임무 실패
    MissionCompleted,       // 임무 완료
    NotEnoughMoney,         // 돈 부족
    // ... 계속 추가
}