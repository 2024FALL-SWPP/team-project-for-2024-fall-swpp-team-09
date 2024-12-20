using UnityEngine;

public class AnomalyForDeletion : AbstractAnomalyObject
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "AnomalyForDeletion";

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        gameObject.SetActive(false);
        Log($"Delete `{gameObject.name}`.");

        return res;
    }
}
