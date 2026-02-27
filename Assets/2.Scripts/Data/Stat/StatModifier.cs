public class StatModifier
{
    public float Value { get; private set; }            //값
    public CalculateType Type { get; private set; } //계산 타입
    public object Source { get; private set; }          //버프 출처 - 디버깅용

    public StatModifier(float value, CalculateType type, object source)
    {
        Value = value;
        Type = type;
        Source = source;
    }
}
