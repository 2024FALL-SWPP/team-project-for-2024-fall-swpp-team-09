using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly18_Object : MonoBehaviour
{
    /***************
     * new methods *
     ***************/

    // 초기 위치부터 최종 위치까지 지속시간 동안 움직이는 함수
    public IEnumerator MoveAsync(Vector3 positionInit, Vector3 displacement, float duration)
    {
        float timeStart = Time.time;
        float time;

        yield return null;

        while ((time = Time.time - timeStart) < duration) {
            transform.position = positionInit + displacement * time / duration;

            yield return null;
        }
    }
}
