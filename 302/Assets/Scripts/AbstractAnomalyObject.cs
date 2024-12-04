public abstract class AbstractAnomalyObject : AbstractBehaviour
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "AbstractAnomalyObject";

    // 이상현상 컨트롤러
    public AbstractAnomalyController Controller { get; set; }

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
