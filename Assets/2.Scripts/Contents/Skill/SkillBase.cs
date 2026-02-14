public abstract class SkillBase
{
    public SkillSo SkillData { get; set; }
    public ActorEntity Owner { get; protected set; }
    public float RemainCoolTime { get; set; }

    protected SkillBase(ActorEntity owner, SkillSo skillSo)
    {
        Owner = owner;
        SkillData = skillSo;
    }

    public virtual void DoSkill()
    {

    }
}