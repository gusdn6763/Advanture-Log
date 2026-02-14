public interface ICharacterCreationSection
{
    public void Refresh();  //시작 갱신
    public bool IsValid(); //조건 체크
    void Apply(GameStartData data); //데이터 전달
}