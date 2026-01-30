public enum InteractionMode
{
    None = 0,
    Target,   // 오브젝트 선택 이동
    Menu,     // 선택 대상에 대한 커맨드(메뉴/행동 선택)
}

public enum InputAction
{
    None,
    MoveUp,         //기본값-위키, 타겟 선택
    MoveDown,       //기본값-아래키, 타겟 선택
    MoveLeft,       //기본값-왼쪽키, 타겟 선택
    MoveRight,      //기본값-오른쪽키, 타겟 선택
    Select,         //기본값-엔터키, 선택 확정
    Cancel,         //기본값-Esc, 취소
    OpenMenu,        //기본값-Enter, 메뉴 오픈
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