abstract public class AbstractStageObserver : AbstractBehaviour
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "AbstractStageObserver";

    /*******************
     * virtual methods *
     *******************/

    // 단계 변경 시 불리는 메서드
    public virtual bool UpdateStage()
    {
        return true;
    }
}
