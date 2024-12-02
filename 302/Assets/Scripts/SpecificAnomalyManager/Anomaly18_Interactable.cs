using System.Collections;
using UnityEngine;

public class Anomaly18_Interactable : SCH_AnomalyInteractable
{
    /**********
     * fields *
     **********/

    // 가변 수치
    public float duration;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly18_Prefab";

    /*************************************
     * implementation: SCH_AnomalyObject *
     *************************************/

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        Log("Call `FadeAsync` asynchronously");
        StartCoroutine(FadeAsync());

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 지속시간 동안 투명해지다가 사라지는 메서드
    private IEnumerator FadeAsync()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        Color color;
        float timeStart = Time.time;
        float time, alpha;

        foreach (Collider collider in colliders) {
            collider.enabled = false;
        }

        foreach (Renderer renderer in renderers) {
            foreach (Material material in renderer.materials) {
                material.renderQueue = 3000;
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.SetFloat("_Mode", 2);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_ZWrite", 0);
            }
        }

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
