using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly18_Prefab : MonoBehaviour
{
    /***************
     * new methods *
     ***************/

    // 지속시간 동안 투명해지다가 사라지는 함수
    public IEnumerator FadeAsync(float duration)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        Color color;
        float timeStart = Time.time;
        float time, alpha;

        yield return null;

        while ((time = Time.time - timeStart) < duration) {
            alpha = 1.0f - time / duration;

            foreach (Renderer renderer in renderers) {
                foreach (Material material in renderer.materials) {
                    color = material.color;
                    color.a = alpha;
                    material.color = color;
                }
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
