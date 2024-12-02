using System.Collections;
using UnityEngine;

public class Anomaly18_Object : SCH_AnomalyObject
{
    /**********
     * fields *
     **********/

    // 가변 수치
    public Vector3 position;
    public Vector3 direction;

    public float valueNormal;
    public float valueAnomaly;

    public float duration;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly18_Object";

    /*************************************
     * implementation: SCH_AnomalyObject *
     *************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        transform.position = position + direction * valueAnomaly;
        Log("Set position success");

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        Log("Call `MoveAsync` asynchronously");
        StartCoroutine(MoveAsync());

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 초기 위치부터 최종 위치까지 지속시간 동안 움직이는 메서드
    private IEnumerator MoveAsync()
    {
        Vector3 positionInit = position + direction * valueAnomaly;
        Vector3 displacement = direction * (valueNormal - valueAnomaly);
        float timeStart = Time.time;
        float time;

        yield return null;

        while ((time = Time.time - timeStart) < duration) {
            transform.position = positionInit + displacement * time / duration;

            yield return null;
        }

        enabled = false;
    }
}
