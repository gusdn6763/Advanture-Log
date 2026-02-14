public class StatModifier
{
    public readonly float Value;            //값
    public readonly StatCalculateType Type; //계산 타입
    public readonly object Source;          //버프 출처 - 디버깅용

    public StatModifier(float value, StatCalculateType type, object source)
    {
        Value = value;
        Type = type;
        Source = source;
    }
}
