using System.Collections;
using UnityEngine;

public class Anomaly6_Cake : SCH_AnomalyInteractable
{
    /**********
     * fields *
     **********/

    public float duration;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly6_Cake";

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
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        float timeStart = Time.time;
        float time;

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
            float alpha = 1.0f - time / duration;

            foreach (Renderer renderer in renderers) {
                foreach (Material material in renderer.materials) {
                    Color color = material.color;

                    color.a = alpha;
                    material.color = color;
                }
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
