public class Anomaly6Manager : AbstractAnomalyController
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly6Manager";

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = base.Awake_();

        Log("Call `StartAnomaly` begin");
        if (StartAnomaly()) {
            Log("Call `StartAnomaly` success");
        } else {
            Log("Call `StartAnomaly` failed", mode: 1);
            res = false;
        }

        return res;
    }
}
