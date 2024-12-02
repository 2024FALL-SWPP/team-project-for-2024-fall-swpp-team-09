public class SCH_AnomalyObject : SCH_Behaviour
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "SCH_AnomalyObject";

    // 이상현상 매니저
    public SCH_AnomalyManager Manager { get; set; }

    /*******************
     * virtual methods *
     *******************/

    // 이상현상을 시작하는 메서드
    public virtual bool StartAnomaly()
    {
        return true;
    }

    // 이상현상을 초기화하는 메서드
    public virtual bool ResetAnomaly()
    {
        return true;
    }
}
