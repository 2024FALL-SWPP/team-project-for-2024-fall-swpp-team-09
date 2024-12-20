public class Anomaly00Controller : AbstractAnomalyObject
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly00Controller";

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        GameManager.Instance.SetStageClear();

        return res;
    }
}
